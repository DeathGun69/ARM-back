using System;
using System.Linq;

using System.Net;

using System.IO;

namespace TeacherARMBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            (string ip, string port) settings = ("*", "25555");
            try
            {
                settings = ParseArgs(args);
            }
            catch
            {
                Console.WriteLine("Cannot correctly parse arguments");
            }



            var listener = new ConnectionHandler(settings.ip, settings.port, CreateHandler());

            listener.StartHandling();
        }

        static (String ip, String port) ParseArgs(string[] args) => (args[0], args[1]);

        static Func<HttpListenerContext, String> CreateHandler() => (HttpListenerContext ctx) =>
        {
            Func<String, String> createResult = (String text) => "{\"result\":\"{\"" + text + "\"}";

            var outputString = createResult("Method is not implemented");

            if (!ctx.Request.QueryString.AllKeys.Contains("request"))
            {
                outputString = createResult("Request must be in request variable. Example: http://192.168.0.1:25555/?request={\"method\":\"text\"");
            }
            else
            {
                try
                {
                    var request = new RequestBody(ctx.Request.QueryString["request"]);                    
                    if (request.Type == RequestBody.RequestType.Test)
                    {
                        outputString = TestResponse.HandleTest();
                    }
                    else if (request.Type == RequestBody.RequestType.Select)
                    {
                        outputString = SelectResponse.HandleSelect();
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    outputString = createResult("Json error: " + ex.Message);
                }
            }
            Console.WriteLine(DateTime.Now + ":IN:" + ctx.Request.RawUrl + ":OUT:" + outputString);
            return outputString;
        };



    }
}
