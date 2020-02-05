using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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



            var listener = new ConnectionHandler(settings.ip, settings.port, CreateConnectionHandler());

            listener.StartHandling();
        }

        static (String ip, String port) ParseArgs(string[] args) => (args[0], args[1]);

        static Func<HttpListenerContext, String> CreateConnectionHandler() => (HttpListenerContext ctx) =>
        {
            Func<String, String> createError = (String text) => "{\"error\":\"{\"" + text + "\"}";
            Func<String, String> createResult = (String text) => "{\"result\":\"{\"" + text + "\"}";
            ctx.Response.AddHeader("Content-Type", "application/json");
            var outputString = createError("Method is not implemented");

            if (!ctx.Request.QueryString.AllKeys.Contains("request"))
            {
                outputString = createError("Request must be in request variable. Example: http://IP:PORT/?request={\"method\":\"test\"}");
            }
            else
            {
                try
                {
                    var request = new RequestBody(ctx.Request.QueryString["request"]);                    
                    if (request.Type == RequestBody.RequestType.Test)
                    {
                        outputString = createResult(TestResponse.HandleTest());
                    }
                    else if (request.Type == RequestBody.RequestType.Select)
                    {
                        if (request.Params.TryGetProperty("table_name", out var element)) {
                            var table_name = element.GetString();
                            if (Handlers.TableNames.Contains(table_name)) {
                                outputString = createResult(Handlers.HandleSelect(table_name));
                            } else {
                                outputString = createError(table_name + " is not exists");
                            }
                        } else {
                            outputString = createError("Select request must contain table_name variable");
                        }
                    } else if (request.Type == RequestBody.RequestType.Delete) 
                    {
                        foreach (var element in request.Params.EnumerateArray()) {
                            if (request.Params.TryGetProperty("table_name", out var el_table_name)) {
                                var table_name = el_table_name.GetString();
                                if (!Handlers.TableNames.Contains(table_name)) {
                                    outputString = createError(table_name + " not exists");
                                    break;
                                }                           
                                
                                if (request.Params.TryGetProperty("rows", out var el_rows)) {    
                                    outputString = createResult( Handlers.HandleDelete(table_name, el_rows.EnumerateArray().ToList().Select(x => x.GetInt32())).ToString() );
                                } else {
                                    outputString = createError("All elements must have rows");
                                    break;
                                }
                            } else {
                                outputString = createError("All elements must have table_name");
                                break;
                            }
                        }
                    }
                }
                catch (System.Text.Json.JsonException ex)
                {
                    outputString = createError("Error: " + ex.Message);
                }
            }
            Console.WriteLine(DateTime.Now + ":IN:" + ctx.Request.RawUrl + ":OUT:" + outputString);
            return outputString.Replace("\\u0022", "\"");
        };
        
      



    }
}
