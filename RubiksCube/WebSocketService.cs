using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace RubiksCube
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data;

            Console.WriteLine(e.Data);

            Send(msg + " returned");
        }
    }

    public class WebSocketService
    {
        public WebSocketService()
        {
            _server = new WebSocketServer("ws://127.0.0.1:3000");
            _server.AddWebSocketService<Echo>("/echo");           
        }

        public void Start()
        {
            _server.Start();
            Console.ReadKey(true);
            _server.Stop();
        }

        private WebSocketServer _server;
    }
}