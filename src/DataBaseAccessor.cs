using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MySql.Data;
using MySql.Data.MySqlClient;



namespace TeacherARMBackend
{
    
    //Класс который предоставляет возможность для выполнения запросов к БД. Инициализирует соединения и вообще должен решать инфрастутурные вопросы по соединению с БД
    public class DataBaseAccessor 
    {
        static string CONNECTION_STRING = "server=127.0.0.1;uid=admin;pwd=admin;database=arm_teacher";

        public DataBaseAccessor()
        {

        }

        public IEnumerable<MySqlDataReader > ExecuteWithResult(string query) {
            using (var conn = new MySqlConnection(CONNECTION_STRING)) {
                conn.Open();
                using var command = new MySqlCommand(query, conn);
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
                using var conn = new MySqlConnection(CONNECTION_STRING);
                conn.Open();
                using var command = new MySqlCommand(text, conn);
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