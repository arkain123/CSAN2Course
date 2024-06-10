using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class ClientTCP
    {
        private const int PORT = 11000;
        private const int SIZE = 512;

        static void Main()
        {
            try
            {
                Console.WriteLine("Введите имя удалённого узла");
                string hostName = Console.ReadLine();

                IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName, System.Net.Sockets.AddressFamily.InterNetwork);

                if (ipAddresses.Length == 0)
                {
                    Console.WriteLine("Не удаётся распознать имя удалённого узла.");
                    return;
                }
                Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Клиент запущен");
                Console.WriteLine("Соединяемcя с удаленной точкой {0}", ipAddresses[0]);
                s1.Connect(ipAddresses[0], PORT);
                string textClient = "Hello Server " + DateTime.Now + " <The End>";
                byte[] byteSend = Encoding.ASCII.GetBytes(textClient);
                // отправляем массив байтов через сокет
                s1.Send(byteSend);

                byte[] byteRec = new byte[SIZE];    // буфер для сообщений от сервера
                int len = s1.Receive(byteRec);      // получаем от сервера массив байтов
                string textServer = null;
                textServer = Encoding.ASCII.GetString(byteRec, 0, len);
                Console.WriteLine("От сервера полученно: {0}", textServer);
                Console.WriteLine("Локальная конечная точка: {0}", s1.LocalEndPoint);
                s1.Shutdown(SocketShutdown.Both);
                s1.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
