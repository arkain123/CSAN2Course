using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        Console.ReadLine();

        TcpClient client = new TcpClient("127.0.0.1", 8888);
        NetworkStream stream = client.GetStream();

        Console.Write("Enter the start port: ");
        int startPort = Int32.Parse(Console.ReadLine());
        Console.Write("Enter the end port: ");
        int endPort = Int32.Parse(Console.ReadLine());
        Console.WriteLine("Please wait...");

        string ports = $"{startPort},{endPort}";
        byte[] data = Encoding.ASCII.GetBytes(ports);
        stream.Write(data, 0, data.Length);

        byte[] bytes = new byte[256];
        int length = stream.Read(bytes, 0, bytes.Length);
        string openPorts = Encoding.ASCII.GetString(bytes, 0, length);

        Console.WriteLine($"Open ports in range {startPort}-{endPort}: {openPorts}");

        client.Close();
    }
}