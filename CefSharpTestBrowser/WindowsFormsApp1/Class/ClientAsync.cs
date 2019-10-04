using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using WindowsFormsApp1;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace SkydevCSTool.Class
{
    
    
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
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the   
                // remote device is "host.contoso.com".  
                
                IPAddress ipAddress = IPAddress.Parse(server_ip);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // Send test data to the remote device.  
                Send(client, new PairCommand { Action = "CONNECT", Message = Globals.Profile });
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
                Console.WriteLine(e.ToString());
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
                MessageBox.Show(e.ToString());
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
                                        MessageBox.Show("Connected!");
                                        Send(client, new PairCommand { Action = "REQUEST_CACHE" });
                                        break;
                                    case "DENY":
                                        MessageBox.Show("Your request to pair has been denied!");
                                        // TODO: ?? check if we need to close the socket and Globals.Client
                                        break;
                                    case "SAVE_SERVER_CACHE":
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
                                        Send(client, new PairCommand { Action = "SAVE_CLIENT_CACHE", Message = file, Profile = Globals.ComplianceAgent.profile });
                                        // Finalize the handshake by switching to the server cache
                                        Globals.Profile = data.Profile;
                                        Globals.unixTimestamp = data.Timestamp;
                                        Globals.frmMain.SwitchCache();
                                        Send(client, new PairCommand { Action = "BEGIN_SEND" });
                                        break;
                                    // END HANDSHAKE BLOCK

                                    case "REFRESH":
                                        Globals.chromeBrowser.Load(Url.CB_COMPLIANCE_URL);
                                        Globals.unixTimestamp = data.Timestamp;
                                        break;

                                    case "SWITCH":
                                        if (data.Profile == Globals.Profile)
                                            break;
                                        Globals.Profile = data.Profile;
                                        Globals.unixTimestamp = data.Timestamp;
                                        Byte[] bytes = Convert.FromBase64String(data.Message);
                                        string _temporary_cookies_directory = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile);
                                        if (!Directory.Exists(_temporary_cookies_directory))
                                        {
                                            Directory.CreateDirectory(_temporary_cookies_directory);
                                        }
                                        string _path = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\SkydevCsTool\\cookies\\", Globals.Profile, "\\Cookies");
                                        File.WriteAllBytes(_path, bytes);
                                        Globals.frmMain.SwitchCache();
                                        Console.WriteLine("do switch");
                                        break;

                                    case "UPDATE_TIME":
                                        Globals.unixTimestamp = data.Timestamp;
                                        break;

                                    case "GOTO":
                                        if (data.Message != Globals.CurrentUrl)
                                            Globals.chromeBrowser.Load(data.Message);
                                        Globals.unixTimestamp = data.Timestamp;
                                        break;
                                }
                            }
                            catch
                            {
                                state.sb.Append(comm);
                            }
                        }

                        //receiveDone.Set();
                        // Not all data received. Get more.  

                        //Receive(client);
                        //StateObject newState = new StateObject();
                        //newState.workSocket = client;
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

       public static void Send(Socket client, PairCommand data)
        {
            data.Timestamp = Globals.unixTimestamp;
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(string.Concat(JsonConvert.SerializeObject(data), "|"));

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
               
    }
}
