using System;
using System.Collections.Generic;

using ServerLib.Interface;

namespace ServerLib.Impl
{
    class AdvancedAsyncStateObject : DefaultAsyncStateObject, IAdvancedAsyncStateObject
    {
        public byte[] _endPacket { get; set; }
        private int _endPacketSize { get { return _endPacket.Length; } }

        private byte[] _backupBuffer;
        private int _backupBufferSize = 0;
        /// <summary>
        /// 수신한 데이터(_receiveBuffer)의 마지막이 엔드패킷인지 여부 확인
        /// 마지막 자리부터 검사를 시작해서 동일하지 않으면 false반환
        /// </summary>
        public bool _haveEndPacket
        {
            get
            {
                int count = _receivedSize - 1;
                for (int i = _endPacketSize - 1; i > -1; i--)
                {
                    if (_endPacket[i] != _receiveBuffer[count--])
                        return false;
                }

                return true;
            }
        }
        /// <summary>
        /// 수신한 데이터를 목록으로 반환
        /// 엔드패킷 기준으로 메세지를 분할하여 반환한다.
        /// </summary>
        public List<byte[]> _messageList
        {
            get
            {
                ClearReceiveByte();
                return CreateMessageList(_backupBuffer, _backupBufferSize, _endPacket);
            }
        }

        public AdvancedAsyncStateObject(byte[] endPacket, IAsyncSocketClient client)
            : base(client)
        {
            _endPacket = endPacket;
        }
        /// <summary>
        /// 수신한 버퍼(_receiveBuffer)를 초기화하고 _backupBuffer에 수신한 데이터를 저장한다.
        /// </summary>
        public void ClearReceiveByte()
        {
            _backupBufferSize = _receivedSize;
            _backupBuffer = new byte[_backupBufferSize];
            Array.Copy(_receiveBuffer, 0, _backupBuffer, 0, _backupBufferSize);

            _receiveBuffer.Initialize();
            _receivedSize = -1;
        }
        /// <summary>
        /// 엔드패킷을 기준으로 분할 된 메세지 목록 반환
        /// </summary>
        /// <param name="readByte">수신 데이터</param>
        /// <param name="readSize">수신 데이터 사이즈</param>
        /// <param name="endByte">엔드패킷</param>
        /// <returns>수신 메세지 리스트</returns>
        private List<Byte[]> CreateMessageList(byte[] readByte, int readSize, byte[] endByte)
        {
            List<byte[]> messageList = new List<byte[]>();

            int startIndex = 0;
            int endIndex = 0;

            while (true)
            {
                endIndex = FindEndPacketIndex(readByte, startIndex, readSize, endByte);

                if (endIndex == -1)
                    break;

                int size = endIndex - startIndex;
                byte[] temp = new byte[size];
                Array.Copy(readByte, startIndex, temp, 0, size);

                messageList.Add(temp);
                startIndex = endIndex + endByte.Length;
            }

            return messageList;
        }
        /// <summary>
        /// 메세지를 처음부터 확인하여 엔드패킷의 위치를 반환
        /// </summary>
        /// <param name="sourceByte">위치를 확인할 바이트</param>
        /// <param name="start">시작위치</param>
        /// <param name="end">종료위치</param>
        /// <param name="endByte">엔드패킷</param>
        /// <returns>엔드 패킷 시작 위치</returns>
        private int FindEndPacketIndex(byte[] sourceByte, int start, int end, byte[] endByte)
        {
            for (int index = start; index < end; index++)
            {
                if (sourceByte[index] == endByte[0])
                {
                    if (IsEndPacket(sourceByte, index, endByte))
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
        /// <summary>
        /// sourceByte의 index위치부터 시작해서 엔드패킷과 일치하는지 확인
        /// </summary>
        /// <param name="sourceByte">검색할 바이트</param>
        /// <param name="index">검색 시작 위치</param>
        /// <param name="endByte">엔드페킷</param>
        /// <returns></returns>
        private bool IsEndPacket(byte[] sourceByte, int index, byte[] endByte)
        {
            for (int endIndex = 0; endIndex < endByte.Length; endIndex++)
            {
                if (sourceByte.Length < index)
                    return false;

                if (sourceByte[index++] != endByte[endIndex])
                    return false;
            }

            return true;
        }
    }
}
