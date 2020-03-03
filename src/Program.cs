using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

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
        //TODO: очень жирный метод вышел, надо разбить
        static Func<HttpListenerContext, String> CreateConnectionHandler() => (HttpListenerContext ctx) =>
        {
            Func<String, String> createError = (String text) => "{\"error\":\"" + text + "\"}";
            Func<String, String> createResult = (String text) => "{\"result\":" + text + "}";
            Func<JsonElement, JsonElement> checkTable = (JsonElement param) =>
            {
                if (param.TryGetProperty("table_name", out var el_table_name)) {
                    if (!EntityProvider.Instance.Tables.ContainsKey(el_table_name.GetString()))
                        throw new Exception(el_table_name.GetString() + " not exists\n"); 
                    }   
                else
                    throw new Exception("Must have table_name\n");
                return param;
            };
            Func<JsonElement, JsonElement> checkRows = (JsonElement param) =>
            {
                if (!param.TryGetProperty("rows", out _))
                    throw new Exception("Rows are not provided\n");
                return param;
            };
            Func<JsonElement, JsonElement> checkUserData = (JsonElement param) => {
                if (!param.TryGetProperty("login", out _)) {
                    throw new Exception("Excepted login field!");
                } else if (!param.TryGetProperty("password", out _)) {
                    throw new Exception("Excepted password field");
                }
                return param;
            };
            Func<JsonElement, JsonElement> checkAuth = (JsonElement param) => {
                if (!param.TryGetProperty("token", out _)) {
                    throw new Exception("Not allowed");
                }
                return param;
            };

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
                                throw new Exception("Method is not implemented");
                            }

                        case RequestBody.RequestType.Test:
                            {
                                outputString = createResult(Handlers.HandleTest());
                                break;
                            }
                        case RequestBody.RequestType.Authorize:
                            {
                                checkUserData(request.Params);
                                outputString = createResult(Handlers.HandleAuth(request.Params));
                                break;
                            }
                        case RequestBody.RequestType.Select:
                            {
                                checkAuth(checkTable(request.Params));
                                outputString = createResult(Handlers.HandleSelect(request.Params));
                                break;
                            }
                        case RequestBody.RequestType.Delete:
                            {
                                checkAuth(checkRows(checkTable(request.Params)));
                                outputString = createResult(Handlers.HandleDelete(request.Params));

                                break;
                            }
                        case RequestBody.RequestType.Insert:
                            {
                                checkAuth(checkRows(checkTable(request.Params)));
                                outputString = createResult(Handlers.HandleInsert(request.Params));
                                break;
                            }
                        case RequestBody.RequestType.Update:
                            {                                
                                checkAuth(checkRows(checkTable(request.Params)));
                                outputString = createResult(Handlers.HandleUpdate(request.Params));
                                break;
                            }
                    }

                }
                catch (Exception ex)
                {
                    outputString = createError(ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine(DateTime.Now + ":IN:" + ctx.Request.RawUrl + ":OUT:" + outputString);
            return outputString.Replace("\\u0022", "\"");
        };

    }
}
