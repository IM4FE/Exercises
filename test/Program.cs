using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace test
{
    internal class Program
    {
        static void SearchVowelRegion (string str)
        {
            int? firstIndex = null;
            int? endIndex=null;
            for (int i=0;i<str.Length;i++)
            {
                if (str[i] == 'a' || str[i] == 'e' || str[i] == 'u' || str[i] == 'y' || str[i] == 'o' || str[i] == 'i')
                {
                    if (firstIndex == null)
                    firstIndex = i;
                    else
                        endIndex = i;
                }
                if (i == str.Length - 1 && endIndex == null)
                    endIndex = firstIndex;
            }
            string res = firstIndex == null ? "В строке нет гласных букв" : "Подстрока с началом и концом из «aeiouy»: " + str.Substring(Convert.ToInt32(firstIndex), Convert.ToInt32(endIndex) - Convert.ToInt32(firstIndex)+1);
            Console.WriteLine(res);
        }
        static void CountNumLetter (string str)
        {
            string res = "";
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
                res += "\nСимвол '" + lettersSort[i] + "' повторяется " + num + " раз";
            }
            Console.WriteLine(res);
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
            string pattern = "^[a-zA-Z]+$";
            Regex rg = new Regex(pattern);
            string str = Console.ReadLine();
            if (rg.IsMatch(str))
            {
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
                    Console.Write(strRes);
                    CountNumLetter(strRes);
                    SearchVowelRegion(strRes);
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
                    Console.WriteLine(strRes);
                }
            }
            else
                Console.WriteLine("Входная строка не должна иметь кириллицу или цифры!");
            Console.ReadLine();
        }
    }
}
