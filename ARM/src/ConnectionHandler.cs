using System;
using System.Net;   

using System.IO;


namespace TeacherARMBackend {
    class ConnectionHandler {
        protected HttpListener _listener {get;} = new HttpListener();

        protected Func<HttpListenerContext, String> _handler {get; }

        public ConnectionHandler(string ip, string port , Func<HttpListenerContext, String> handler) {
            string host = $"http://{ip}:{port}/";
            _listener.Prefixes.Add(host);            

            _handler = handler;
        }

        public void StartHandling() {
            _listener.Start();    
            while (true) {
                var ctx = _listener.GetContext();
                var output = _handler(ctx);                
                ctx.Response.OutputStream.Write(System.Text.Encoding.UTF8.GetBytes(output));                
                ctx.Response.Close();
            }
        }
    }
}