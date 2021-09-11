using System.Collections.Generic;
using System;

namespace ServerLib.Event
{
    /// <summary>
    /// 클라이언트 데이터를 받았을 때 발생하는 이벤트
    /// </summary>
    public delegate void AsyncSocketReceiveEventHandler(object sender, AsyncSocketReceiveEventArgs e);

    public class AsyncSocketReceiveEventArgs : EventArgs
    {
        public int _receiveByteSize { get; private set; }
        public List<string> _receiveMessageList { get; private set; }

        public AsyncSocketReceiveEventArgs(int receiveByteSize, List<string> receiveMessageList)
        {
            _receiveByteSize = receiveByteSize;
            _receiveMessageList = receiveMessageList;
        }
    }
}
