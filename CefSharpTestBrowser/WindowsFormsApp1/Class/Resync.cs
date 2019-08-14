using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SkydevCSTool.Class
{
   public class Resync
    {
        private static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source= cscb.db; Version = 3; New = True; Compress = True; ");
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
    }
}
