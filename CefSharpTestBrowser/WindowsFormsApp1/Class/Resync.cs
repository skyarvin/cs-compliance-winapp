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
using WindowsFormsApp1.Models;
using System.Threading;
using WindowsFormsApp1;

namespace SkydevCSTool.Class
{
   public class Resync
    {
        private static SQLiteConnection CreateConnection()
        {
            if (!System.IO.File.Exists("cscb.db"))
                throw new Exception("No local DB found. Please contact admin");
            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source= cscb.db; Version = 3; Compress = True;");
            sqlite_conn.Open();
            return sqlite_conn;
        }
        public static void SavetoDB(string JsonData, string action)
        {
            using (IDbConnection db = CreateConnection())
            {
                db.Execute("INSERT INTO tblAgentErrlogs (Data,Action,Created_at) VALUES (@Data,@Action,@Created_at)",
                    new ErrorLogs
                    {
                        Data = JsonData,
                        Action = action,
                        Created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            }
        }

        public static void RetryActions()
        {
            using (IDbConnection db = CreateConnection())
            {
                var errlogs = db.Query<ErrorLogs>("Select * From tblAgentErrlogs where Created_at >= DateTime('Now', 'LocalTime', '-5 Minute')");
                if (errlogs.Count() > 0)
                {
                    foreach(var item in errlogs)
                    {
                        try
                        {
                            var flag = false;
                            if (item.Action == "Save")
                                flag = Logger.Save_Json_String(item.Data);
                            if (item.Action == "Update")
                                flag = Logger.Update_Json_String(item.Data);
                            if (flag)
                                db.Execute("Delete from tblAgentErrlogs WHERE ID = @ID", item);
                        }
                        catch { }
                    }
                }
            }
            Thread.Sleep(30000);
        }
        public static List<ErrorLogs> GetLogs() {
            using (IDbConnection db = CreateConnection()) {
                var errlogs = db.Query<ErrorLogs>("Select * from tblAgentErrlogs");
                return errlogs.ToList();
            }
        }
    }
}
