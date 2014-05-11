using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;

namespace Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Ping_Pong.Ping();
        }

    }

    class Ping_Pong
    {

        static UInt16 ChecksumIMCP(byte[] data, int dataSize)
        {
            //from http://tools.ietf.org/html/rfc1071
            UInt32 total = 0;

            for (int i = 0; i < dataSize; i += 2)
            {
                UInt16 twoBytes = (UInt16)( data[i] * 256 + data[i + 1] );
                total += twoBytes;
            }

            total = (total & 0x0000FFFF) + ((total & 0xFFFF0000) / 65536);
            UInt16 checksum = (UInt16)~total;
            return checksum;
        }

        public static void Ping()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            byte[] ray = new byte[8];
            //1 byte type, 1 byte code, 2 bytes checksum, and then multiple of 4 bytes of data
            //
            ray[0] = 0x08; //type
            ray[1] = 0x00; //code
            ray[2] = 0x00; //checksum byte 1
            ray[3] = 0x00; //checksum byte 2

            ray[4] = 0x42; //byte packet 1
            ray[5] = 0x42; //byte packet 1
            ray[6] = 0x42; //byte packet 1
            ray[7] = 0x42; //byte packet 1

            UInt16 checksum = ChecksumIMCP(ray, 8);
            ray[2] = (byte)(checksum / 256);
            ray[3] = (byte)checksum;

            sock.SendTo(ray, new IPEndPoint(IPAddress.Parse("192.168.0.101"),0));

            //Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            //sock.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.221"), 0));
            //sock.IOControl(IOControlCode.ReceiveAll, new byte[] { 1, 0, 0, 0 }, new byte[] { 1, 0, 0, 0 });

            //byte[] buffer = new byte[4096];
            //EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            
            //byte[] ray = new byte[2];
            //ray[0] = 0;
            //ray[1] = 0;

            //sock.SendTo(ray, new IPEndPoint(IPAddress.Parse("192.168.0.101"),0));

            //int bytesRead = sock.ReceiveFrom(buffer, ref remoteEndPoint);
            //Console.WriteLine("got back " + bytesRead.ToString() + "  " +  buffer[0].ToString() + " " + buffer[1].ToString());

        }

        
        

    }
}
