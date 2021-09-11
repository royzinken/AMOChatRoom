using System;

namespace ServerLib.Event
{
    /// <summary>
    /// 클라이언트에서 데이터를 전달 할 때 발생하는 이벤트
    /// </summary>
    public delegate void AsyncSocketSendEventHandler(object sender, AsyncSocketSendEventArgs e);

    public class AsyncSocketSendEventArgs : EventArgs
    {
        public int _sendBytes { get; private set; }

        public AsyncSocketSendEventArgs(int sendBytes)
        {
            _sendBytes = sendBytes;
        }
    }
}
