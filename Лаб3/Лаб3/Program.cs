using System;
using System.IO;
using System.Net; // сетевые средства
using System.Net.NetworkInformation; // Информация о сети
using System.Net.Sockets;
using System.Text;

namespace Addreses
{
    internal class Program // Вывод адресов всех сетевых интерфейсов локального компьютера
    {
        static void Main()
        {
            int Ask()
            {
                Console.Write("> ");
                try
                { int choice = Convert.ToInt32(Console.ReadLine()); return choice; }
                catch (Exception ex)
                {
                    Console.WriteLine("Неправильный ввод!");
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    return -1;
                }
            }

            void MenuPrint()
            {
                Console.Clear();
                Console.WriteLine("1. Посмотреть информацию о всех сетевых интерфейсах локального компьютера");
                Console.WriteLine("2. Посмотреть информацию о специальных адресах");
                Console.WriteLine("3. Определить IP-адрес по DNS");
                Console.WriteLine("4. Создать файл на удаленном узле");
                Console.WriteLine("0. Выход");
                InputComplete(Ask());
            }

            void InputComplete(int choice)
            {
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        DoFirst();
                        Console.ReadLine();
                        MenuPrint();
                        break;
                    case 2:
                        Console.Clear();
                        DoSecond();
                        Console.ReadLine();
                        MenuPrint();
                        break;
                    case 3:
                        Console.Clear();
                        DoThird();
                        Console.ReadLine();
                        MenuPrint();
                        break;
                    case 4:
                        //-> \\192.168.244.50\ptototip\something.txt
                        Console.Clear();
                        DoFourth();
                        Console.ReadLine();
                        MenuPrint();
                        break;
                    case 0:
                        break;
                    default:
                        MenuPrint();
                        break;
                }


                void DoFirst()
                {
                    try
                    {
                        // Получаем объект содержащий информацию о хосте и выводим имя узла
                        IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                        Console.WriteLine("Имя компьютера: {0}", computerProperties.HostName);

                        // Получаем все сетевые интерфейcы локального компьютера
                        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces(); //Массив указателей
                        if (adapters.Length < 1)
                        {
                            Console.WriteLine("Сетевые адаптеры не найдены");
                            return;
                        }
                        Console.WriteLine("Количество сетевых интерфейсов: {0}\n", adapters.Length);

                        // Цикл по интерфейсам
                        foreach (NetworkInterface adapter in adapters)
                        {
                            Console.WriteLine(String.Empty.PadLeft(70, '='));
                            Console.WriteLine("\tИмя сетевого адаптера: {0}", adapter.Name);
                            Console.WriteLine("\tТип сетевого интерфейса: {0}", adapter.NetworkInterfaceType);
                            Console.WriteLine("\tОписание интерфейса: {0}", adapter.Description);
                            Console.WriteLine("\tСостояние сетевого подключения: {0}", adapter.OperationalStatus);

                            // Получаем и выводим физический адрес
                            PhysicalAddress physicalAddress = adapter.GetPhysicalAddress();
                            byte[] bytes = physicalAddress.GetAddressBytes();
                            if (bytes.Length > 0)
                            {
                                Console.Write("\tФизический адрес: ");
                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    Console.Write(bytes[i].ToString("X2"));
                                    if (i != bytes.Length - 1)
                                        Console.Write('-');
                                }
                                Console.WriteLine();
                                Console.WriteLine("\tРазмер физического адреса: {0} байт", bytes.Length);
                                Console.WriteLine();
                            }

                            // Получение и вывод IP-адресов
                            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                            // Получаем unicast-адреса, назначеннsые текущему интерфейсу
                            UnicastIPAddressInformationCollection unicastCollection = adapterProperties.UnicastAddresses;


                            if (unicastCollection.Count > 0)
                            {
                                foreach (UnicastIPAddressInformation unicastAddr in unicastCollection)
                                {
                                    // Проверка на loopback
                                    if (adapter.NetworkInterfaceType.ToString() == "Loopback")
                                    {
                                        if (unicastAddr.Address.AddressFamily == AddressFamily.InterNetwork)
                                            Console.Write("\tIPv4");
                                        if (unicastAddr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                            Console.Write("\tIPv6");
                                        Console.WriteLine(" обратной связи: {0}", unicastAddr.Address.ToString());
                                        continue;
                                    }

                                    // Выводим IPv4
                                    if (unicastAddr.Address.AddressFamily == AddressFamily.InterNetwork)
                                    {
                                        Console.WriteLine("\tIPv4 адрес: {0}", unicastAddr.Address.ToString());
                                        Console.WriteLine("\tIPv4 маска: {0}", unicastAddr.IPv4Mask.ToString());
                                        Console.WriteLine("\tIPv4 размер адреса: 32 бита");
                                        Console.WriteLine("\tIPv4 размер сетевого префикса: {0}", unicastAddr.PrefixLength);
                                        Console.WriteLine();
                                    }

                                    // Выводим IPv6
                                    if (unicastAddr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                    {
                                        if (unicastAddr.Address.IsIPv6LinkLocal)
                                        {
                                            Console.WriteLine("\tIPv6 канальный адрес: {0}", unicastAddr.Address.ToString());
                                        }
                                        else
                                        {
                                            Console.WriteLine("\tIPv6 адрес: {0}", unicastAddr.Address.ToString());
                                        }
                                        Console.WriteLine("\tIPv6 размер адреса: 128 бит");
                                        Console.WriteLine("\tIPv6 размер сетевого префикса: {0}", unicastAddr.PrefixLength);
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка: " + ex.ToString());
                    }
                }

                void DoSecond()
                {
                    Console.WriteLine("Адрес IPv4 петли обратной связи: " + IPAddress.Loopback);
                    Console.WriteLine("Адрес IPv6 петли обратной связи: " + IPAddress.IPv6Loopback);
                    Console.WriteLine("Широковещательный IP-адрес: " + IPAddress.Broadcast);
                    Console.WriteLine("IPv4 Адрес, обозначающий все сетевые интерфейсы данного узла: " + IPAddress.Any);
                    Console.WriteLine("IPv6 Адрес, обозначающий все сетевые интерфейсы данного узла: " + IPAddress.IPv6Any);
                }

                void DoThird()
                {
                    Console.WriteLine("Введите доменное имя или адрес: ");
                    Console.Write("> ");
                    var input = Console.ReadLine();
                    input = "https://" + input;
                    try
                    {
                        var inputdns = new Uri(input);
                        Console.Write("Доменное имя: ");
                        Console.WriteLine(inputdns.Host);
                        IPHostEntry ip = Dns.GetHostEntry(inputdns.Host);
                        Console.WriteLine("IP-адреса:");
                        foreach (var addr in ip.AddressList)
                            Console.WriteLine("\t" + addr);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Доменное имя не распознано!");
                        Console.WriteLine(ex.Message);
                    }

                }

                void DoFourth()
                {
                    Console.WriteLine("Введите UNC-имя файла, который хотите создать (\\\\Сервер\\СетевойКаталог\\Файл):");
                    Console.Write("> ");
                    var UNC = Console.ReadLine();

                    try
                    {
                        // Create the file, or overwrite if the file exists.
                        using (FileStream fs = File.Create(UNC))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes("Бородкин 10701322");
                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                        }

                        // Open the stream and read it back.
                        using (StreamReader sr = File.OpenText(UNC))
                        {
                            string s = "";
                            while ((s = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(s);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally { }

                }
            }

            MenuPrint();
        }
    }
}
/* 
 * 
 * 2 задание не полностью, доделать
 * Вывести для каждого интерфейса адрес, маску, размер адреса, размер сетевого префикса
 * V
 * 
 * 3 задание простое, буквально 3 строки
 * Вывести адрес петли обратной связи (IPv4, IPv6)
 * Вывести широковещательный адрес
 * Адрес обозначающий все интерфейсы узла 
 * (Это все специальные адреса)
 * 
 * 4 задание
 * Надо чтобы пользователь мог ввести любой доменный адрес, а мы его преобразуем в IP адрес.
 * Надо использовать класс DNS
 * (Использовать Microsoft Docs)
 * 
 * 5 задание (можно пропустить, ибо нужна сеть с DNS сервером)
 * 
 * 6 задание (Выполнить используя 2-ую лабораторную (общая папка))
 * Создать на другому компьютере через общую папку файл с именем и номер группы. Делаем при помощи UNC имени
 * 
 * + Отчёт
 * В методичке написано что должен содержать
 * 1. Титульный лист
 * 2. Цель работы 
 * 3. Задания (Для каждого: Задание, Листинг, Скриншот)
 * 4. Выводы
 * 
 * Сделать до след. лабораторной работы
 */