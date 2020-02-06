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
            Select
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
            }
            //Пока они не очень нужны
            Params = input.GetProperty("params");
        }

    }
    class TestResponse
    {
    }

    /*ITeacherDataBase dataBase = MockDataBase.DataBase;
           var cources = dataBase.GetCourses();
           var lessons = dataBase.GetLessons();
           var themes = dataBase.GetThemes();

           var response = new SelectResponse();


           //что то не так с кодировками 
           JsonSerializerOptions jso = new JsonSerializerOptions();            
           jso.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
           jso.PropertyNameCaseInsensitive = false;
           jso.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
           jso.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
           response.result.Add("cources", JsonSerializer.Serialize<IEnumerable<Course>>(cources, jso));
           response.result.Add("lessons", JsonSerializer.Serialize<IEnumerable<Lesson>>(lessons, jso));
           response.result.Add("themes", JsonSerializer.Serialize<IEnumerable<Theme>>(themes,jso));

           return JsonSerializer.Serialize<Dictionary<string, string>>(response.result, jso);*/


    public static class Handlers
    {
        
        public static string[] TableNames { get; } = { "course", "section", "user", "competence", "theme" };

        //Вся валидация должна происходить до вызова методов. В коментах написаны сигнатуры методов АПИ   
        //method:test        
        public static string HandleTest() => "{\"message\":\"This is the Test response. If you read this message it means that server is running and ready to go\"}";
        private class SelectResponse {
            
        }
        
        //method:select 
        //params:table_name:string
        public static string HandleSelect(JsonElement param)
        {
            JsonElement json = new JsonElement();
            return "nothing";
        }        
        //method:delete 
        //params:[table_name: string, rows: []] 
        public static string HandleDelete(JsonElement param)
        {
            return "0";
        }
        //method:insert
        //params:[table_name: string, rows: []] 
        public static string HandleInsert(JsonElement param) {

            return "0";
        }
        //method:update 
        //params:[table_name:string , rows: []] 
        public static string HandleUpdate(JsonElement param) {
            return "0";
        }
    }

}