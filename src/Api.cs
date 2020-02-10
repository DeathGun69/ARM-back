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

        //method:select 
        //params:table_name:string
        public static string HandleSelect(JsonElement param)
        {
            var tableName = param.GetProperty("table_name").GetString();
            
             JsonSerializerOptions jso = new JsonSerializerOptions();            
           jso.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
           jso.PropertyNameCaseInsensitive = false;
           jso.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
           jso.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;

            switch (tableName)
            {
                case "course": return JsonSerializer.Serialize(DataBaseAccessor.Instance.GetCourses(), jso);
                case "section": return JsonSerializer.Serialize(DataBaseAccessor.Instance.GetSections(), jso);
                case "user": return JsonSerializer.Serialize(DataBaseAccessor.Instance.GetUsers(), jso);
                case "competence": return JsonSerializer.Serialize(DataBaseAccessor.Instance.GetCompetence(), jso);
                case "theme": return JsonSerializer.Serialize(DataBaseAccessor.Instance.GetThemes(), jso);
            }

            return "{}";
        }
        //method:delete 
        //params:[table_name: string, rows: []] 
        public static string HandleDelete(JsonElement param)
        {
            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            foreach (var row in param.EnumerateArray())
            {
                switch (tableName)
                {
                    case "course": DataBaseAccessor.Instance.DeleteCourse(row.GetInt32()); ++count; break;
                    case "section": DataBaseAccessor.Instance.DeleteSection(row.GetInt32()); ++count; break;
                    case "user": DataBaseAccessor.Instance.DeleteUser(row.GetInt32()); ++count; break;
                    case "competence": DataBaseAccessor.Instance.DeleteCompetence(row.GetInt32()); ++count; break;
                    case "theme": DataBaseAccessor.Instance.DeleteTheme(row.GetInt32()); ++count; break;
                }
            }
            return count.ToString();
        }
        //method:insert
        //params:[table_name: string, rows: []] 
        public static string HandleInsert(JsonElement param)
        {
            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                switch (tableName)
                {
                    case "course": DataBaseAccessor.Instance.CreateCourse(JsonSerializer.Deserialize<Course>(row.GetRawText())); ++count; break;
                    case "section": DataBaseAccessor.Instance.CreateSection(JsonSerializer.Deserialize<Section>(row.GetRawText())); ++count; break;
                    case "user": DataBaseAccessor.Instance.CreateUser(JsonSerializer.Deserialize<User>(row.GetRawText())); ++count; break;
                    case "competence": DataBaseAccessor.Instance.CreateCompetence(JsonSerializer.Deserialize<Competence>(row.GetRawText())); ++count; break;
                    case "theme": DataBaseAccessor.Instance.CreateTheme(JsonSerializer.Deserialize<Theme>(row.GetRawText())); ++count; break;
                }
            }
            return count.ToString();
        }
        //method:update 
        //params:[table_name:string , rows: []] 
        public static string HandleUpdate(JsonElement param)
        {
            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            foreach (var row in param.EnumerateArray())
            {
                switch (tableName)
                {
                    case "course": DataBaseAccessor.Instance.UpdateCourse(JsonSerializer.Deserialize<Course>(row.GetRawText())); ++count; break;
                    case "section": DataBaseAccessor.Instance.UpdateSection(JsonSerializer.Deserialize<Section>(row.GetRawText())); ++count; break;
                    case "user": DataBaseAccessor.Instance.UpdateUser(JsonSerializer.Deserialize<User>(row.GetRawText())); ++count; break;
                    case "competence": DataBaseAccessor.Instance.UpdateCompetence(JsonSerializer.Deserialize<Competence>(row.GetRawText())); ++count; break;
                    case "theme": DataBaseAccessor.Instance.UpdateTheme(JsonSerializer.Deserialize<Theme>(row.GetRawText())); ++count; break;
                }
            }
            return count.ToString();
        }

    }


}
