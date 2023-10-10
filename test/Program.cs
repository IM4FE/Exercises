using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
using test;
using static System.Net.WebRequestMethods;
using System.IO;
using System.Net;
using test.ModelsRandApi.Request;
using test.ModelsRandApi.Response;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace test
{
    public class WorkWithString
    {
        private async Task<T[]> GenerateAsync<T, TParams>(T lenght, TParams parameters)
        {
            LowerBaseResonce<T> result = await POSTRequstRandApi<T, TParams, LowerBaseResonce<T>>(new BaseRequestRpc<TParams>(parameters), lenght);
            if (result.random == null)
                return null;
            T[] randomResult = result.random.data;
            return randomResult;
        }
        private async Task<TResponse> POSTRequstRandApi<T, TRequest, TResponse>(BaseRequestRpc<TRequest> requestData, T lenght)
            where TResponse : LowerBaseResonce<T>
        {
            string url = "https://api.random.org/json-rpc/4/invoke";
            int id = (int)DateTime.UtcNow.Ticks;
            requestData.id = id;

            string rpc = JsonSerializer.Serialize(requestData);
            byte[] rpcBuffer = Encoding.UTF8.GetBytes(rpc);

            using (MemoryStream requestStream = new MemoryStream(rpcBuffer))
            {
                try
                {
                    HttpContent requestContent = new StreamContent(requestStream);
                    requestContent.Headers.Add("Content-Type", "application/json");

                    using (var http = new HttpClient())
                    {
                        HttpResponseMessage response = await http.PostAsync(url, requestContent);
                        await response.Content.LoadIntoBufferAsync();
                        var rpcStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        BaseResponseRpc<TResponse> responseData = await JsonSerializer.DeserializeAsync<BaseResponseRpc<TResponse>>(rpcStream).ConfigureAwait(false);

                        if (response.StatusCode != HttpStatusCode.OK || responseData.error != null)
                        {
                            throw new InvalidOperationException(responseData.error.message);
                        }

                        if (responseData.id != id)
                            throw new InvalidOperationException("Response id does not match request id");

                        return responseData.result;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error occured making a request to random.org.", ex);
                }
            }
        }
        static void Swap(ref char x, ref char y)
        {
            var t = x;
            x = y;
            y = t;
        }

        static int Partition(char[] array, int minIndex, int maxIndex)
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
            Swap(ref array[pivot], ref array[maxIndex]);
            return pivot;
        }

        // quick sort
        static char[] QuickSort(char[] array, int minIndex, int maxIndex)
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

        static char[] QuickSort(char[] array)
        {
            return QuickSort(array, 0, array.Length - 1);
        }
        static string SearchVowelRegion(string str)
        {
            int? firstIndex = null;
            int? endIndex = null;
            for (int i = 0; i < str.Length; i++)
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
            string res = firstIndex == null ? null : str.Substring(Convert.ToInt32(firstIndex), Convert.ToInt32(endIndex) - Convert.ToInt32(firstIndex) + 1);
            return res;
        }
        static string CountNumLetter(string str)
        {
            string res = "";
            char[] lettersAll = str.ToCharArray();
            List<char> lettersSort = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                if (lettersSort.Contains(str[i]))
                {
                    continue;
                }
                else
                    lettersSort.Add(str[i]);
            }
            for (int i = 0; i < lettersSort.Count; i++)
            {
                int num = 0;
                for (int j = 0; j < lettersAll.Length; j++)
                {
                    if (lettersSort[i] == lettersAll[j])
                    {
                        num++;
                    }
                }
                res += "\nСимвол '" + lettersSort[i] + "' повторяется " + num + " раз";
            }
            return res;
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
        static string ProcessingString(string str)
        {
            string strRes="";
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
            return strRes;
        }
        public static async Task<string> Work(string str)
        {
            string pattern = "^[a-z]+$";
            Regex rg = new Regex(pattern);
            string strRes, message;
            if (rg.IsMatch(str))
            {
                strRes=ProcessingString(str);
                Console.WriteLine(strRes);

                message = CountNumLetter(strRes);
                Console.WriteLine(message);

                message= SearchVowelRegion(strRes) == null ? "В строке нет гласных букв" : "Подстрока с началом и концом из «aeiouy»: " + SearchVowelRegion(strRes);
                Console.WriteLine(message);

                string options = @"
1) Quick
2) Tree";
                Console.WriteLine(options + "\nНажмите клавишу с номером варианта сортировки\n(Если вы нажали кнопку, которой нет в списке, то будет по умолчанию применятся сортировка методом Quick): ");
                Console.Write("Ваш выбор: ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1: //Quick
                        Console.Write("\nСортировка Quick: ");
                        char[] arr = strRes.ToCharArray();
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
                Console.WriteLine();

                WorkWithString p = new WorkWithString();
                int[] numbers = await p.GenerateAsync(strRes.Length, new RequestParameters(0, strRes.Length - 1, 1));
                int number;
                if (numbers == null)
                {
                    Random random = new Random();
                    number = random.Next(0, strRes.Length - 1);
                    Console.WriteLine("При работе с удаленным Api Random.org была возвращена непредвиденная ошибка\nСтрока, полученная при работе с системным генератором случайных чисел: ");
                }
                else
                {
                    Console.WriteLine("Строка, полученая при работе с API Random.org (генератором случайных чисел): ");
                    number = numbers[0];
                }
                char[] strChars = strRes.ToCharArray();
                strChars[Convert.ToInt32(number)] = ' ';
                foreach (char c in strChars)
                { Console.Write(c); }
                Console.Write("\n");
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
                Console.WriteLine(strRes + string.Join(",", lettersUp));
            }
            return strRes;
        }
    }
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
    public class Program
    {
        static async Task Main(string[] args)
        {
            string str = Console.ReadLine();
            await WorkWithString.Work(str);
            Console.ReadLine();
        }
    }
}
