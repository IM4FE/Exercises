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
        static string CountNumLetter (string str)
        {
            char[] lettersAll = str.ToCharArray ();
            List<char> lettersSort=new List<char>();
            for (int i= 0; i < str.Length; i++)
            {
                if (lettersSort.Contains(str[i]))
                {
                    continue;
                }
                else
                    lettersSort.Add(str[i]);
            }
            for (int i= 0; i < lettersSort.Count; i++)
            {
                int num = 0;
                for (int j=0; j< lettersAll.Length;j++)
                {
                    if (lettersSort[i]==lettersAll[j])
                    {
                        num++;
                    }    
                }
                str += "\nСимвол '" + lettersSort[i] + "' повторяется " + num + " раз";
            }
            return str;
        }
        static string Mirror(int strLength, string str)
        {
            string res = "";
            for (int i = 0; i < strLength; i++)
            {
                res += str.Substring(strLength - i - 1, 1);
            }
            return res;
        }
        static void Main(string[] args)
        {
            string str = Console.ReadLine();
            string strRes = "";
            if (!(str.Count(Char.IsUpper) > 0))
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
                strRes = CountNumLetter(strRes);
            }
            else
            {
                strRes = "Ошибка. Были введены не подходящие символы! Неприемлемые символы:";
                List<char> lettersUp = new List<char>();
                for (int i = 0; i < str.Length; i++)
                {
                    char letter = char.Parse(str.Substring(i, 1));
                    if (Char.IsUpper(letter))
                    {
                        if (lettersUp.Contains(str[i]))
                        {
                            continue;
                        }
                        else
                            lettersUp.Add(str[i]);
                    }
                }
                for (int i = 0; i < lettersUp.Count; i++)
                {
                    if (i == 0)
                            strRes += lettersUp[i];
                        else
                            strRes += "," + lettersUp[i];
                }
            }
            Console.WriteLine(strRes);
            Console.ReadLine();
        }
    }
}
