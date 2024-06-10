using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;

namespace SocketClient
{
    class ClientIP
    {
        private const string serverIP = "127.0.0.1"; // IP-адрес сервера
        private const int serverPort = 11000; // Порт сервера

        static void Main()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                // Создание TCP-клиента
                TcpClient client = new TcpClient();
                // Установка соединения с сервером
                client.Connect(serverIP, serverPort);
                Console.WriteLine("Успешное подключение к серверу {0}:{1}", serverIP, serverPort);

                byte[] buffer = new byte[1024];
                int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Получены данные от клиента:\n{0}", dataReceived);

                // Обработка данных (в данном случае просто вывод на консоль)
                Console.WriteLine("Список IPv4-адресов и масок подсети сетевых интерфейсов удаленного узла:");
                Console.WriteLine(dataReceived);

                // Закрытие соединения
                client.Close();
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