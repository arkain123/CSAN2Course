using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class ServerTCP
    {
        private const string defaultGateway = "0.0.0.0";
        private const int PORT = 11000;
        private const int length = 10;
        private const int SIZE = 512;

        static void Main()
        {
            try
            {
                Console.WriteLine("Сервер запущен...\n");
                // Parse("0.0.0.0") - преобразуем строку IP-адреса в экземпляр класса IPAddress
                // создаём сетевую конечную точку в виде IP-адреса и номера порта
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(defaultGateway), PORT);
                Socket s1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s1.Bind(ipEndPoint); // связывам объект Socket с локальной конечной точкой
                s1.Listen(length);  // устанавливаем объект Socket в состояние прослушивания,
                                    // задаем количество клиентов, ожидающих в очереди
                Console.WriteLine("Слушающий сокет:\n"
                    + "\tДескриптор сокета: {0};\n"
                    + "\tIP-адрес: {1};\n"
                    + "\tПорт: {2}; \n",
                    s1.Handle, ipEndPoint.Address, ipEndPoint.Port);

                while (true)    // ждём подключений клиентов в бесконечном цикле
                {
                    Console.WriteLine("\nСервер ждёт подключения клиентов:");
                    // программа приостанавливается, ожидая входящее соединение
                    Socket s2 = s1.Accept();    // извлекает из очереди ожидающих запросов 1-ый
                                                // запрос на соединение и создает для его обработки новый сокет
                    Console.WriteLine("Получен запрос от клиента на установление соединения:\n"
                        + "\tДескриптор: {0};\n"
                        + "\tIP-адрес клиентского сокета: {1};\n"
                        + "\tПорт: {2}\n",
                        s2.Handle, ((IPEndPoint)s2.RemoteEndPoint).Address,
                        ((IPEndPoint)s2.RemoteEndPoint).Port);

                    string dataRec = null;

                    while (true)    // общение с подключившимся клиентов
                    {
                        byte[] byteRec = new byte[SIZE];    // буфер для сообщений от клиента
                        // получаем данные из связанного объекта
                        int lenBytesReceived = s2.Receive(byteRec);
                        // декодируем все байты из указанного массива байтов в строку
                        dataRec += Encoding.ASCII.GetString(byteRec, 0, lenBytesReceived);

                        if (dataRec.IndexOf("<The End>") > (-1))    // ищем подстроку
                        {
                            break;
                        }
                    }

                    Console.WriteLine("Получено сообщение от клиента: {0}", dataRec);
                    string dataSend = "Hello, Client! " + DateTime.Now;
                    // декодируем все символы в последовательность байтов
                    byte[] byteSend = Encoding.ASCII.GetBytes(dataSend);
                    int lenBytesSend = s2.Send(byteSend);   // передаем данные клиенту
                    Console.WriteLine("Успешно передано клиенту {0} байт", lenBytesSend);

                    s2.Shutdown(SocketShutdown.Both);       // блокируем передачу и получение данных
                    s2.Close(); // закрываем подключение и освобождаем все связанные ресурсы
                    Console.WriteLine("Сервер завершил соединение с клиентом");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
