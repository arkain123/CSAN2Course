using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class ClientUDP
    {
        private const int PORT = 11000; //Именованные константы (порт и размер буфера)
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
                Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Console.WriteLine("Клиент запущен");
                //Создаем удаленную конечную точку
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddresses[0], PORT);
                Console.WriteLine("Соединяемся с удаленной точкой {0}", ipEndPoint);
                s1.Connect(ipEndPoint);
                String textClient = "Hello Server " + DateTime.Now.ToString() + "<The End>";
                byte[] byteSend = Encoding.ASCII.GetBytes(textClient);
                // Отправляем массив байтов через сокет
                s1.SendTo(byteSend, ipEndPoint);

                byte[] byteRec = new byte[SIZE]; // буфер для сообщений от сервера
                //int len = s1.Receive(byteRec); // Получаем от сервера массив байтов
                EndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int len = s1.ReceiveFrom(byteRec, ref serverEndPoint);
                string textServer = null;
                textServer = Encoding.ASCII.GetString(byteRec, 0, len);
                Console.WriteLine("От сервера полученно {0}", textServer);
                Console.WriteLine("Локальная конечная точка {0}", s1.LocalEndPoint);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.ReadLine();
            }

        }
    }
}
