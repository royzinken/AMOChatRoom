using System.Collections.Generic;

namespace ServerLib.Interface
{
    /// <summary>
    /// IAsyncStateObject를 확장하여 앤드 패킷 확인 추가
    /// </summary>
    public interface IAdvancedAsyncStateObject : IAsyncStateObject
    {
        byte[] _endPacket { get; set; }
        bool _haveEndPacket { get; }
        List<byte[]> _messageList { get; }

        void ClearReceiveByte();
    }
}
