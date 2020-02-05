using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Npgsql;



namespace TeacherARMBackend
{

    public class Competence
    {
        public int id { get; set; }
        public string name { get; set; }
        public int code { get; set; }
    }

    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public string univer { get; set; }
        public int hours { get; set; }
        public string[] groups { get; set; }
        public int id_teacher { get; set; }

        public int id_competence { get; set; }
    }

    public class Section
    {
        public int id { get; set; }
        public string name { get; set; }
        public int id_theme { get; set; }
        public int id_course { get; set; }
    }

    public class Theme
    {
        public int id { get; set; }
        public string name { get; set; }
        public int hours { get; set; }
        public int id_competence { get; set; }
        public string lesson_type { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
    class DataBaseAccessor
    {
        static string CONNECTION_STRING = "Host=127.0.0.1;Port=5432;Username=admin;Password=admin;Database=arm_teacher";


        public DataBaseAccessor()
        {
            try
            {
                GetCompetence();
                Console.WriteLine("Successful access to database!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Database error");
                Console.WriteLine(e.Message);
            }
        }

        public IEnumerable<Competence> GetCompetence()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, code FROM competence", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new Competence
                {
                    id = (int)reader[0],
                    name = (string)reader[1],
                    code = (int)reader[2]
                };
            conn.Close();
        }

        public IEnumerable<Course> GetCourses()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, univer, hours, groups, id_teacher, id_competence FROM course", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new Course
                {
                    id = (int)reader[0],
                    name = (string)reader[1],
                    univer = (string)reader[2],
                    hours = (int)reader[3],
                    groups = (string[])reader[4],
                    id_teacher = (int)reader[5],
                    id_competence = (int)reader[6]
                };
            conn.Close();
        }

        public IEnumerable<User> GetUsers()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, surname, patronymic, login, password FROM arm_user", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new User
                {
                    id = (int)reader[0],
                    name = (string)reader[1],
                    surname = (string)reader[2],
                    patronymic = (string)reader[3],
                    login = (string)reader[4],
                    password = (string)reader[5]
                };
            conn.Close();
        }

        public IEnumerable<Theme> GetThemes()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, hours, id_competence, lesson_type FROM theme", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new Theme
                {
                    id = (int)reader[0],
                    name = (string)reader[1],
                    hours = (int)reader[2],
                    id_competence = (int)reader[3],
                    lesson_type = (string)reader[4]
                };
            conn.Close();
        }

        public IEnumerable<Section> GetSections()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, id_theme, id_course FROM section", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new Section
                {
                    id = (int)reader[0],
                    name = (string)reader[1],
                    id_theme = (int)reader[2],
                    id_course = (int)reader[3]
                };
            conn.Close();
        }


        /*public static void Main(String[] args)
        {
            var db = new DataBaseAccessor();
            foreach (var course in db.GetCourses())
                Console.WriteLine(course.name, course.univer);
            
            foreach (var course in db.GetThemes())
                Console.WriteLine(course.name, course.lesson_type);

            foreach (var course in db.GetCompetence())
                Console.WriteLine(course.name, course.code);

            foreach (var course in db.GetUsers())
                Console.WriteLine(course.name, course.login, course.password);

            foreach (var course in db.GetSections())
                Console.WriteLine(course.name, course.id_course);
        }*/
    }
}