using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    internal class Program
    {
        static string Mirror(int strLength, string str)
        {
            string res="";
            for (int i = 0; i < strLength; i++)
            {
                res += str.Substring(strLength - i-1, 1);
            }
            return res;
        }
        static void Main(string[] args)
        {
            string str = Console.ReadLine();
            string strRes = "";
            if (!(str.Count(Char.IsUpper)>0))
            {
                if (str.Length % 2 == 0)
                {
                    int strLength = str.Length / 2;
                    string strTemp1 = str.Substring(0, strLength);
                    string strTemp2 = str.Substring(strLength);
                    strRes += Mirror(strLength, strTemp1);
                    strRes += Mirror(strLength, strTemp2);
                }
                else
                {
                    strRes = Mirror(str.Length, str) + str;
                }
            }
            else
            {
                strRes = "Ошибка. Были введены не подходящие символы! Неприемлемые символы:";
                for (int i = 0; i < str.Length; i++)
                {
                    char letter = char.Parse(str.Substring(i, 1));
                    if (Char.IsUpper(letter))
                    {
                        if (i == 0)
                            strRes += letter;
                        else
                            strRes +="," + letter;
                    }
                }
            }
            Console.WriteLine(strRes);
            Console.ReadLine();
        }
    }
}
