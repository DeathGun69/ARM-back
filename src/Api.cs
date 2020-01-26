using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace TeacherARMBackend
{
    public class RequestBody
    {
        public enum RequestType
        {
            Test,
            Insert,
            Delete,
            Update,
            Select
        }
        public RequestType Type { get; }

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
        public string result {get; } = "This is the Test response. If you read this message it means that server is running and ready to go";
        public static string HandleTest() => JsonSerializer.Serialize<TestResponse>(new TestResponse());
    }

    class SelectResponse {

        public Dictionary<string, string> result = new Dictionary<string, string>();

        public static string HandleSelect() {
            ITeacherDataBase dataBase = MockDataBase.DataBase;
            var cources = dataBase.GetCourses();
            var lessons = dataBase.GetLessons();
            var themes = dataBase.GetThemes();

            var response = new SelectResponse();


            //что то не так с кодировками 
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            response.result.Add("cources", JsonSerializer.Serialize<IEnumerable<Course>>(cources, jso));
            response.result.Add("lessons", JsonSerializer.Serialize<IEnumerable<Lesson>>(lessons, jso));
            response.result.Add("themes", JsonSerializer.Serialize<IEnumerable<Theme>>(themes, jso));

            return JsonSerializer.Serialize<Dictionary<string, string>>(response.result, jso);
        }
    }
}