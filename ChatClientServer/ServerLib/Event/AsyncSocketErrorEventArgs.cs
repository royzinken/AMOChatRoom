using System;

using ServerLib.Interface;

namespace ServerLib.Event
{
    /// <summary>
    /// 소켓 또는 동작 중 발생하는 오류 전달 이벤트
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AsyncSocketErrorEventHandler(object sender, AsyncSocketErrorEventArgs e);

    public class AsyncSocketErrorEventArgs : EventArgs
    {
        public Exception _asyncSocketException { get; private set; }

        public AsyncSocketErrorEventArgs(Exception exception)
        {
            _asyncSocketException = exception;
        }
    }
}
