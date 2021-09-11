using System.Collections.Generic;
using System.Net.Sockets;
using ServerLib.Event;

namespace ServerLib.Interface
{
    /// <summary>
    /// 비동기 소켓 서버
    /// </summary>
    public interface IAsyncSocketServer
    {
        /// <summary>
        /// 서버 소켓
        /// </summary>
        Socket _serverSocket { get; }
        /// <summary>
        /// 서버가 바인딩 되는 포트
        /// </summary>
        int _bindingPort { get; }
        /// <summary>
        /// 접속한 클라이언트의 리스트
        /// </summary>
        List<IAsyncSocketClient> _clientList { get; }

        /// <summary>
        /// 지정한 아이디의 클라이언트 반환
        /// </summary>
        /// <param name="clientID">클라이언트 아이디</param>
        /// <returns>클라이언트 객체</returns>
        IAsyncSocketClient GetClientByID(string clientID);
        void SetClientIDandNameByIP(string ip, string id, string name);
        /// <summary>
        /// 서버 바인딩
        /// </summary>
        void Listen();
        /// <summary>
        /// 접속된 클라이언트 연결 및 서버 연결 해제
        /// </summary>
        void Close();
        /// <summary>
        /// 데이터를 모든 클라이언트에게 전송(브로드캐스팅)
        /// </summary>
        /// <param name="sendByte">전송 데이터</param>
        void SendToAllClient(byte[] sendByte);
        /// <summary>
        /// 데이터를 일치하는 ID의 클라이언트에게 전송
        /// </summary>
        /// <param name="sendByte">전송 데이터</param>
        /// <param name="clientID">클라이언트 아이디</param>
        void SendToSpecificClientByID(byte[] sendByte, string clientID);
        /// <summary>
        /// 데이터를 일치하는 IP의 클라이언트에게 전송
        /// </summary>
        /// <param name="sendByte">전송 데이터</param>
        /// <param name="clientID">클라이언트 아이디</param>
        void SendToSpecificClientByIP(byte[] sendByte, string clientIP);
        /// <summary>
        /// 데이터를 자신을 제외한 클라이언트들에게 전송
        /// </summary>
        /// <param name="sendByte">전송 데이터</param>
        /// <param name="myClient">자신의 클라이언트 아이디</param>
        void SendToOtherClient(byte[] sendByte, IAsyncSocketClient myClient);

        /// <summary>
        /// 클라이언트 서버 접속 이벤트
        /// </summary>
        event AsyncSocketAcceptEventHandler _onAcceptEvent;
        /// <summary>
        /// 서버 동작 오류 이벤트
        /// </summary>
        event AsyncSocketErrorEventHandler _onErrorEvent;
        /// <summary>
        /// 서버 연결 끊김 이벤트
        /// </summary>
        event AsyncSocketCloseEventHandler _onCloseEvent;
        /// <summary>
        /// 클라이언트 데이터 수신 이벤트
        /// </summary>
        event AsyncSocketReceiveEventHandler _onClientReceiveEvent;
        /// <summary>
        /// 클라이언트 데이터 송신 이벤트
        /// </summary>
        event AsyncSocketSendEventHandler _onClientSendEvent;
        /// <summary>
        /// 클라이언트 접속 종료 이벤트
        /// </summary>
        event AsyncSocketCloseEventHandler _onClientCloseEvent;
        /// <summary>
        /// 클라이언트 에러 발생 이벤트
        /// </summary>
        event AsyncSocketErrorEventHandler _onClientErrorEvent;
    }
}
