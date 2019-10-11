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

        public string remoteip { get; set; }
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
            try
            {
                // Signal the main thread to continue.  
                allDone.Set();

                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = handler;
                state.remoteip = handler.RemoteEndPoint.ToString();
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Globals.frmMain.ServerHandleSocketError("CON00");
            }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                String content = String.Empty;

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
                    if (!string.IsNullOrEmpty(content) && content.IndexOf("|") > -1)
                    {
                        var comms = content.Split('|');
                        state.sb.Clear();
                        foreach (var comm in comms)
                        {
                            try
                            {
                                var data = JsonConvert.DeserializeObject<PairCommand>(comm);
                                switch (data.Action)
                                {
                                    case "CONNECT":
                                        DialogResult dialogResult = MessageBox.Show(string.Concat("Allow incoming connection from ", data.Message, "?"), "Confirm", MessageBoxButtons.YesNo);
                                        if (dialogResult == DialogResult.Yes)
                                        {
                                            Send(handler, new PairCommand { Action = "APPROVE" });
                                        }
                                        else
                                        {
                                            Send(handler, new PairCommand { Action = "DENY" });
                                        }
                                        break;
                                    case "REQUEST_CACHE":
                                        string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile, "\\Cookies");
                                        string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                                        System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                                        Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                                        string file = Convert.ToBase64String(sbytes);
                                        Send(handler, new PairCommand { Action = "SAVE_SERVER_CACHE", Message = file, Profile = Globals.Profile });
                                        break;
                                    case "SAVE_CLIENT_CACHE":
                                        Byte[] rbytes = Convert.FromBase64String(data.Message);
                                        string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile);
                                        if (!Directory.Exists(temporary_cookies_directory))
                                        {
                                            Directory.CreateDirectory(temporary_cookies_directory);
                                        }
                                        string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile, "\\Cookies");
                                        File.WriteAllBytes(path, rbytes);
                                        Globals.Profiles.Add(new Profile { Name = data.Profile, RemoteAddress = handler.RemoteEndPoint.ToString() });
                                        break;
                                    case "BEGIN_SEND":
                                        Globals.Connections.Add(handler);
                                        Globals.frmMain.SetBtnConnectText("DISCONNECT");
                                        Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count);
                                        SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count });
                                        break;
                                    case "REQUEST_TIME":
                                        var duration_threshold = DurationThreshold();
                                        AsynchronousSocketListener.SendToAll(new PairCommand { Action = "UPDATE_TIME",Message = duration_threshold.ToString() });
                                        Globals.frmMain.max_room_duration = duration_threshold;
                                        break;
                                    case "REFRESH":
                                        Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
                                        SendToAll(new PairCommand { Action = "REFRESH" });
                                        break;
                                    case "GOTO":
                                        if (data.Message != Globals.CurrentUrl)
                                            Globals.chromeBrowser.Load(data.Message);
                                        SendToAll(new PairCommand { Action = "GOTO", Message = data.Message }, handler);
                                        break;
                                    case "CLEAR":
                                        if (!Globals.ApprovedAgents.Contains(data.Profile)) {
                                            Globals.ApprovedAgents.Add(data.Profile);
                                            Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count);
                                            SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count });
                                        }
                                        break;
                                }
                            }
                            catch
                            {
                                state.sb.Append(comm);
                            }
                        }
                    }

                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                try
                {
                    //TODO PING CLIENT
                    string RemoteEndPoint = handler.RemoteEndPoint.ToString();
                    Globals.Connections = Globals.Connections.Where(m => m.RemoteEndPoint.ToString() != RemoteEndPoint).ToList();
                    Profile deleteProfile = Globals.Profiles.Where(m => m.RemoteAddress == RemoteEndPoint).FirstOrDefault();
                    Globals.Profiles.Remove(deleteProfile);
                    Globals.frmMain.ServerHandleSocketError("CON03", deleteProfile.Name);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Dispose();
                    handler.Close();
                }
                catch { }
            }
        }

        public static int DurationThreshold()
        {
            if (Globals.Profiles.Count > 2)
                return 15;
            if (Globals.Profiles.Count > 1)
                return 24;
            return 48;
        }
        public static void SendToAll(PairCommand data, Socket exclude_handler=null)
        {
            foreach (var connection in Globals.Connections)
            {

                if (exclude_handler != null && connection.RemoteEndPoint.ToString() == exclude_handler.RemoteEndPoint.ToString())
                {
                    continue;
                }
                AsynchronousSocketListener.Send(connection, data);
            }
        }

        public static bool HasConnections()
        {
            return Globals.Connections.Count > 0;
        }

        public static void Send(Socket handler, PairCommand data)
        {
            data.Timestamp = Globals.unixTimestamp;
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(string.Concat(JsonConvert.SerializeObject(data), "|"));

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);
        }
    }
}
