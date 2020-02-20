using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Encodings;
using System.Text.Unicode;

using System.Text;
using System.Text.Json.Serialization;

namespace TeacherARMBackend
{
    public class RequestBody
    {
        public enum RequestType
        {
            None,
            Test,
            Insert,
            Delete,
            Update,
            Select,
            Authorize
        }
        public RequestType Type { get; } = RequestType.None;

        public JsonElement Params { get; }

        public RequestBody(String request)
        {
            var input = JsonSerializer.Deserialize<JsonElement>(request);
            var method = input.GetProperty("method").GetString();
            switch (method)
            {
                case "test": Type = RequestType.Test; break;
                case "insert": Type = RequestType.Insert; break;
                case "delete": Type = RequestType.Delete; break;
                case "update": Type = RequestType.Update; break;
                case "select": Type = RequestType.Select; break;
                case "auth": Type = RequestType.Authorize; break;
            }

            if (input.TryGetProperty("params", out var param))
            {
                Params = param;
            }
        }

    }
    public static class Handlers
    {
        private static SessionManager _sesMan = new SessionManager();

        //Вся валидация должна происходить до вызова методов. В комментах написаны сигнатуры методов АПИ   
        //method:test        
        public static string HandleTest() => "{\"message\":\"This is the Test response. If you read this message it means that server is running and ready to go\"}";
        
        public static string HandleAuth(JsonElement param) {
            var login = param.GetProperty("login").GetString();
            var password = param.GetProperty("password").GetString();

            var session = _sesMan.Authorize(login, password);
            if (session != null) 
                return "\"" + session.Token + "\"";
            else throw new Exception("User not found!");
        }
        //method:select 
        //params:table_name:string
        public static string HandleSelect(JsonElement param)
        {
            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null) {                
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();

            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            jso.PropertyNameCaseInsensitive = false;
            jso.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jso.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;

            var helper = EntityProvider.Instance.Tables[tableName];
            return JsonSerializer.Serialize(helper.GetType().GetMethod("GetAllRows").Invoke(helper, null), jso);
        }
        //method:delete 
        //params:[table_name: string, rows: [int]] 
        public static string HandleDelete(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null) {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            var helper = EntityProvider.Instance.Tables[tableName];
            var method = helper.GetType().GetMethod("DeleteRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[] {row.GetInt32()})) {
                    ++count;
                }
            }
            return count.ToString();
        }
        //method:insert
        //params:[table_name: string, rows: [Object]] 
        public static string HandleInsert(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null) {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;

            var helper = EntityProvider.Instance.Tables[tableName];
            var objectType = helper.GetType().BaseType.GetGenericArguments()[0];
            
            var method = helper.GetType().GetMethod("InsertRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[]{ JsonSerializer.Deserialize(row.GetRawText(), objectType)})) {
                    ++count;
                }
            }
            return count.ToString();
        }
        //method:update 
        //params:[table_name:string , rows: [Object]] 
        public static string HandleUpdate(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null) {
                throw new Exception("Invalid token");
            }
            
            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            var helper = EntityProvider.Instance.Tables[tableName];
            var objectType = helper.GetType().BaseType.GetGenericArguments()[0];
            
            var method = helper.GetType().GetMethod("UpdateRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[]{ JsonSerializer.Deserialize(row.GetRawText(), objectType)})) {
                    ++count;
                }
            }
            return count.ToString();
        }

    }


}
