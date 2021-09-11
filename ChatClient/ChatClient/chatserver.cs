using ServerLib.Impl;
using ServerLib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    class chatserver
    {
        private static IAsyncSocketClient _client;

        public static IAsyncSocketClient ChatServerConn()
        { 
            if(_client == null)
            {
                _client = new AdvancedAsyncSocketClient(Encoding.UTF8.GetBytes("\r\n"));
                _client.Connect("192.168.2.34", 5010);
            }
            return _client;
        }
    }
}
