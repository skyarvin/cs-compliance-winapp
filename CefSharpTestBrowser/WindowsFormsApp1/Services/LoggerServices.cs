using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1.Services
{
    public class LoggerServices
    {
        public static string baseUrl = "https://cscb.skydev.solutions/api";
        public static string apiKey = "0a36fe1f051303b2029b25fd7a699cfcafb8e4619ddc10657ef8b32ba159e674";

        public static IEnumerable<Logger> Get()
        {
            var retList = new List<Logger>();
            using (var client = new HttpClient())
            {
                var uri = string.Concat(baseUrl, "/api/v1/employees");
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        var jsonString = content.ReadAsStringAsync();
                        jsonString.Wait();
                        retList = JsonConvert.DeserializeObject<List<Logger>>(jsonString.Result).ToList();
                    }
                }
            }

            return retList;
        }

        public static IEnumerable<Logger> Get(string username)
        {
            var retList = new List<Logger>();
            using (var client = new HttpClient())
            {
                var uri = string.Concat(baseUrl, "/api/v1/employees/", username);
                using (HttpResponseMessage response = client.GetAsync(uri).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        var jsonString = content.ReadAsStringAsync();
                        jsonString.Wait();
                        retList = JsonConvert.DeserializeObject<List<Logger>>(jsonString.Result).ToList();
                    }
                }
            }

            return retList;
        }

        public static Agent GetAgentId(string username)
        {
            using (var client = new HttpClient())
            {
                var uri = string.Concat(baseUrl, "/agents/", username);
                client.DefaultRequestHeaders.Add("Authorization", apiKey);
                using (HttpResponseMessage response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri)).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var jsonString = content.ReadAsStringAsync();
                            jsonString.Wait();
                            return JsonConvert.DeserializeObject<Agent>(jsonString.Result);
                        }
                    }
                }
            }

            return null;
        }


        public static bool Save(Logger log)
        {
            SaveToLogFile(JsonConvert.SerializeObject(log), (int)LogType.Action);
            using (var client = new HttpClient())
            {
                var uri = string.Concat(baseUrl, "/logs/");
                client.DefaultRequestHeaders.Add("Authorization", apiKey);
                var content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");
                var response = client.PostAsync(uri, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else {
                    SaveToLogFile(JsonConvert.SerializeObject(log), (int)LogType.Error);
                    MessageBox.Show(String.Concat("Something went wrong.", System.Environment.NewLine, "Please contact Admin."), "Error");

                }
            }

            return false;
        }

        public static void SaveToLogFile(string logText, int logtype)
        {
            string logFilePath = "";
            string path = String.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "/SkydevCsTool/logs/");
            switch(logtype)
            {
                case (int)LogType.Action:
                    logFilePath = @path + "log.txt";
                    break;
                case (int)LogType.Url_Change:
                    logFilePath = @path + "url_log.txt";
                    break;
                case (int)LogType.Error:
                    logFilePath = @path + "error_log.txt";
                    break;
            }
                
            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
             if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(DateTime.Now.ToString());
                    log.WriteLine(logText);
                    log.Write(System.Environment.NewLine);
                }
            }
        }

        public static int GetDuration(DateTime startTime)
        {
            return Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);
        }
    }
}
