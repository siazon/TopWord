using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TopWord.Dapper
{
    public class SQLiteUtil
    {
        private static object obj = new object();
        private static SQLiteUtil _ins;

        public static SQLiteUtil Ins
        {
            get
            {
                lock (obj)
                {
                    if (_ins == null)
                    {
                        _ins = new SQLiteUtil();
                    }
                }
                return _ins;
            }
        }
        public SQLiteUtil()
        {
        }
        public SQLiteConnection SQLiteConnection()
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = Environment.CurrentDirectory + @"\Card.db3";
            SQLiteConnection con = new SQLiteConnection(sb.ToString());
            con.Open();
            return con;
        }
        public List<T> Query<T>(string sql)
        {
            using (IDbConnection connection = SQLiteConnection())
            {
                return connection.Query<T>(sql).ToList();
               // return connection.Execute("", new { });
            }
        }
        public int Insert<T>(string sql,T obj)
        {
            using (IDbConnection connection = SQLiteConnection())
            {
                 return connection.Execute(sql, obj);
            }
        }
        public int Update<T>(string sql, T obj)
        {
            using (IDbConnection connection = SQLiteConnection())
            {
                return connection.Execute(sql, obj);
            }
        }
    }
}
