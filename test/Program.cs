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
    class TreeNode //Tree sort
    {
        public TreeNode(char data)
        {
            Data = data;
        }
        public char Data { get; set; } //данные
        public TreeNode Left { get; set; } //левая ветка дерева
        public TreeNode Right { get; set; } //правая ветка дерева
        public void Insert(TreeNode node) //рекурсивное добавление узла в дерево
        {
            if (node.Data < Data)
            {
                if (Left == null)
                {
                    Left = node;
                }
                else
                {
                    Left.Insert(node);
                }
            }
            else
            {
                if (Right == null)
                {
                    Right = node;
                }
                else
                {
                    Right.Insert(node);
                }
            }
        }
        public char[] Transform(List<char> elements = null) //преобразование дерева в отсортированный массив
        {
            if (elements == null)
            {
                elements = new List<char>();
            }

            if (Left != null)
            {
                Left.Transform(elements);
            }

            elements.Add(Data);

            if (Right != null)
            {
                Right.Transform(elements);
            }

            return elements.ToArray();
        }
    }
    internal class Program
    {
        static void Swap(ref char x, ref char y)
        {
            var t = x;
            x = y;
            y = t;
        }

        static int Partition(char [] array, int minIndex, int maxIndex)
        {
            var pivot = minIndex - 1;
            for (var i = minIndex; i < maxIndex; i++)
            {
                if (array[i] < array[maxIndex])
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }
            }

            pivot++;
            Swap(ref array[pivot],ref array[maxIndex]);
            return pivot;
        }

        // quick sort
        static char [] QuickSort(char [] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }

            var pivotIndex = Partition(array, minIndex, maxIndex);
            QuickSort(array, minIndex, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, maxIndex);

            return array;
        }

        static char [] QuickSort(char[] array)
        {
            return QuickSort(array, 0, array.Length - 1);
        }
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
            string pattern = "^[a-z]+$";
            Regex rg = new Regex(pattern);
            string str = Console.ReadLine();
            string strRes = "";
                if (rg.IsMatch(str))
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
                    string options = @"
1) Quick
2) Tree";
                    Console.WriteLine(options+"\nНажмите клавишу с номером варианта сортировки\n(Если вы нажали кнопку, которой нет в списке, то будет по умолчанию применятся сортировка методом Quick): ");
                Console.Write("Ваш выбор: ");
                    switch(Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1: //Quick
                        Console.Write("\nСортировка Quick: ");
                        char [] arr= strRes.ToCharArray();
                        Console.WriteLine(QuickSort(arr));
                            break;
                        case ConsoleKey.D2:
                            var treeNode = new TreeNode(strRes[0]);
                            for (int i = 1; i < strRes.Length; i++)
                            {
                                treeNode.Insert(new TreeNode(strRes[i]));
                            }
                            Console.Write("\nСортировка Tree: "); //Tree
                            Console.WriteLine(treeNode.Transform());
                            break;
                        default:
                            {
                                goto case ConsoleKey.D1;
                            }
                    }
                }
                else
                {
                    strRes = "Ошибка. Были введены не подходящие символы! Неприемлемые символы:";
                    List<char> lettersUp = new List<char>();
                    for (int i = 0; i < str.Length; i++)
                    {
                        char letter = char.Parse(str.Substring(i, 1));
                        if (!rg.IsMatch(letter.ToString()))
                        {
                            if (lettersUp.Contains(str[i]))
                            {
                                continue;
                            }
                            else
                                lettersUp.Add(str[i]);
                        }
                    }
                    Console.WriteLine(strRes+string.Join(",", lettersUp));
                }
            Console.ReadLine();
        }
    }
}
