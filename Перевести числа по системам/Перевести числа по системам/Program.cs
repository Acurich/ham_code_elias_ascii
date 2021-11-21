using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Перевести_числа_по_системам
{
    class Program
    {
        static long From2To10(string n)
        {
            double ans = 0;
            for (int i = n.Length - 1, j = 0; i >= 0; i--, j++)
            {
                int num = n[i] - '0';
                ans += Math.Pow(2, j) * num;
            }
            return (int)ans;
        }
        static long From10To2(int n)
        {

            string s = "";
            while (n > 0)
            {
                s += n % 2;
                n /= 2;
            }

            char[] reversed = s.Reverse().ToArray();
            s = new string(reversed);

            return Convert.ToInt64(s);
        }
        //перегрузка для больших чисел
        static string From10To2(string n)
        {

            string s = "";
            long nn = Convert.ToInt64(n);
            while (nn > 0)
            {
                s += nn % 2;
                nn /= 2;
            }

            char[] reversed = s.Reverse().ToArray();
            s = new string(reversed);
            if (s.Length < 8)
            {
                s = new string(Enumerable.Repeat('0', 8 - s.Length).ToArray()) + s;
                
            }

            return s;
        }
        static string From10To16(long n)
        {
            string s = "";
            while (n > 0)
            {
                long ost = n % 16;
                switch (ost) // перебираем варианты остатков
                {
                    case 10:
                        s += 'A';
                        break;
                    case 11:
                        s += 'B';
                        break;
                    case 12:
                        s += 'C';
                        break;
                    case 13:
                        s += 'D';
                        break;
                    case 14:
                        s += 'E';
                        break;
                    case 15:
                        s += 'F';
                        break;
                    default:
                        s += ost;
                        break;
                }
                n /= 16;
            }
            char[] reversed = s.Reverse().ToArray();
            s = new string(reversed);
            return s;
        }
        static double From16To10(string n)
        {
            double answ = 0;
            for (int i = n.Length - 1, j = 0; i >= 0; i--, j++)
            {
                int num = 0;
                switch (char.ToUpper(n[i]))
                {
                    case 'A':
                        num = 10;
                        break;
                    case 'B':
                        num = 11;
                        break;
                    case 'C':
                        num = 12;
                        break;
                    case 'D':
                        num = 13;
                        break;
                    case 'E':
                        num = 14;
                        break;
                    case 'F':
                        num = 15;
                        break;
                    default:
                        num = n[i] - '0';
                        break;
                }
                answ += Math.Pow(16, j) * num;
            }
            return answ;

        }
        static string From2To16(string n)
        {
            return From10To16(From2To10(n));
        }

        static void Main(string[] args)
        {

            //проверка на существование файла
            if (!File.Exists("In.txt"))
            {
                File.Create("In.txt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введите данные в In.txt");
                Console.ReadLine();
                return;
            }

            FileStream file = new FileStream("In.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(file);

            string s = reader.ReadToEnd();

            //проверка на пустоту файла
            if (s == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл In.txt пустой");
                Console.ReadLine();
                return;
            }

            string[] strings = s.Split(' ');


            File.Delete("out.txt");
            FileStream out_file = new FileStream("Out.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(out_file);
            while (true)
            {
                Console.WriteLine(" 0 - 2->10 \n 1 - 10->2 \n 2 - 10->16 \n 3 - 16->10 \n 4 - 2->16");
                string answer = Console.ReadLine();
                try
                {
                    if (answer == "0")
                    {
                        foreach (string str in strings)
                        {
                            //проверка на правильность двоичной записи всех чисел ряда
                            foreach (char ch in str)
                            {

                                if (ch != '1' && ch != '0')
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Введённые числа не соответсвуют двоичной системе!");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }


                            writer.Write(From2To10(str) + " ");
                        }
                        break;
                    }
                    if (answer == "1")
                    {
                        foreach (string str in strings)
                        {
                            //проверка на правильность 10-чной записи
                            foreach (char ch in str)
                            {
                                if (char.IsLetter(ch))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Введённые числа не соответсвуют десятичной системе!");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }


                            writer.Write(From10To2(str) + " ");

                        }
                        break;
                    }
                    if (answer == "2")
                    {
                        foreach (string str in strings)
                        {
                            //проверка на правильность 10-чной записи
                            foreach (char ch in str)
                            {
                                if (char.IsLetter(ch))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Введённые числа не соответсвуют десятичной системе!");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }

                            long n = Convert.ToInt64(str);
                            writer.Write(From10To16(n) + " ");

                        }
                        break;
                    }
                    if (answer == "3")
                    {
                        foreach (string str in strings)
                        {
                            foreach (char ch in str)
                            {
                                if (char.ToUpper(ch) > 'F')
                                {

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Введённые числа не соответсвуют 16-ричной системе!");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }
                            writer.Write(From16To10(str) + " ");
                        }
                        break;
                    }
                    if (answer == "4")
                    {
                        foreach (string str in strings)
                        {
                            //проверка на правильность двоичной записи всех чисел ряда
                            foreach (char ch in str)
                            {

                                if (ch != '1' && ch != '0')
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Введённые числа не соответсвуют двоичной системе!");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }


                            writer.Write(From2To16(str) + " ");
                        }
                        break;
                    }
                }
                catch
                {

                    continue;
                }
            }
            writer.Close();
        }
    }
}
