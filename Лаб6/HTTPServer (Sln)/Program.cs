using System;
using System.IO;
using System.Net;
using System.Text;

class HttpServer
{
    static void Main()
    {
        string rootDirectory = @"E:\BNTU\2курс\КСИС\Lab6\serverRoot";

        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");

        Console.WriteLine("Server started. Listening for connections...");

        listener.Start();

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine("Client connected. Request URL: " + request.Url);

            string filePath = rootDirectory + request.Url.LocalPath;
            if (File.Exists(filePath))
            {
                byte[] content = File.ReadAllBytes(filePath);

                response.ContentLength64 = content.Length;
                response.OutputStream.Write(content, 0, content.Length);
                response.OutputStream.Close();

                Console.WriteLine("File sent to client.");
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();

                Console.WriteLine("File not found.");
            }
        }
    }
}
