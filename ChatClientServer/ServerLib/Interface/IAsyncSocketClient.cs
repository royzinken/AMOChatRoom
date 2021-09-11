using System.Net.Sockets;
using System.Collections.Generic;
using ServerLib.Event;

namespace ServerLib.Interface
{
    /// <summary>
    /// 비동기 소켓 클라이언트
    /// </summary>
    public interface IAsyncSocketClient
    {
        /// <summary>
        /// 클라이언트 소켓
        /// </summary>
        Socket _clientSocket { get; }
        /// <summary>
        /// 클라이언트 아이디
        /// </summary>
        string _clientID { get; set; }
        /// <summary>
        /// 클라이언트 아이피
        /// </summary>
        string _clientIP { get; set; }
        /// <summary>
        /// 클라이언트 포트번호
        /// </summary>
        string _clientPort { get; set; }
        /// <summary>
        /// 클라이언트 접속자 이름
        /// </summary>
        string _clientName { get; set; }
        /// <summary>
        /// 원격지 연결 여부
        /// </summary>
        bool _isConnected { get; }

        /// <summary>
        /// 원격지 접속
        /// </summary>
        /// <param name="hostIPAddress">원격지 IP</param>
        /// <param name="port">원격지 Port</param>
        void Connect(string hostIPAddress, int port);
        /// <summary>
        /// 원격지 데이터 전달
        /// </summary>
        /// <param name="sendByte">전달데이터</param>
        void Send(byte[] sendByte);
        /// <summary>
        /// 접속 종료
        /// </summary>
        void Close();

        /// <summary>
        /// 연결 이벤트
        /// </summary>
        event AsyncSocketConnectEventHandler _onConnectEvent;
        /// <summary>
        /// 데이터 수신 이벤트
        /// </summary>
        event AsyncSocketReceiveEventHandler _onReceiveEvent;
        /// <summary>
        /// 데이터 송신 이벤트
        /// </summary>
        event AsyncSocketSendEventHandler _onSendEvent;
        /// <summary>
        /// 접속 종료 이벤트
        /// </summary>
        event AsyncSocketCloseEventHandler _onCloseEvent;
        /// <summary>
        /// 에러 발생 이벤트
        /// </summary>
        event AsyncSocketErrorEventHandler _onErrorEvent;

    }
}
