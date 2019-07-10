# Discord Like Chat Clone.

Inspired by IRC, master server hosts virtual servers with channels, users can create servers, friends, private messaging,..

### Why
Discord sucks. It's constantly having problems or is down. The Discord client is a resource hog. It uses half a CPU core just to blink the fuckign cursor whit eating half a gig of ram.

I know this isn't going to take off and become the next big thing, im building this mainly to see if I can, and to have a fallback service to talk with my friends if discord dies again.

# Protocol

This is a custom binary based protocol, parsing and creating is unsafe code. I'm casting byte arrays directly into a struct, achieving sub 0.09ms speeds for deserialization paired with high performance SocketAsyncEventArgs based sockets. Including poorly implemented compression. Of course this entire thing runs completely GC neutral. No allocations during normal operation. 

Here's the packet layout.

```C
STRUCT
{
    SHORT LENGTH;
    BYTE ID;
    BYTE[] PAYLOAD;
}
```

where `PAYLOAD` would be other fields.
In C# that looks like this:

```cs
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MsgDataRequest
    {
        public short Length { get; private set; }
        public PacketType Id { get; private set; }
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public int Param { get; set; }
        public MsgDataRequestType Type { get; set; }

        // Generic Create Method. Allocated on the stack, returning dereferenced pointer.

        public static MsgDataRequest Create(int userId, int targetId,int param, MsgDataRequestType type)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->TargetId = targetId;
            msg->Param = param;
            msg->Type = type;

            return *msg;
        }
        public static MsgDataRequest CreateFriendListRequest(int userId)
        {
            var msg = stackalloc MsgDataRequest[1];
            msg->Length = (short)sizeof(MsgDataRequest);
            msg->Id = PacketType.MsgDataRequest;
            msg->UserId = userId;
            msg->Type = MsgDataRequestType.Friends;

            return *msg;
        }
```

It's essential to read the first two bytes and read them as an 16 bit integer `BitConverter.ToInt16(array,0);` for example, that value will be the amount of bytes required for this packet. Then, if compression is enabled, run a deflate stream over it to decompress it. It's important that the deflate stream skips the header, so the first two bytes, as they represent the length of the compressed chunk and obviously aren't compressed.

Now you got your complete packet assembled and ready to go, read the next byte - the third byte - this will give you the packet Id. 

Based on the third byte, you route the packet to the relevant packet handler.

Now, inside the packet handler you gotta start thinking about Serialization and Deserialization, that's usually alot of code... so lets dig in..

```cs

        // Serialization :)
        public static implicit operator byte[] (MsgDataRequest msg)
        {
            var buffer = new byte[sizeof(MsgDataRequest)];
            fixed (byte* p = buffer)
                *(MsgDataRequest*)p = *&msg;
            return buffer;
        }
        
        // Deserialization :)
        public static implicit operator MsgDataRequest(byte[] msg)
        {
            fixed (byte* p = msg)
                return *(MsgDataRequest*)p;
        }

```
I swear that's it. 

A typical use case would be something like
```cs

byte[] buffer = Socket.Receive();
LoginPacketHandler(buffer);

void LoginPacketHandler(byte[] packet);
{

    MsgLogin msg = packet; //Deserialization

    msg.UniqueId = Authenticate(msg.GetUsername(),msg.GetPassword());

    Socket.Send(msg); // Serialization

}
```

which is pretty fucking neat.

Anyways, everything should be very straight forward to implement in lower level languages, I've tried to keep it very simple and friendly.


# Login sequence:

Client sends MsgLogin with user/pass, server responds with MsgLogin. If it contains a UniqueId other than ZERO (0), assign this UniqueId to your client. You are now logged in.
If the MsgLogin.UniqueId is 0, either wrong user/pass was entered or the account doesn't exist.
The server will also send a MsgUser packet which will contain the same UniqueId telling you that this packet contains all the info the server knows about your User. Eg. the Nickname. 

That's it. You are now authenticated, logged in, and the client has all the information needed to start the loading sequence.

# Loading sequence:

## Friends:
First we will request your friends from the server. Send a MsgDataRequest with type Friends, the server will reply with a MsgUser packet for every friend. After the last MsgUser, the server will send MsgDataRequest with type Friends to signal the client that it has received all friends.

The client now has to send further MsgDataRequest packets with type Message and TargetId set to the friend's Id - for every friend - and the server will reply with MsgText packets
ServerId set to 0 (which indicates your local 'server' for direct messages, basically the imaginary server you use to talk to your friends privately) and AuthorId being set to a Friend's ID. Every MsgText at this stage is a message you got while you were offline.

## Servers:

Pretty much the same as for friends. MsgDataRequest with type Servers, server replies with MsgVServer packets for every server, then you send MsgDataRequest Type Channels for each VServer, the server replies with MsgChannel, then for each channel you send a MsgDataRequest type Message and TargetId = ServerId, Param = ChannelId.
