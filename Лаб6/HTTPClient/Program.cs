using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class HttpClient
{
    static void Main()
    {
        string url = "http://example.com/index.html"; // Заданный URI-адрес
        string host = "example.com";
        string html = "index.html";
        int port = 80;
        //string url = "http://localhost:8080/index.html";
        //string host = "localhost";
        //string html = "index.html";
        //int port = 8080;

        TcpClient client = new TcpClient();
        client.Connect(host, port);

        NetworkStream stream = client.GetStream();

        string request = "GET /" + html + " HTTP/1.1\r\n" +
                         "Host: " + host + "\r\n" +
                         "User-Agent: MySimpleHttpClient\r\n" +
                         "\r\n";

        byte[] requestData = Encoding.UTF8.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);

        byte[] responseData = new byte[4096];
        int bytesRead = stream.Read(responseData, 0, responseData.Length);
        string response = Encoding.UTF8.GetString(responseData, 0, bytesRead);

        Console.WriteLine(response);
        File.WriteAllText("response.html", response);

        client.Close();
    }
}
