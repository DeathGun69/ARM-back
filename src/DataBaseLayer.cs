using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Npgsql;


namespace TeacherARMBackend
{    
    public class DataBaseEntity
    {
        public int id { get; set; }
    }

    //сериализатор объектов и помощник по созданию  sql запросов (чтобы не писать для каждой сущности свои запросы)    
    public abstract class DataBaseSqlHelper<T> where T : DataBaseEntity, new()
    {
        public abstract string TableName { get; }
        private DataBaseAccessor Accessor { get; }

        public DataBaseSqlHelper(DataBaseAccessor accessor)
        {
            Accessor = accessor;
        }

        private string GetValuesString(T t) => string.Join(", ", typeof(T).GetProperties().OrderBy(x => x.Name).Select(x =>
        {
            var value = x.GetValue(t);
            var type = value.GetType();
            if (type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid))
            {
                return $"\'{value.ToString()}\'";
            }
            return value.ToString();
        }));

        public IEnumerable<T> GetAllRows()
        {
            var columnsStr = string.Join(",", typeof(T).GetProperties().OrderBy(x => x.Name).Select(x => x.Name));
            var query = $"SELECT {columnsStr} FROM {TableName}";
            Console.WriteLine(query);
            var columnsWithTypes = typeof(T).GetProperties().OrderBy(x => x.Name).Select(x => (x.Name, x.GetType())).ToList();
            return Accessor.ExecuteWithResult(query).Select(reader =>
            {
                T obj = new T();
                for (int i = 0; i < columnsWithTypes.Count(); ++i)
                {
                    obj.GetType().GetProperty(columnsWithTypes[i].Name).SetValue(obj, reader[i]);
                }
                return obj;
            });
        }


        public bool InsertRow(T t)
        {
            var properties = typeof(T).GetProperties().OrderBy(x => x.Name).Where(x => x.Name != "id");

            var columns = string.Join(",", properties.Select(x => x.Name));
            var values = string.Join(",", properties.Select(x =>
            {
                var value = x.GetValue(t, null);
                var type = value.GetType();
                if (type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid))
                {
                    return $"\'{value.ToString()}\'";
                }
                return value.ToString();
            }));
            return Accessor.ExecuteCommand($"INSERT INTO {TableName} ({columns}) VALUES ({values})") > 0 ? true : false;
        }

        public bool UpdateRow(T t)
        {
            var properties = typeof(T).GetProperties().OrderBy(x => x.Name).Where(x => x.Name != "id");

            var values = string.Join(",", properties.Select(x =>
            {
                var value = x.GetValue(t, null);
                var type = value.GetType();
                if (type == typeof(string) || type == typeof(DateTime) || type == typeof(Guid))
                {
                    return $"{x.Name}=\'{value.ToString()}\'";
                }
                return $"{x.Name}={value.ToString()}";
            }));

            return Accessor.ExecuteCommand($"UPDATE {TableName} SET {values} WHERE id={t.id}") > 0 ? true : false;
        }

        public bool DeleteRow(int id) => Accessor.ExecuteCommand($"DELETE FROM {TableName} WHERE id={id}") > 0 ? true : false;
    }
}