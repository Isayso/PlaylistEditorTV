using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PlaylistEditor
{
    internal class ClassUDP2
    {
        public class TestMulticastOption
        {

            public IPAddress mcastAddress;
            public int mcastPort;
            public Socket mcastSocket;
            public MulticastOption mcastOption;


            public void MulticastOptionProperties()
            {
                Console.WriteLine("Current multicast group is: " + mcastOption.Group);
                Console.WriteLine("Current multicast local address is: " + mcastOption.LocalAddress);
            }


            public void StartMulticast()
            {

                try
                {
                    mcastSocket = new Socket(AddressFamily.InterNetwork,
                                             SocketType.Dgram,
                                             ProtocolType.Udp);

                    Console.Write("Enter the local IP address: ");

                    IPAddress localIPAddr = IPAddress.Parse(Console.ReadLine());

                    //IPAddress localIP = IPAddress.Any;
                    EndPoint localEP = (EndPoint)new IPEndPoint(localIPAddr, mcastPort);

                    mcastSocket.Bind(localEP);


                    // Define a MulticastOption object specifying the multicast group
                    // address and the local IPAddress.
                    // The multicast group address is the same as the address used by the server.
                    mcastOption = new MulticastOption(mcastAddress, localIPAddr);

                    mcastSocket.SetSocketOption(SocketOptionLevel.IP,
                                                SocketOptionName.AddMembership,
                                                mcastOption);
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            public void ReceiveBroadcastMessages()
            {
                bool done = false;
                byte[] bytes = new Byte[100];
                IPEndPoint groupEP = new IPEndPoint(mcastAddress, mcastPort);
                EndPoint remoteEP = (EndPoint)new IPEndPoint(IPAddress.Any, 0);


                try
                {
                    while (!done)
                    {
                        Console.WriteLine("Waiting for multicast packets.......");
                        Console.WriteLine("Enter ^C to terminate.");

                        mcastSocket.ReceiveFrom(bytes, ref remoteEP);

                        Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                          groupEP.ToString(),
                          Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                    }

                    mcastSocket.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

        }

    }
}

