using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;

namespace SkydevCSTool.Class
{
    public class Client
    {
        public Socket Connection { get; set; }
        public bool IsConnected { get; set; }
        public string Message { get; set; }

        public Client(string server_ip)
        {
            this.Connect(server_ip);
        }

        public void Connect(string ip)
        {
            this.Connection = Sockets.ExecuteClient(ip);
            try
            {
                this.Send(new PairCommand { Action = "CONNECT", Message = Globals.Profile});
                PairCommand response = this.Receive();
                if (response.Action == "APPROVE")
                {
                    this.IsConnected = true;
                    this.Message = "Connected!";
                }
                else
                {
                    
                    this.IsConnected = false;
                    this.Message = "Your request to pair has been denied!";
                }

            }
            catch (SocketException ex)
            {
                this.IsConnected = false;
                this.Message = "Pairing Connection has been disconnected";
            }
        }

        public void Send(PairCommand command)
        {
            byte[] bytes_message = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(command));
            this.Connection.Send(bytes_message);
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

        public void SendCache()
        {

            string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.ComplianceAgent.profile, "\\Cookies");
            string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
            System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
            Byte[] bytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
            string file = Convert.ToBase64String(bytes);
            this.Send(new PairCommand { Action = "RECEIVE_CACHE", Message = file, Profile=Globals.ComplianceAgent.profile });

        }


    }
}
