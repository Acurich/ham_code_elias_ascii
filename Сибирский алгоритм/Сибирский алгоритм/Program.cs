using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Сибирский_алгоритм
{
    class Program
    {
        //функция подсчета цифр, меньших или равных n
        static int CalcNums(int n)
        {
            int answ = 0;
            for (int i = 0; i <= n; i++)
            {
                answ += i;
            }
            return answ;
        }

        //функция нахождения того самого числа
        static int FindNum(int length)
        {
            int answ = 0;
            for (int i = 1; CalcNums(i) <= length; i++)
            {
                answ = i;
            } //5 5+4+3+2+1
            return answ;
        }

        static void Main(string[] args)
        {
            //проверка на существование файла In.txt
            if (!File.Exists("in.txt"))
            {
                File.Create("in.txt");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введите данные в in.txt");
                Console.ReadLine();
                return;
            }

            //создаем экземпляры файла и считывающего устройства
            FileStream file = new FileStream("in.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);

            string str = reader.ReadToEnd(); //в переменной лежит строка из файла

            //проверяем на то, чтобы файл не был пустым
            if (str == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл in.txt пустой");
                Console.ReadLine();
                return;
            }

            // преобразуем строку в массив символов
            char[] without_space_arr = str.ToCharArray();


            bool isNotDrunk = false;
            List<string> pyramid = new List<string>(); //коллекция частей пирамиды
            while (true)

            {
                Console.WriteLine("0 - Трезвая пирамида, 1 - Испорченная пирамида");
                string recive = Console.ReadLine();
                try
                {
                    if (recive == "0")
                    {
                        string[] s = str.Split(' ');

                        foreach (string st in s)
                        {
                            pyramid.Add(st);
                        }
                        isNotDrunk = true;
                        break;
                    }
                    if (recive == "1")
                    {
                        str = ""; //обнуляем str
                                  //записываем в str строку без пробелов
                        for (int i = 0; i < without_space_arr.Length; i++)
                        {

                            if ((i + 1) % 5 == 0 && without_space_arr[i] == ' ')
                            {
                                continue;
                            }
                            str += without_space_arr[i];
                        }


                        int key_num = FindNum(str.Length); //то самое ключевое 

                        int odds = str.Length - CalcNums(key_num); //кол-во лишних цифр

                        if (key_num % 2 != 0) //добавляем лишнее наверх
                        {
                            int stop_index = 0;//переменная для запоминания позиции
                                               //заполняем пирамиду
                            for (int i = key_num, count = odds; i > 0; i--, count--) //i - сколько символов брать
                            {
                                int tmp_i = i; //временная i

                                if (count > 0)
                                {
                                    tmp_i++;
                                }

                                int index = 0; //временная переменная для хранения позиции
                                string tmp = ""; //временная строка для занесения в коллекцию

                                for (int j = 0; j < tmp_i; j++)
                                {
                                    tmp += str[stop_index + j];
                                    index = stop_index + j;
                                }
                                pyramid.Add(tmp);
                                stop_index = index + 1;

                            }
                            break;
                        }
                        //добавляем лишнее вниз
                        else
                        {
                            int position = str.Length - 1;
                            if (odds != 0)
                            {
                                pyramid.Add(str.Substring(str.Length - 1, 1));
                                position--;

                            }
                            for (int i = 1, count = odds; i <= key_num; i++, count--)
                            {
                                int tmp_i = i;
                                if (count > 1) tmp_i++;
                                string tmp = "";
                                for (int j = position; j > position - tmp_i; j--)
                                {
                                    tmp += str[j];

                                }
                                char[] ch = tmp.ToCharArray();
                                Array.Reverse(ch);
                                tmp = new string(ch);
                                pyramid.Add(tmp);

                                position -= tmp_i;
                            }
                            pyramid.Reverse();
                            break;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }





            //перезаписываем файл
            File.Delete("out.txt");

            //создаем файл для вывода и записывающее устройство к нему
            FileStream out_file = new FileStream("out.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(out_file);

            int pyramidCharCount = str.Length;
            int tmp_jj = 0, tmp_ii = 0;
            List<int> ivec = new List<int>();
            List<int> jvec = new List<int>();
            int n = pyramid[0].Length;
            for (int centeri = 1, centerj = 0; centeri <= n + 2 && centerj <= n + 2; centeri += 2, centerj += 2)  // n - длина самой большой строки
            {
                while (tmp_ii <= centeri)
                {
                    jvec.Add(tmp_ii);
                    tmp_ii++;
                }
                tmp_ii--;
                while (tmp_ii > 0)
                {
                    tmp_ii--;
                    jvec.Add(tmp_ii);

                }
                while (tmp_jj <= centerj)
                {
                    ivec.Add(tmp_jj);
                    tmp_jj++;
                }
                tmp_jj--;
                while (tmp_jj > 0)
                {
                    tmp_jj--;
                    ivec.Add(tmp_jj);
                }
            }

            for (int i = ivec.Count - pyramidCharCount; i > 0; i--)
            {
                ivec.RemoveAt(ivec.Count - 1);
            }
            for (int i = jvec.Count - pyramidCharCount; i > 0; i--)
            {
                jvec.RemoveAt(jvec.Count - 1);
            }

            for (int i = 0; i < ivec.Count; i++)
            {
                writer.Write(pyramid[ivec[i]][jvec[i]]);
            }


            file.Close();
            writer.Close();

        }
    }
}

