using System;

namespace Universal.IO.Sockets
{
    public class NeutralBuffer
    {
        public byte[] ReceiveBuffer { get; set; }
        public byte[] SendBuffer { get; set; }
        public int BytesInBuffer { get; set; }
        public int BytesRequired { get; set; }
        public int BytesProcessed { get; set; }
        public byte[] MergeBuffer { get; set; }

        public Span<byte> GetSpan(int index)
        {
            return Span<byte>.Empty;
        }

        public NeutralBuffer(int receiveBufferSize = 3072, int sendBufferSize = 3072)
        {
            ReceiveBuffer = new byte[receiveBufferSize];
            SendBuffer = new byte[sendBufferSize];
            MergeBuffer = new byte[receiveBufferSize];
        }
    }
}