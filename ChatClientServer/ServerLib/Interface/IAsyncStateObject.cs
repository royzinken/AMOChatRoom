using System.Collections.Generic;

namespace ServerLib.Interface
{
    /// <summary>
    /// 비동기 데이터 리시브를 위한 버퍼링 객체
    /// </summary>
    public interface IAsyncStateObject
    {
        int _bufferSize { get; }
        byte[] _receiveBuffer { get; set; }
        int _receivedSize { get; set; }

        IAsyncSocketClient _client { get; }
    }
}
