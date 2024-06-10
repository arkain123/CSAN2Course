using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.NetworkInformation;

namespace SocketServer
{
    class ServerTCP
    {
        private const int serverPort = 11000; // Порт сервера

        static void Main()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8; // Установка кодировки UTF-8 для вывода в консоль
                Console.WriteLine("Сервер запущен... \n");

                // Создание TCP-сервера
                TcpListener listener = new TcpListener(IPAddress.Any, serverPort);
                listener.Start();
                Console.WriteLine("Сервер слушает на порту {0}", serverPort);

                while (true)
                {
                    // Принятие входящего подключения
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Принято входящее подключение от клиента {0}", ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                    NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    StringBuilder sb = new StringBuilder();
                    foreach (NetworkInterface ni in interfaces)
                    {
                        IPInterfaceProperties ipProps = ni.GetIPProperties();
                        foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                        {
                            if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                sb.AppendLine("IPv4-адрес: " + addr.Address.ToString());
                                sb.AppendLine("Маска подсети: " + addr.IPv4Mask.ToString());
                            }
                        }
                    }
                    string dataToSend = sb.ToString();
                    byte[] byteSend = Encoding.UTF8.GetBytes(dataToSend);
                    NetworkStream stream = client.GetStream();
                    stream.Write(byteSend, 0, byteSend.Length);
                    Console.WriteLine("Данные успешно отправлены на клиент");

                    // Получение данных от клиента
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Получены данные от клиента:\n{0}", dataReceived);

                    // Обработка данных (в данном случае просто вывод на консоль)
                    Console.WriteLine("Список IPv4-адресов и масок подсети сетевых интерфейсов удаленного узла:");
                    Console.WriteLine(dataReceived);

                    // Закрытие соединения с клиентом
                    stream.Close();
                }
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