using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using SkydevCSTool.Models;

namespace SkydevCSTool.Class
{
   public class Resync
    {
        private static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source= cscb.db; Version = 3; New = True; Compress = True;");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }
        public static void SavetoDB(string JsonData, string action)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO tblAgentErrlogs (Data,Action,Created_at) VALUES ('" + string.Concat(JsonData, "','", action,"','", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "')";
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void RetryActions()
        {

            using (IDbConnection db = CreateConnection())
            {
                var errlogs = db.Query<ErrorLogs>("Select * From tblAgentErrlogs where Sent = 0");
                
                if(errlogs.Count() > 0)
                {
                    foreach(var item in errlogs)
                    {

                    }
                }
            }
            //do 
            //var logs = Errorlogs.where(a => a.Created_at >= Datetime.Now.AddMinutes(-5) && a.Sent == 0)
            //If(logs.count() > 0)
            //{
            //foreach a in logs
            //{ 
            //  Httpclient PostAsync
            //  if success
            //      a.Sent = 1;
            //      a.Save();
            //}


            //}

            //while (true)
        }
    }
}
