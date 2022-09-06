using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Net;

namespace RubiksCube
{
    public class Echo : WebSocketBehavior
    {
        public Echo()
        {
            _cube = new Cube(3);
        }

        protected override void OnOpen()
        {
            Send("Welcome to Rubiks Cube!");
            Send(MessageHandler.GetCubeState(_cube));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data;

            Console.WriteLine(e.Data);

            Send(msg + " returned");
            MessageHandler.InterpretCommand(msg);
        }

        private Cube _cube;
    }

    public class WebSocketService
    {
        public WebSocketService()
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;
            _server = new WebSocketServer("ws://" + addr[0] + ":3000");
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