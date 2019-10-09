using System.Collections.Generic;
using System.Net.Sockets;

namespace Universal.IO.Sockets.Pools
{
    public class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> _pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            lock (_pool)
                _pool.Push(item);
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (_pool)
                return _pool.Pop();
        }

        public int Count
        {
            get { return _pool.Count; }
        }

    }
}