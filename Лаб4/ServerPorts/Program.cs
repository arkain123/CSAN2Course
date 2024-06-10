using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
        listener.Start();

        Console.WriteLine("Server is running...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] bytes = new byte[256];
            int length = stream.Read(bytes, 0, bytes.Length);
            string data = Encoding.ASCII.GetString(bytes, 0, length);
            int[] ports = Array.ConvertAll(data.Split(','), int.Parse);

            string openPorts = GetOpenPorts(ports[0], ports[1]);
            byte[] response = Encoding.ASCII.GetBytes(openPorts);
            stream.Write(response, 0, response.Length);

            client.Close();
        }
    }

    static string GetOpenPorts(int startPort, int endPort)
    {
        StringBuilder result = new StringBuilder();

        for (int port = startPort; port <= endPort; port++)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Trying {0}...", port);
                client.Connect("127.0.0.1", port);
                result.Append($"{port}, ");
                client.Close();
                Console.WriteLine("Success!");
            }
            catch
            {
                Console.WriteLine("Refused!");
                continue;
            }
            Console.WriteLine("Completed!");
        }

        return result.ToString();
    }
}