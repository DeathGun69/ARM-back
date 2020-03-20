using System.IO;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Encodings;
using System.Text.Unicode;
using GemBox.Document.Tables;


using GemBox.Document;


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
            Authorize,
            Document
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
                case "doc": Type = RequestType.Document; break;
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

        public static string HandleAuth(JsonElement param)
        {
            var login = param.GetProperty("login").GetString();
            var password = param.GetProperty("password").GetString();

            var session = _sesMan.Authorize(login, password);
            if (session != null)
            {
                Console.WriteLine("Добавляю: " + session.Token);
                return "\"" + session.Token + "\"";
            }
            else throw new Exception("User not found!");
        }
        //method:select 
        //params:table_name:string
        public static string HandleSelect(JsonElement param)
        {
            var token = param.GetProperty("token").GetString();
            var session = _sesMan.CheckToken(token);
            if (session == null)
            {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();


            var helper = EntityProvider.Instance.Tables[tableName];
            var objectType = helper.GetType().BaseType.GetGenericArguments()[0];

            //Объект для сериализации объектов            
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            jso.PropertyNameCaseInsensitive = false;
            jso.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jso.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;

            if (objectType == typeof(Course) && !session.User.is_admin)
            {
                return JsonSerializer.Serialize(EntityProvider.Instance.CourseHelper.GetAllRows().Where(x => x.id_teacher == session.User.id), jso);
            }


            return JsonSerializer.Serialize(helper.GetType().GetMethod("GetAllRows").Invoke(helper, null), jso);
        }
        //method:delete 
        //params:[table_name: string, rows: [int]] 
        public static string HandleDelete(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null)
            {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            var helper = EntityProvider.Instance.Tables[tableName];
            var method = helper.GetType().GetMethod("DeleteRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[] { row.GetInt32() }))
                {
                    ++count;
                }
            }
            return count.ToString();
        }
        //method:insert
        //params:[table_name: string, rows: [Object]] 
        public static string HandleInsert(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null)
            {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;

            var helper = EntityProvider.Instance.Tables[tableName];
            var objectType = helper.GetType().BaseType.GetGenericArguments()[0];

            var method = helper.GetType().GetMethod("InsertRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[] { JsonSerializer.Deserialize(row.GetRawText(), objectType) }))
                {
                    ++count;
                }
            }
            return count.ToString();
        }
        //method:update 
        //params:[table_name:string , rows: [Object]] 
        public static string HandleUpdate(JsonElement param)
        {

            if (_sesMan.CheckToken(param.GetProperty("token").GetString()) == null)
            {
                throw new Exception("Invalid token");
            }

            var tableName = param.GetProperty("table_name").GetString();
            int count = 0;
            var helper = EntityProvider.Instance.Tables[tableName];
            var objectType = helper.GetType().BaseType.GetGenericArguments()[0];

            var method = helper.GetType().GetMethod("UpdateRow");
            foreach (var row in param.GetProperty("rows").EnumerateArray())
            {
                if (method.Invoke(helper, new object[] { JsonSerializer.Deserialize(row.GetRawText(), objectType) }))
                {
                    ++count;
                }
            }
            return count.ToString();
        }

        public static byte[] HandleDocument(JsonElement param)
        {

            var token = param.GetProperty("token").GetString();
            var session = _sesMan.CheckToken(token);
            if (session == null)
            {
                throw new Exception("Invalid token");
            }

            uint id = session.User.id;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;

            using MemoryStream stream = new MemoryStream();
            var competences = EntityProvider.Instance.CompetenceHelper.GetAllRows();

            var courses = EntityProvider.Instance.CourseHelper.GetAllRows().Where(x => x.id_teacher == id);
            var course_plan = EntityProvider.Instance.CoursePlanHelper.GetAllRows();
            var sections = EntityProvider.Instance.SectionHelper.GetAllRows();

            var themes = EntityProvider.Instance.ThemeHelper.GetAllRows();
            var themeCompetences = EntityProvider.Instance.ThemeCompetenceHelper.GetAllRows();

            var doc = new DocumentModel();


            //Создаем первую строчку - названия столбцов
            var rows = new List<TableRow>();
            rows.Add(new TableRow(doc, new List<TableCell>{
                    new TableCell(doc, new Paragraph(doc, "Курс")),
                    new TableCell(doc, new Paragraph(doc, "Компетенции курса")),
                    new TableCell(doc, new Paragraph(doc, "Раздел")),
                    new TableCell(doc, new Paragraph(doc, "Тема")),
                }
            ));
            //Для каждого курса создаем свой комплекс ячеек
            foreach (var course in courses)
            {
                //Создаем столбцы с названием курса и информации по компетенциям
                var row = new TableRow(doc);
                //Запоминаем ячейку с курсом т.к. у него будет меняться RowSpan в зависимости от ячеек правее
                var cource_cell = new TableCell(doc, new Paragraph(doc, course.name));
                row.Cells.Add(cource_cell);
                //Аналогично запоминаем компетенцию и параграф, т.к. там будет меняться еще текст.
                var competence_par = new Paragraph(doc, "-");
                var competence_cell = new TableCell(doc, competence_par);
                //Создаем набор компетенций которые принадлежат этому курсу
                //Именно набор, так темы могут обладать одинаковыми компетенциями
                HashSet<Competence> competences_set = new HashSet<Competence>();

                row.Cells.Add(competence_cell);
                //Ищем все разделы которые принадлежат к этому курсу
                var c_sections = sections.Where(x => course_plan.Where(x => x.id_course == course.id).Select(x => x.id_section).Contains(x.id));
                foreach (var section in c_sections)
                {
                    //Создаем ячейку с разделом, и запоминаем т.к. у него будет меняться RowSpan в зависимости от ячеек правее
                    var section_cell = new TableCell(doc, new Paragraph(doc, section.name));
                    row.Cells.Add(section_cell);
                    //Ищем все темы которые принадлежат к этому разделу
                    var c_themes = themes.Where(x => section.id == x.id_section);
                    foreach (var theme in c_themes)
                    {
                        //Создаем тему
                        var theme_cell = new TableCell(doc, new Paragraph(doc, theme.name + "\n" + theme.info));
                        row.Cells.Add(theme_cell);
                        rows.Add(row);
                        row = new TableRow(doc);
                        //Ищем компетенцию, которая данная тема обладает
                        try {
                        competences_set.Add(competences.First( x => x.id == themeCompetences.First(x => theme.id == x.id_theme).id_competence));
                        } catch {}


                        //RowSpan у раздела будет меняться в зависимости от количества тем которые в этом разделе
                        section_cell.RowSpan++;
                    }                    
                    if (c_themes.Any())
                        section_cell.RowSpan--;
                    else
                    {
                        //В случае если нет разделов оставляем хотя бы прочерк
                        row.Cells.Add(new TableCell(doc, new Paragraph(doc, "-")));
                        row = new TableRow(doc);
                    }
                    //RowSpan курса меняется в зависимости от RowSpan раздела
                    cource_cell.RowSpan += section_cell.RowSpan;
                }

                if (c_sections.Any())
                    cource_cell.RowSpan--;
                else
                {
                    row.Cells.Add(new TableCell(doc, new Paragraph(doc, "-")));
                    row.Cells.Add(new TableCell(doc, new Paragraph(doc, "-")));
                }
                if (row.Cells.Any())
                    rows.Add(row);
                //Меняем текст у параграфа компетенции если компетенции есть
                foreach (var comp in competences_set ) {
                    competence_par.Content.LoadText($"{comp.code}:{comp.name} ");
                }
                //RowSpan компетенции равен RowSpan курса
                competence_cell.RowSpan = cource_cell.RowSpan;
            }
            //Заголовок в начале с информации о том по какому пользователю был сделан отчет
            doc.Sections.Add(new GemBox.Document.Section(doc, new Paragraph(doc, new Run(doc, $"Курсы пользователя {session.User.name} {session.User.surname}  {session.User.patronymic}")
            {
                CharacterFormat = { Size = 18 }
            })
            {
                ParagraphFormat = {
                        Alignment = HorizontalAlignment.Center
                    }
            }
            ));
            //Добавляем таблицу в документ если у пользователя есть курсы, если нет, сохраянм в документе соответствующее сообщение
            if (rows.Count > 1)
            {
                var table = new Table(doc, rows);
                doc.Sections.Add(new GemBox.Document.Section(doc, table));
                doc.Sections.Add(new GemBox.Document.Section(doc, new Paragraph(doc, $"Количество курсов: {courses.Count()}") {
                    ParagraphFormat = { Alignment = HorizontalAlignment.Right}
                } ));
            }
            else
            {
                doc.Sections.Add(new GemBox.Document.Section(doc, new Paragraph(doc, "Данный пользователь не имеет курсов")));
            }
            //Оставляем информацию о времени создания отчета
            doc.Sections.Add(new GemBox.Document.Section(doc, new Paragraph(doc, $"Время создания отчета: {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}"){
              ParagraphFormat = {
                        Alignment = HorizontalAlignment.Right
                    }  
            }));


            doc.Save(stream, SaveOptions.DocxDefault);
            return stream.ToArray();


        }

    }


}
