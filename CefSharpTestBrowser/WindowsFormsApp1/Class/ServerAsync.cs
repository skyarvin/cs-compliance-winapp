using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace SkydevCSTool.Class
{
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 51200;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousSocketListener
    {
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener()
        {
        }

        public static void StartListening(string ip_address_v4)
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPAddress ipAddress = IPAddress.Parse(ip_address_v4);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11111);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            Globals.Connections.Add(handler);
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (!string.IsNullOrEmpty(content))
                {
                    var data = JsonConvert.DeserializeObject<PairCommand>(content);
                    switch(data.Action)
                    {
                        case "CONNECT":
                            DialogResult dialogResult = MessageBox.Show("Allow incoming connection?", "Confirm", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                Send(handler, new PairCommand { Action = "APPROVE" });
                            }
                            break;
                        case "REQUEST_CACHE":
                            string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.ComplianceAgent.profile, "\\Cookies");
                            string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                            System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                            Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                            string file = Convert.ToBase64String(sbytes);
                            Send(handler, new PairCommand { Action = "SENDFILE", Message = file, Profile = Globals.ComplianceAgent.profile });
                            break;
                        case "RECEIVE_CACHE":
                            Byte[] rbytes = Convert.FromBase64String(data.Message);
                            string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile);
                            if (!Directory.Exists(temporary_cookies_directory))
                            {
                                Directory.CreateDirectory(temporary_cookies_directory);
                            }
                            string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile, "\\Cookies");
                            File.WriteAllBytes(path, rbytes);
                            Globals.Profiles.Add(data.Profile);
                            break;
                        case "REFRESH":
                            SendToAll("REFRESH");
                            break;
                    }

                    // Not all data received. Get more.  
                    StateObject newState = new StateObject();
                    newState.workSocket = handler;
                    handler.BeginReceive(newState.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), newState);
                }
            }
        }

        public static void SendToAll(string action)
        {
            foreach (var connection in Globals.Connections)
            {
                AsynchronousSocketListener.Send(connection, new PairCommand { Action = action, Profile = Globals.Profile });
            }
        }

        public static void Send(Socket handler, PairCommand data)
        {
            data.Timestamp = Globals.unixTimestamp;
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
