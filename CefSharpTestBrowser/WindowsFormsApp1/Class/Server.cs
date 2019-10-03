using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;

namespace SkydevCSTool.Class
{
    public class Server
    {
        public Socket Connection { get; set; }

        public List<Socket> Connections = new List<Socket>();
        public bool IsConnected { get; set; }
        public string Message { get; set; }

        public Server(string myIP)
        {
            this.Start(myIP);
        }
        public void Start(string ip)
        {
            this.Connection = Sockets.ExecuteServer(ip);
        }

        public void Accept()
        {
            this.Connection = this.Connection.Accept();
            Connections.Add(this.Connection);
        }

        public void Send(PairCommand command)
        {
            command.Timestamp = Globals.unixTimestamp;
            byte[] bytes_message = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(command));
            this.Connection.Send(bytes_message);
        }

        public void SendCache()
        {
            
            string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.ComplianceAgent.profile, "\\Cookies");
            string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
            System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
            Byte[] bytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
            string file = Convert.ToBase64String(bytes);
            this.Send(new PairCommand { Action = "SENDFILE", Message = file ,Profile = Globals.ComplianceAgent.profile });
        }

        public PairCommand Receive()
        {
            byte[] bytes = new Byte[this.Connection.ReceiveBufferSize];
            int data = this.Connection.Receive(bytes);
            var response = Encoding.ASCII.GetString(bytes, 0, data);
            return JsonConvert.DeserializeObject<PairCommand>(response);
        }

        public void Dispose()
        {
            this.IsConnected = false;
            this.Connection.Shutdown(SocketShutdown.Both);
            this.Connection.Disconnect(true);
            this.Connection.Close();
        }
    }
}
