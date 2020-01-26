using System;
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
            var rawString = new StreamReader(ctx.Request.InputStream).ReadToEnd();
            Console.WriteLine("INPUT: " + rawString);
            var request = new RequestBody(rawString);

            var outputString = "{\"result\":\"Method is not implemented\"}";
            if (request.Type == RequestBody.RequestType.Test) {
                outputString = TestResponse.HandleTest();
            } else if (request.Type == RequestBody.RequestType.Select) {
                outputString = SelectResponse.HandleSelect();
            }
            Console.WriteLine(outputString);
            return outputString;
        };



    }
}
