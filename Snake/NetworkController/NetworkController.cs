using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
    /// <summary>
    /// This class holds all the necessary state to handle a client connection
    /// Note that all of its fields are public because we are using it like a "struct"
    /// It is a simple collection of fields
    /// </summary>
    public class SocketState
    {
        public Socket theSocket;
        public int ID;
        public Callback CallMe;
        public Callback SendCallback;
        public delegate void Callback(SocketState State);

        public int BufferSize
        {
            get;
            set;
        }

        // This is the buffer where we will receive message data from the client
        public byte[] messageBuffer = new byte[1024];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
            BufferSize = 1024;
        }
    }

    public static class Networking
    {

        public const int DEFAULT_PORT = 11000;

        /// <summary>
        /// Connects to a server at the given address and then performs the Action
        /// </summary>
        /// <param name="Action">Action to be performed hen connection occurs</param>
        /// <param name="Address">IP address of the desired server</param>
        /// <returns>The SocketState that is created by the connection</returns>
        public static SocketState ConnectToServer(SocketState.Callback Action, String Address)
        {


            System.Diagnostics.Debug.WriteLine("connecting  to " + Address);


            SocketState resultSocket;
            // Connect to a remote device.
            try
            {

                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;
                IPAddress ipAddress = IPAddress.None;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(Address);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid address: " + Address);
                        return null;
                    }
                }
                catch (Exception e1)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(Address);
                }

                // Create a TCP/IP socket.
                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                resultSocket = new SocketState(socket, -1);

                resultSocket.CallMe = Action;

                resultSocket.theSocket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, resultSocket);

            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                throw e;
            }

            return resultSocket;
        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges connect request
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            // Extract the SocketState
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                // Connection failed
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }

            // Do whatever the socket state wants us to do when we finally connect
            ss.CallMe(ss);

            // Starting the event loop and receiving data
            ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, Networking.ReceiveCallback, ss);
        }

        /// <summary>
        /// Method to be called when data has been received
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            int bytesRead = ss.theSocket.EndReceive(ar);

            // If the socket is still open
            if (bytesRead > 0)
            {
                string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                // Append the received data to the growable buffer.
                // It may be an incomplete message, so we need to start building it up piece by piece
                ss.sb.Append(theMessage);

                ss.CallMe(ss);
            }
        }

        /// <summary>
        /// Start receiving more data from the socket
        /// </summary>
        /// <param name="ss">The socketstate to receive data from</param>
        public static void GetData(SocketState ss)
        {

            // Starts the event loop again to receive data
            ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length, SocketFlags.None, Networking.ReceiveCallback, ss);
        }

        /// <summary>
        /// Send data over the given socket.
        /// </summary>
        /// <param name="ss">The socketstate that the data should be sent to</param>
        /// <param name="data">The data to be sent</param>
        public static void SendData(SocketState ss, String data)
        {
            // Convert the string data to a Byte Array
            byte[] messageBytes = Encoding.UTF8.GetBytes(data + "\n");

            // Send the data
            ss.theSocket.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, Networking.SendCallback, ss);
        }

        /// <summary>
        /// This Callback goes off once data has been sent 
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            // pull out the socket state
            SocketState ss = (SocketState)ar.AsyncState;

            // End the send
            ss.theSocket.EndSend(ar);

            // Check if sendcallback is null
            if (ss.SendCallback != null)
            {
                // Do whatever the socketstate wants to do after finishing sending
                ss.CallMe(ss);
            }
            
        }
    }

}
