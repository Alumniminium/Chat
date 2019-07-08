# Login sequence:                                                     // Establish a connection and get authenticated with the server.

* Client  ->              Connect                     ->  Server      // Socket.Connect(ServerIp,ServerPort);
 if connected
* Client  ->              Send MsgLogin               ->  Server      // MsgLogin with Username and Password set.
 else
* Sleep 5s and retry

* Server  ->              Send MsgLogin               ->  Client      // Returns MsgLogin with UniqueId for Client.

if MsgLogin.UniqueId != 0
* You are now logged in.
else
* Wrong user/Password

## End Login sequence.

# Loading sequence:

## Friends:
* Client  ->              Send MsgFriendList          ->  Server      // Type: RequestList
for each friend
* Server  ->                Send MsgFriend            ->  Client      // Type: Info
* Client  ->              MsgRequestMessages          ->  Server      // Type: Offline, AuthorId: friend.id
* Server  ->                    MsgText               ->  Client      // Type: Offline, AuthorId: friend.id or Type.NoOfflineMessages
end loop

## Servers:
* Client  ->             Send MsgVServerList          ->  Server      // Type: RequestList
for each server
* Server  ->                Send MsgVServer           ->  Server      // One or more packets.


