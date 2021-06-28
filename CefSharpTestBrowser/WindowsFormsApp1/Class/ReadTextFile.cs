using CSTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTool.Class
{
    public class ReadTextFile
    {
        static readonly string textFile = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), string.Concat("/CsTool/logs/", DateTime.Now.ToString("MM-dd-yyyy"), "/activity_log.txt"));
        public static string Read()
        {
            string txt = ""; 
            if (File.Exists(textFile))
            {
                // Read entire text file content in one string  
                txt = File.ReadAllText(textFile);
            }

            return txt;
        }
    }
}
