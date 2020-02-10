using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Npgsql;



namespace TeacherARMBackend
{

    public class Competence
    {
        public int id { get; set; } =-1;
        public string name { get; set; }
        public int code { get; set; }
    }

    public class Course
    {
        public int id { get; set; } = -1;
        public string name { get; set; }
        public string univer { get; set; }
        public int hours { get; set; }
        public string[] groups { get; set; }
        public int id_teacher { get; set; }

        public int id_competence { get; set; }
    }

    public class Section
    {
        public int id { get; set; } = -1;
        public string name { get; set; }
        public int id_theme { get; set; }
        public int id_course { get; set; }
    }

    public class Theme
    {
        public int id { get; set; } = -1;
        public string name { get; set; }
        public int hours { get; set; }
        public int id_competence { get; set; }
        public string lesson_type { get; set; }
    }

    public class User
    {
        public int id { get; set; } = -1;
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
    class DataBaseAccessor
    {
        public static DataBaseAccessor Instance {get => new DataBaseAccessor(); }
        static string CONNECTION_STRING = "Host=127.0.0.1;Port=5432;Username=admin;Password=admin;Database=arm_teacher";


        private DataBaseAccessor()
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

        private int ExecuteCommand(string text) {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand(text, conn);   
            var count = command.ExecuteNonQuery();
            conn.Close();
            return count;
        }
        //TODO:
        public bool UpdateCompetence(Competence row) => 
        ExecuteCommand($"UPDATE competence SET name = '{row.name}', code = {row.code} WHERE id = {row.id}") > 0 ? true : false;

        public bool DeleteCompetence(int id) =>
        ExecuteCommand($"DELETE FROM competence WHERE id = {id}") > 0 ? true : false;

        public bool CreateCompetence(Competence row) =>
        ExecuteCommand($"INSERT INTO competence (name, code) VALUES ('{row.name}', {row.code})") > 0 ? true : false;

        public IEnumerable<Course> GetCourses()
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT id, name, univer, hours, groups, id_teacher, id_competence FROM course", conn);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                yield return new Course
                {
                    id = (int) reader[0],
                    name = (string) reader[1],
                    univer = (string) reader[2],
                    hours = (int) reader[3],
                    groups = (string[]) reader[4],
                    id_teacher = (int) reader[5],
                    id_competence = (int) reader[6]
                };
            conn.Close();
        }
        //TODO:
        public bool UpdateCourse(Course row) {            
            var groupsString = "{" + row.groups.ToList().Aggregate( (first, second) => first + "," + second) + "}"; 
            return ExecuteCommand($"UPDATE course SET name='{row.name}', univer='{row.univer}', hours={row.hours}, groups='{groupsString}', id_teacher={row.id_teacher}, id_competence={row.id_competence} WHERE id = {row.id}") > 0 ? true : false;
        }

        public bool DeleteCourse(int id) =>
        ExecuteCommand($"DELETE FROM course WHERE id = {id}") > 0 ? true : false;

        public bool CreateCourse(Course row)  {
            var groupsString = "{" + row.groups.ToList().Aggregate( (first, second) => first + "," + second) + "}"; 
            return ExecuteCommand($"INSERT INTO course (name, univer, hours, groups, id_teacher, id_competence) VALUES ('{row.name}', '{row.univer}', {row.hours}, '{groupsString}', {row.id_teacher}, {row.id_competence} )") > 0 ? true : false;
        }
        //
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
        public bool UpdateUser(User row) => 
        ExecuteCommand($"UPDATE arm_user  SET name='{row.name}', surname='{row.surname}', patronymic='{row.patronymic}', login='{row.login}', password='{row.password}' WHERE id = {row.id}") > 0 ? true : false;

        public bool DeleteUser(int id) =>
        ExecuteCommand($"DELETE FROM arm_user WHERE id = {id}") > 0 ? true : false;

        public bool CreateUser(User row) =>
        ExecuteCommand($"INSERT INTO arm_user (name, surname, patronymic, login, password) VALUES ('{row.name}','{row.surname}','{row.patronymic}', '{row.login}','{row.password}')") > 0 ? true : false;

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
        //TODO:
        public bool UpdateTheme(Theme row)  =>
        ExecuteCommand($"UPDATE theme SET name='{row.name}', hours={row.hours}, id_competence={row.id_competence}, lesson_type='{row.lesson_type}' WHERE id = {row.id}") > 0 ? true : false;

        public bool DeleteTheme(int id) =>
        ExecuteCommand($"DELETE FROM theme WHERE id = {id}") > 0 ? true : false;
        public bool CreateTheme(Theme row) =>
        ExecuteCommand($"INSERT INTO theme (name, hours, id_competence, lesson_type) VALUES ('{row.name}', {row.hours}, {row.id_competence}, '{row.lesson_type}')") > 0 ? true : false;


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
        //TODO: 
        public bool UpdateSection(Section row)  =>
        ExecuteCommand($"UPDATE section SET name='{row.name}', id_theme={row.id_theme}, id_course={row.id_course} WHERE id = {row.id}") > 0 ? true : false;

        public bool DeleteSection(int id) =>
        ExecuteCommand($"DELETE FROM section WHERE id = {id}") > 0 ? true : false;

        public bool CreateSection(Section row) =>
        ExecuteCommand($"INSERT INTO section (name, id_theme, id_course) VALUES ('{row.name}', {row.id_theme}, {row.id_course})") > 0 ? true : false;
    }
}