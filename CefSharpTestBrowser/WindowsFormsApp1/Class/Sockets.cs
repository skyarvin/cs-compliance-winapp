using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SkydevCSTool.Class
{
    public class Sockets
    {
        public static Socket ExecuteServer(string ip_address_v4)
        {
            IPAddress ipAddr = IPAddress.Parse(ip_address_v4);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using 
            // Socket Class Costructor 
            Socket listener = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this 
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create 
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return listener;
        }

        public static Socket ExecuteClient(string ip_address_v4)
        {
            Socket sender = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Establish the remote endpoint 
                // for the socket. This example 
                // uses port 11111 on the local 
                // computer. 
                IPAddress ipAddr = IPAddress.Parse(ip_address_v4);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                try
                {
                    // Connect Socket to the remote 
                    // endpoint using method Connect() 
                    sender.Connect(localEndPoint);

                    // We print EndPoint information 
                    // that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ",
                                sender.RemoteEndPoint.ToString());

                    //byte[] messageSent = Encoding.ASCII.GetBytes("RESET");
                    //int byteSent = sender.Send(messageSent);
                    //byte[] messageReceived = new byte[1024];
                    //int byteRecv = sender.Receive(messageReceived);
                    //Console.WriteLine("Message from Server -> {0}",
                    //    Encoding.ASCII.GetString(messageReceived,
                    //                                0, byteRecv));

                    // Close Socket using 
                    // the method Close() 
                    //sender.Shutdown(SocketShutdown.Both);
                    //sender.Close();
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

            return sender;
        }
    }
}
