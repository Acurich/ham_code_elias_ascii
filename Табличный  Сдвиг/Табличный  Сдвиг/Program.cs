using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Табличный__Сдвиг
{
    class Point
    {

    }
    class Program
    {

        //всевозможные варианты таблицы
        static int[,] Varioties(int length)
        {

            List<int> mults = new List<int>();
            for (int i = 2; i < length; i++)
            {
                for (int j = 2; j < length; j++)
                {
                    if (i * j == length && (Math.Abs(i - j) <= 3) && (Math.Abs(i - j) >= 0))
                    {
                        mults.Add(i);
                        mults.Add(j);
                    }
                }
            }
            int n = mults.Count;
            int[,] tmp = new int[n / 2, 2];
            for (int i = 0, j = 0; i < n; i += 2, j++)
            {
                tmp[j, 0] = mults[i];
                tmp[j, 1] = mults[i + 1];
            }
            return tmp;

        }

        static void Main(string[] args)
        {
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
            string str = reader.ReadToEnd();
            if (str == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Файл In.txt пустой");
                Console.ReadLine();
                return;
            }

            List<int> key = new List<int>();
            bool keyExist = false;

            //Проверка на наличие ключа
            while (true)
            {
                Console.WriteLine("Ведите ключ БЕЗ ПРОБЕЛОВ (если его нет, просто тыкните enter)");

                string answer = Console.ReadLine();
                //проверка на наличие пробелов
                foreach (var s in answer)
                {
                    if (s == ' ') continue;
                }
                if (answer != "")
                {
                    answer = answer.ToLower();

                    //создаем ключ
                    foreach (var ch in answer)
                    {
                        int tmp = (ch - 'а');
                        key.Add(tmp);
                    }
                    keyExist = true;
                    break;
                }
                else break;


            }

            int[] key_index = new int[key.Count];

            for (int i = 0, len = key.Count; i < len; i++)
            {

                int index = key.FindIndex(x => x == key.Min());
                key_index[index] = i;
                key[index] = (key[index] + 1) * 100;
            }

            char[,] table = { };

            int[] size = new int[2];

            bool sizeExist = true;

            File.Delete("out.txt");
            FileStream out_file = new FileStream("Out.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(out_file);
            //если есть ключ
            if (keyExist)
            {

                size[0] = key.Count; // столбцы
                size[1] = str.Length / key.Count; //строки
                char[,] tmp_table = new char[size[1], size[0]];

                char[] grupped_Str = str.ToCharArray();
                string[] s = new string[size[0]];
                str = "";

                //заполняем массив строк, которые будут записаны, как столбцы
                for (int i = 0, index = 0; i < grupped_Str.Length; i++)
                {
                    str += grupped_Str[i];
                    if ((i + 1) % size[1] == 0 && i != 0)
                    {
                        s[index] = str;
                        str = "";
                        index++;

                        continue;
                    }

                }
                int counter = 0;
                // заполняем кажый столбик строкой с номером index
                foreach (int index in key_index)
                {
                    for (int i = 0; i < size[1]; i++)
                    {
                        tmp_table[i, counter] = s[index][i];
                    }
                    counter++;
                }

                table = tmp_table;
                #region Вывод
                for (int i = 0; i < size[1]; i++)
                {
                    for (int j = 0; j < size[0]; j++)
                    {
                        writer.Write("{0}", table[i, j]);
                    }
                }
                #endregion
            }
            else if (sizeExist)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Введите размер n m, где n - строки, m - столбцы");
                        string answer = Console.ReadLine();
                        string[] st = answer.Split(' ');

                        if (st.Length == 2)
                        {
                            size[0] = Convert.ToInt32(st[0]); //cтроки
                            size[1] = Convert.ToInt32(st[1]); //столбцы
                        }

                        if (size[0] == 0 && size[1] == 0)
                        {
                            sizeExist = false;
                            break;
                        }

                        char[,] tmp_table = new char[size[1], size[0]];
                        char[] ch = str.ToCharArray();

                        string[] s = new string[size[0]];
                        str = "";

                        //заполняем массив строк, которые будут записаны, как столбцы
                        for (int i = 0, index = 0; i < ch.Length; i++)
                        {
                            str += ch[i];
                            if ((i + 1) % size[1] == 0 && i != 0)
                            {
                                s[index] = str;
                                str = "";
                                index++;

                                continue;
                            }

                        }

                        for (int i = 0; i < size[0]; i++)
                        {
                            for (int j = 0; j < size[1]; j++)
                            {
                                tmp_table[j, i] = s[i][j];
                            }
                        }
                        table = tmp_table;
                        for (int i = 0; i < size[1]; i++)
                        {
                            for (int j = 0; j < size[0]; j++)
                            {
                                writer.Write("{0}", table[i, j]);
                            }
                        }
                        break;
                    }
                    catch
                    {
                        continue;
                    }



                }
            }

            if (!sizeExist)
            {
                int[,] mults = Varioties(str.Length);
                int len = mults.Length / 2;

                char[] ch = str.ToCharArray();
                str = "";

                //заполняем массив строк, которые будут записаны, как столбцы
                for (int j = 0; j < len; j++)
                {
                    char [,] s = new char[mults[j, 0],mults[j,1]];
                    for (int i = 0, index = 0; i < ch.Length; i++)
                    {

                        str += ch[i];
                        if ((i + 1) % mults[j, 0] == 0 && i != 0)
                        {
                            for (int c = 0; c < mults[j,0]; c++)
                            {
                                s[c, index] = str[c];
                            }
                            str = "";
                            index++;
                            continue;
                        }
                    }
                    for (int i = 0; i < mults[j, 0]; i++)
                    {
                        for (int c = 0; c < mults[j, 1]; c++)
                        {
                            writer.Write(s[i,c]);
                        }
                        writer.WriteLine();

                    }
                    writer.WriteLine();
                    writer.WriteLine();


                }


            }
            writer.Close();
            file.Close();
            Console.Read();

        }
    }
}
