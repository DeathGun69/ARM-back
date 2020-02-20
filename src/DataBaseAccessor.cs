using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Npgsql;



namespace TeacherARMBackend
{
    
    //Класс который предоставляет возможность для выполнения запросов к БД. Инициализирует соединения и вообще должен решать инфрастутурные вопросы по соединению с БД
    public class DataBaseAccessor 
    {
        static string CONNECTION_STRING = "Host=127.0.0.1;Port=5432;Username=admin;Password=admin;Database=arm_teacher";

        public DataBaseAccessor()
        {
            //TODO: Check connection to DB
        }

        public IEnumerable<NpgsqlDataReader> ExecuteWithResult(string query) {
            using (var conn = new NpgsqlConnection(CONNECTION_STRING)) {
                conn.Open();
                using var command = new NpgsqlCommand(query, conn);
                using var reader = command.ExecuteReader();
                while (reader.Read()) {
                    yield return reader;
                }
            }
        }
        public int ExecuteCommand(string text)
        {
            try
            {
                using var conn = new NpgsqlConnection(CONNECTION_STRING);
                conn.Open();
                using var command = new NpgsqlCommand(text, conn);
                var count = command.ExecuteNonQuery();
                conn.Close();
                return count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }
    }
}