using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Хемминг
{
    class Program
    {
        static int To10(int n)
        {
            int max = 1;
            long tmp = n;
            while (tmp / 10 != 0)
            {
                max++;
                tmp /= 10;

            }

            int ans = 0;
            tmp = n;
            for (int i = 0; i < max; i++)
            {
                long divi = (tmp % 10) * 2;
                ans += Convert.ToInt32(Math.Pow(divi, i));
                tmp /= 10;
            }
            if (n % 10 == 0) { ans--; }
            return ans;
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
            List<string> str_arr = new List<string>();
            string tmp_str = "";
            int counter = 0;
            #region Разделяем по 15
            foreach (char ch in str)
            {
                if (counter == 15)
                {
                    str_arr.Add(tmp_str);
                    tmp_str = "";
                    counter = 0;
                }
                if (ch != ' ')
                {
                    tmp_str += ch;
                    counter++;
                }
            }
            str_arr.Add(tmp_str);
            #endregion  
            string concat_str = "";
            foreach (string s in str_arr)
            {
                string ss = s;
                string misstake_pos;

                #region Находим место ошибки
                int c1 = 0, c2 = 0, c4 = 0, c8 = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    c1 += Convert.ToInt32(Convert.ToString(s[i]));
                }
                for (int i = 1; i < s.Length - 1; i += 4)
                {
                    c2 += Convert.ToInt32(Convert.ToString(s[i])) + Convert.ToInt32(Convert.ToString(s[i + 1]));
                }
                for (int i = 3; i < s.Length; i += 8)
                {
                    c4 += Convert.ToInt32(Convert.ToString(s[i])) + Convert.ToInt32(Convert.ToString(s[i + 1])) + Convert.ToInt32(Convert.ToString(s[i + 2])) + Convert.ToInt32(Convert.ToString(s[i + 3]));
                }
                for (int i = 7; i < s.Length; i++)
                {
                    c8 += Convert.ToInt32(Convert.ToString(s[i]));
                }
                c1 = c1 % 2 == 0 ? 0 : 1;
                c2 = c2 % 2 == 0 ? 0 : 1;
                c4 = c4 % 2 == 0 ? 0 : 1;
                c8 = c8 % 2 == 0 ? 0 : 1;
                if (c1 == 0 && c2 == 0 && c4 == 0 && c8 == 0) misstake_pos = "0";
                else misstake_pos = Convert.ToString(c1) + Convert.ToString(c2) + Convert.ToString(c4) + Convert.ToString(c8);
                char[] tmp = misstake_pos.Reverse().ToArray();
                misstake_pos = new string(tmp);
                #endregion

                #region Исправляем ошибку
                int misstake_index = To10(Convert.ToInt32(misstake_pos)) - 1;
                if (misstake_index != -1)
                {
                    char removed = ss[misstake_index];
                    ss = ss.Remove(misstake_index, 1);
                    if (removed == '0') ss = ss.Insert(misstake_index, "1");
                    else ss = ss.Insert(misstake_index, "0");

                }
                #endregion

                #region Удаляем защитные биты
                ss = ss.Remove(0, 2);
                ss = ss.Remove(1, 1);
                ss = ss.Remove(4, 1);
                #endregion

                concat_str += ss; //объединяем строки
            }
            string[] answ_str;
            #region Раскладываем исправленные строки в массив

            int count = 0;
            str = "";
            foreach (char ch in concat_str)
            {
                if (count == 8)
                {
                    str += " ";
                    count = 0;
                }
                str += ch;
                count++;
            }
            answ_str = str.Split(' ');
            #endregion

            List<int> answ_ints = new List<int>();
            for (int i = 0; i < answ_str.Length; i++)
            {
                int inter = Convert.ToInt32(answ_str[i]);
                if (inter != 0)
                    answ_ints.Add(inter);
            }


            File.Delete("out.txt");
            FileStream out_file = new FileStream("Out.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(out_file);

            #region Выполнение действий исходя из ответа
            while (true)
            {
                try
                {
                    Console.WriteLine("0 - A10, 1 - A2, 2 - ASCII");
                    string question = Console.ReadLine();
                    if (question == "0")
                    {
                        foreach (int i in answ_ints)
                        {
                            writer.Write(Convert.ToString(To10(i)), Encoding.ASCII);
                            writer.Write(" ");
                        }
                        break;

                    }
                    else if (question == "1")
                    {
                        foreach (string i in answ_str)
                        {
                            writer.Write(i);
                            writer.Write(" ");
                        }
                        break;
                    }
                    else if (question == "2")
                    {
                        foreach (int i in answ_ints)
                        {
                            char ch = Encoding.GetEncoding(1251).GetString(new byte[] { (byte)To10(i) })[0];
                            writer.Write(Convert.ToString(ch), Encoding.ASCII);
                        }

                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            #endregion

            writer.Close();
            file.Close();
        }
    }
}
