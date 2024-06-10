using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class ServerUDP
    {
        private const string defaultGateway = "0.0.0.0";
        private const int PORT = 11000;
        private const int length = 10;
        private const int SIZE = 512;
        static void Main()
        {
            try
            {
                Console.WriteLine("Сервер запущен... \n");
                // Parse ("0.0.0.0") - преобразуем строку Р-адреса в экземпляр класса IPAddress 
                // создаём сетевую конечную точка в виде IP-адреса и номера порта
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(defaultGateway), PORT);
                Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s1.Bind(ipEndPoint); // связываем объект Socket с локальной конечной точкой
                //s1.Listen(length);// устанавливаем объект Socket в состояние прослушивания, //задаем количество клиентов, ожидающих в очереди

                Console.WriteLine("Слушаюший сокет: \n Дескриптор сокета {0}; \n Ip-адрес {1}:" + "\n Порт {2};\n", s1.Handle, ipEndPoint.Address, ipEndPoint.Port);
                while (true) // ждем подключений клиентов в бесконечном цикле
                {
                    Console.WriteLine("\n Сервер ждет подключения клиентов:");


                    byte[] byteRec = new byte[SIZE];
                    EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    int lenBytesReceived = s1.ReceiveFrom(byteRec, ref clientEndPoint);
                    // декодируем все байты из указанного массива байтов в строку
                    var dataRec = Encoding.ASCII.GetString(byteRec, 0, lenBytesReceived);

                    Console.WriteLine("Получено сообщение от клиента {0}: ", dataRec);
                    String dataSend = "Hello, Client! " + DateTime.Now.ToString();
                    //декодируем все символы в последовательноть байтов
                    byte[] byteSend = Encoding.ASCII.GetBytes(dataSend);
                    int lenBytesSend = s1.SendTo(byteRec, SocketFlags.None, clientEndPoint); // Передаем данные клиенту
                    Console.WriteLine(" Передано клиенту успешно {0} байт:", lenBytesSend);

                }
            }
            catch (Exception ex)
            { Console.WriteLine("Ошибка: " + ex.ToString()); }
            finally { Console.ReadLine(); }

        }
    }
}
