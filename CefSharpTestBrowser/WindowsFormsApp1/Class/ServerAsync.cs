using CefSharp.WinForms.Internals;
using Newtonsoft.Json;
using SkydevCSTool.Properties;
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
    public class Server
    {
        public static string Type { get { return "SV"; } }
    }
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 151200;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();

        public string remoteip { get; set; }
    }

    public class ServerAsync
    {
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public ServerAsync()
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
                                        if (Globals.IsClient())
                                        {
                                            Send(handler, new PairCommand { Action = "DENY" });
                                            break;
                                        }
                                        if(string.IsNullOrEmpty(Settings.Default.preference))
                                        {
                                            Globals.frmMain.InvokeOnUiThreadIfRequired(() => Globals.ShowMessage(Globals.frmMain, "Please set your View Preference first and Try again"));
                                            Send(handler, new PairCommand { Action = "DENY" });
                                            break;
                                        }

                                        frmConfirm frmconfirm = new frmConfirm { Title = "Confirm", Message = string.Concat("Allow incoming connection from ", data.Message, "?"), Button1Text="Yes",Button2Text="No"};
                                        frmconfirm.ShowDialog();
                                        if (frmconfirm.DialogResult == DialogResult.Yes)
                                        {
                                            Send(handler, new PairCommand { Action = "APPROVE" });
                                        }
                                        else
                                        {
                                            Send(handler, new PairCommand { Action = "DENY" });
                                        }
                                        break;
                                    case "REQUEST_CACHE":
                                        string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name, "\\Cookies");
                                        string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                                        System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                                        Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                                        string file = Convert.ToBase64String(sbytes);
                                        Send(handler, new PairCommand { Action = "SAVE_SERVER_CACHE", Message = file, Profile = Globals.Profile.Name , ProfileID = Globals.Profile.AgentID });
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
                                        break;
                                    case "BEGIN_SEND":
                                        Globals.ApprovedAgents.Clear();
                                        Globals.Connections.Add(handler);
                                        Globals.Profiles.Add(new Profile { 
                                            Name = data.Profile, 
                                            RemoteAddress = handler.RemoteEndPoint.ToString(), 
                                            AgentID = data.ProfileID, 
                                            Preference = data.Preference,
                                            Type = Client.Type,
                                            IsActive = true
                                        });
                                        Globals.frmMain.SetBtnConnectText("DISCONNECT");
                                        Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
                                        Globals.max_room_duration = ServerAsync.DurationThreshold();
                                        Globals.PartnerAgents = ListOfPartners();

                                        SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count, Url = Globals.CurrentUrl });
                                        SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = Globals.max_room_duration.ToString(), RoomDuration = Globals.room_duration });
                                        SendToAll(new PairCommand { Action = "PARTNER_LIST", Message = Globals.PartnerAgents });
                                        if (!Globals.IsPreferenceSetupValid())
                                        {
                                            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                                            {
                                                if(Globals.FrmSetPreferences.Visible == false)
                                                    Globals.FrmSetPreferences.ShowDialog(Globals.frmMain);
                                            });
                                        }
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
                                        Globals.SaveToLogFile(String.Concat("Server Receive Clear:", Globals.ApprovedAgents.Count, "/", Globals.Profiles.Count, "-", Globals.CurrentUrl), (int)LogType.Activity);
                                        if (!Globals.ApprovedAgents.Contains(data.Profile)) {
                                            Globals.ApprovedAgents.Add(data.Profile);
                                            Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
                                            SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count, Url = Globals.CurrentUrl });
                                        }
                                        break;
                                    case "UNCLEAR":
                                        Globals.SaveToLogFile(String.Concat("Server Receive UnClear:", Globals.ApprovedAgents.Count, "/", Globals.Profiles.Count, "-", Globals.CurrentUrl), (int)LogType.Activity);
                                        if (Globals.ApprovedAgents.Contains(data.Profile))
                                        {
                                            Globals.ApprovedAgents.Remove(data.Profile);
                                            Globals.frmMain.DisplayRoomApprovalRate(Globals.ApprovedAgents.Count, Globals.Profiles.Count, Globals.CurrentUrl);
                                            SendToAll(new PairCommand { Action = "CLEARED_AGENTS", Message = Globals.ApprovedAgents.Count.ToString(), NumberofActiveProfiles = Globals.Profiles.Count, Url = Globals.CurrentUrl });
                                        }
                                        break;

                                    case "COMPUTE_TIME":
                                        ServerAsync.ChatlogRecomputeDurationThreshold(Int32.Parse(data.Message));
                                        ServerAsync.SendToAll(new PairCommand { Action = "UPDATE_TIME", Message = Globals.max_room_duration.ToString(), RoomDuration = Globals.room_duration });
                                        break;
                                    case "AUTO_SWITCH":
                                        SwitchToNextProfile();
                                        break;
                                    case "UPDATE_PREFERENCE":
                                        Globals.Profiles.Where(m => m.AgentID == data.ProfileID).FirstOrDefault().Preference = data.Preference;
                                        Globals.PartnerAgents = ServerAsync.ListOfPartners();
                                        ServerAsync.SendToAll(new PairCommand { Action = "PARTNER_LIST", Message = Globals.PartnerAgents });
                                        if (!Globals.IsPreferenceSetupValid())
                                        {
                                            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                                            {
                                                if (Globals.FrmSetPreferences.Visible == false)
                                                    Globals.FrmSetPreferences.ShowDialog(Globals.frmMain);

                                                Globals.FrmSetPreferences.lblMissingPref.Text = string.Concat("Missing Preference: ", Globals.MissingPreference());
                                            });
                                        }
                                        else
                                        {
                                            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                                            {
                                                if (Globals.FrmSetPreferences.Visible == true)
                                                    Globals.FrmSetPreferences.Close();
                                            });
                                        }
                                        break;
                                    case "USER_STATUS":
                                        ChangeUserActivityStatus(data.ProfileID, data.Message == "INACTIVE" ? false : true);
                                        break;
                                    case "UPDATE_START_TIME":
                                        Globals.StartTime_LastAction = DateTime.Parse(data.Message);
                                        break;
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
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
                    //TODO PING CLIENT - to detect which client has been disconnection
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

        public static string ListOfPartners()
        {
            return JsonConvert.SerializeObject(Globals.Profiles);
        }


        public static void ChatlogRecomputeDurationThreshold(int chatlog_lines_count)
        {
            const int MINIMUM_LINES_REQUIRED = 1500;
            const int MAXIMUM_DURATION = 45;
            int base_duration = DurationThreshold() + (chatlog_lines_count > MINIMUM_LINES_REQUIRED ? ( ((chatlog_lines_count - MINIMUM_LINES_REQUIRED) / 500) * 5) : 0);
            Console.WriteLine("NEW MAX ROOM DURATION" + ((base_duration > MAXIMUM_DURATION) ? MAXIMUM_DURATION : base_duration ));
            Console.WriteLine("CHAT LINES" + chatlog_lines_count);
            int new_max_duration = (base_duration > MAXIMUM_DURATION) ? MAXIMUM_DURATION : base_duration;
            Globals.max_room_duration = Globals.max_room_duration < new_max_duration ? new_max_duration : Globals.max_room_duration;
        }
        public static void SendToAll(PairCommand data, Socket exclude_handler=null)
        {
            foreach (var connection in Globals.Connections)
            {

                if (exclude_handler != null && connection.RemoteEndPoint.ToString() == exclude_handler.RemoteEndPoint.ToString())
                {
                    continue;
                }
                ServerAsync.Send(connection, data);
            }
        }

        public static bool HasConnections()
        {
            return Globals.Connections.Count > 0;
        }

        public static void Send(Socket handler, PairCommand data)
        {
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

        public static void SwitchToNextProfile()
        {
            for (var i = 0; i < Globals.Profiles.Count(); i++)
            {
                if (Globals.Profiles[i].Name == Globals.Profile.Name)
                {
                    if (i == Globals.Profiles.Count() - 1)
                        Globals.Profile = Globals.Profiles[0];
                    else
                        Globals.Profile = Globals.Profiles[i + 1];
                    break;
                }
            }

            foreach (var connection in Globals.Connections)
            {
                string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name, "\\Cookies");
                string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                string file = Convert.ToBase64String(sbytes);
                ServerAsync.Send(connection, new PairCommand { Action = "SWITCH", Profile = Globals.Profile.Name, ProfileID = Globals.Profile.AgentID, Message = file });
            }

            Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
            {
                Globals.frmMain.SwitchCache();
                Console.WriteLine("do switch");
            });
        }

        public static void ChangeUserActivityStatus(int profile_id, bool is_active)
        {
            Globals.Profiles.Where(m => m.AgentID == profile_id).FirstOrDefault().IsActive = is_active;
            if (Globals.Profiles.Where(m => m.IsActive == true).Count() == 0)
            {
                Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                {
                    Globals.frmMain.LoadOriginalProfile();
                    Globals.frmMain.ResetRoomDurationTimer();
                    var result = Globals.ShowMessageDialog(Globals.frmMain, "No group activity detected, all users has been disconnected.");

                });
                
            }
        }
    }
}
