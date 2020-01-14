using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using WindowsFormsApp1;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using CefSharp.WinForms.Internals;
using SkydevCSTool.Properties;

namespace SkydevCSTool.Class
{
    public class Client
    {
        public static string Type { get { return "CL"; } }
    }
    public class AsynchronousClient
    {
        // The port number for the remote device.  
        private const int port = 11111;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        public static void StartClient(string server_ip)
        {
             
            connectDone.Reset();
            sendDone.Reset();
            receiveDone.Reset();

            IPAddress ipAddress = IPAddress.Parse(server_ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                if (!Globals.IsClient())
                {
                    return;
                }

                // Send test data to the remote device.  
                Send(client, new PairCommand { Action = "CONNECT", Message = Globals.ComplianceAgent.profile });
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();
                
                


        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                Globals.Client = client;

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Globals.frmMain.ClientHandleSocketError("CON00");
                connectDone.Set();

            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Globals.frmMain.ClientHandleSocketError("CON03");
                receiveDone.Set();
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    response = state.sb.ToString();
                    if (!string.IsNullOrEmpty(response) && response.IndexOf("|") > -1)
                    {
                        var comms = response.Split('|');
                        state.sb.Clear();
                        foreach(var comm in comms)
                        {
                            try
                            {
                                var data = JsonConvert.DeserializeObject<PairCommand>(comm);
                                switch (data.Action)
                                {
                                    // START HANDSHAKE BLOCK
                                    case "APPROVE":
                                        //MessageBox.Show("Connected!");
                                        frmConfirm frmconfirm = new frmConfirm { Title = "Connection status", Message = "Connected", Button1Text = "Ok", Button2Visible = false };
                                        frmconfirm.ShowDialog();
                                        Send(client, new PairCommand { Action = "REQUEST_CACHE" });
                                        break;
                                    case "DENY":
                                        // MessageBox.Show("Your request to pair has been denied!");
                                        frmConfirm frmDenied = new frmConfirm { Title = "Connection status", Message = "Your request to pair has been denied!", Button1Text = "Ok", Button2Visible = false };
                                        frmDenied.ShowDialog();
                                        Globals.frmMain.SetBtnConnectText("CONNECT");
                                        if(Globals.Client != null)
                                        {
                                            Globals.Client.Shutdown(SocketShutdown.Both);
                                            Globals.Client.Dispose();
                                            Globals.Client.Close();
                                            Globals.Client = null;
                                        }
                                        // TODO: ?? check if we need to close the socket and Globals.Client

                                        break;
                                    case "SAVE_SERVER_CACHE":
                                       if (data.Profile == Globals.ComplianceAgent.profile)
                                        {
                                            Send(client, new PairCommand { 
                                                Action = "BEGIN_SEND" , 
                                                Profile = Globals.ComplianceAgent.profile,
                                                ProfileID = Globals.ComplianceAgent.id, 
                                                Preference = Settings.Default.preference
                                            });
                                            Globals.frmMain.SetBtnConnectText("DISCONNECT");
                                            break;
                                        }
                                        // Receive the cache from server
                                        Byte[] rbytes = Convert.FromBase64String(data.Message);
                                        string temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile);
                                        if (!Directory.Exists(temporary_cookies_directory))
                                        {
                                            Directory.CreateDirectory(temporary_cookies_directory);
                                        }
                                        string path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", data.Profile, "\\Cookies");
                                        File.WriteAllBytes(path, rbytes);
                                        // Give local cache back to server
                                        string source_path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.ComplianceAgent.profile, "\\Cookies");
                                        string output_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool");
                                        System.IO.File.Copy(source_path, String.Concat(output_directory, "\\temp\\Cookies_me"), true);
                                        Byte[] sbytes = File.ReadAllBytes(String.Concat(output_directory, "\\temp\\Cookies_me"));
                                        string file = Convert.ToBase64String(sbytes);
                                        Send(client, new PairCommand { Action = "SAVE_CLIENT_CACHE", Message = file, Profile = Globals.ComplianceAgent.profile, ProfileID = Globals.ComplianceAgent.id });
                                        // Finalize the handshake by switching to the server cache
                                        Globals.Profile = new Profile { Name = data.Profile , AgentID = data.ProfileID,IsActive = true };

                                        Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                                        {
                                            Globals.frmMain.SwitchCache();
                                            Send(client, new PairCommand { 
                                                Action = "BEGIN_SEND", 
                                                Profile = Globals.ComplianceAgent.profile, 
                                                ProfileID = Globals.ComplianceAgent.id,
                                                Preference = Settings.Default.preference
                                            });
                                            Globals.frmMain.SetBtnConnectText("DISCONNECT");

                                          
                                        });
                                        break;
                                    // END HANDSHAKE BLOCK

                                    case "REFRESH":
                                        Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
                                        break;

                                    case "SWITCH":
                                       if (data.Profile == Globals.Profile.Name)
                                            break;
                                        Globals.Profile = new Profile { Name = data.Profile, AgentID = data.ProfileID, IsActive = true };
                                        Byte[] bytes = Convert.FromBase64String(data.Message);
                                        string _temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name);
                                        if (!Directory.Exists(_temporary_cookies_directory))
                                        {
                                            Directory.CreateDirectory(_temporary_cookies_directory);
                                        }

                                        Globals.SaveToLogFile(string.Concat("Client switch: ", Globals.Profile.Name), (int)LogType.Action);
                                        string _path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile.Name, "\\Cookies");
                                        File.WriteAllBytes(_path, bytes);
                                        Globals.frmMain.InvokeOnUiThreadIfRequired(() =>
                                        {
                                            Globals.frmMain.SwitchCache();
                                            Console.WriteLine("do switch");
                                        });
                                        break;

                                    case "UPDATE_TIME":
                                        Globals.max_room_duration = Int32.Parse(data.Message);
                                        Globals.room_duration = data.RoomDuration;
                                        break;

                                    case "GOTO":
                                        if (data.Message != Globals.CurrentUrl)
                                            Globals.chromeBrowser.Load(data.Message);
                                        break;
                                    case "CLEARED_AGENTS":
                                        Globals.frmMain.DisplayRoomApprovalRate(Int32.Parse(data.Message), data.NumberofActiveProfiles, data.Url);
                                        break;
                                    case "PARTNER_LIST":
                                        Globals.Profiles = JsonConvert.DeserializeObject<List<Profile>>(data.Message);
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

                                }
                            }
                            catch (Exception ee)
                            {
                                state.sb.Append(comm);
                            }
                        }

                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Globals.frmMain.ClientHandleSocketError("CON03");
                receiveDone.Set();
            }
        }

       public static void Send(Socket client, PairCommand data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(string.Concat(JsonConvert.SerializeObject(data), "|"));

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            sendDone.Set();
        }
               
    }
}
