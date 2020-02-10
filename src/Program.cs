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
            Func<String, String> createError = (String text) => "{\"error\":\"" + text + "\"}";
            Func<String, String> createResult = (String text) => "{\"result\":" + text + "}";
            ctx.Response.AddHeader("Content-Type", "application/json");
            var outputString = "";

            if (!ctx.Request.QueryString.AllKeys.Contains("request"))
            {
                outputString = createError("Request must be in request variable. Example: http://IP:PORT/?request={\"method\":\"test\"}");
            }
            else
            {
                try
                {
                    var request = new RequestBody(ctx.Request.QueryString["request"]);
                    switch (request.Type)
                    {
                        default:
                            {
                                outputString = createError("Method is not implemented");
                                break;
                            }
                        case RequestBody.RequestType.Test:
                            {
                                outputString = createResult(Handlers.HandleTest());
                                break;
                            }
                        case RequestBody.RequestType.Select:
                            {
                                if (request.Params.TryGetProperty("table_name", out var el_table_name))
                                    if (!Handlers.TableNames.Contains(el_table_name.GetString()))
                                        outputString = createError(el_table_name.GetString() + " is not exists");
                                    else
                                        outputString = createResult(Handlers.HandleSelect(request.Params));
                                else
                                    outputString = createError("Select request must contain table_name variable");

                                break;
                            }
                        case RequestBody.RequestType.Delete:
                            {
                                if (request.Params.TryGetProperty("table_name", out var el_table_name))
                                    if (!Handlers.TableNames.Contains(el_table_name.GetString()))
                                        outputString += createError(el_table_name.GetString() + " not exists\n");
                                    else if (request.Params.TryGetProperty("rows", out _))
                                        outputString = createResult(Handlers.HandleDelete(request.Params));
                                    else
                                        outputString += createError("Rows are not provided\n");
                                else
                                    outputString += createError("All elements must have table_name\n");

                                break;
                            }
                        case RequestBody.RequestType.Insert:
                            {
                                if (request.Params.TryGetProperty("table_name", out var el_table_name))
                                    if (!Handlers.TableNames.Contains(el_table_name.GetString()))
                                        outputString += createError(el_table_name.GetString() + " not exists\n");
                                    else if (request.Params.TryGetProperty("rows", out _))
                                        outputString = createResult(Handlers.HandleInsert(request.Params));
                                    else
                                        outputString += createError("Rows are not provided\n");
                                else
                                    outputString += createError("All elements must have table_name\n");
                                break;
                            }
                        case RequestBody.RequestType.Update:
                            {
                                if (request.Params.TryGetProperty("table_name", out var el_table_name))
                                    if (!Handlers.TableNames.Contains(el_table_name.GetString()))
                                        outputString += createError(el_table_name.GetString() + " not exists\n");
                                    else if (request.Params.TryGetProperty("rows", out _))
                                        outputString = createResult(Handlers.HandleUpdate(request.Params));
                                    else
                                        outputString += createError("Rows are not provided\n");
                                else
                                    outputString += createError("All elements must have table_name\n");
                                break;
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
