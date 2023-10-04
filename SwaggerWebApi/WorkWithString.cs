using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using SwaggerWebApi.ModelsRandApi;
using SwaggerWebApi.ModelsRandApi.Request;
using SwaggerWebApi.ModelsRandApi.Response;
using SwaggerWebApi.Models;
using Microsoft.Extensions.Configuration;

namespace SwaggerWebApi
{
    public class WorkWithString
    {
        private async Task<T[]> GenerateAsync<T, TParams>(T lenght, TParams parameters, string url)
        {
            LowerBaseResonse<T> result = await POSTRequstRandApi<T, TParams, LowerBaseResonse<T>>(new BaseRequestRpc<TParams>(parameters), lenght, url);
            if (result.random == null)
                return null;
            T[] randomResult = result.random.data;
            return randomResult;
        }
        private async Task<TResponse> POSTRequstRandApi<T, TRequest, TResponse>(BaseRequestRpc<TRequest> requestData, T lenght, string url)
            where TResponse : LowerBaseResonse<T>
        {
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
        private static char[] QuickSort(char[] array, int minIndex, int maxIndex)
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

        public static char[] QuickSort(char[] array)
        {
            return QuickSort(array, 0, array.Length - 1);
        }
        public static string SearchVowelRegion(string str)
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
            string res = firstIndex == null ? "В строке нет гласных букв" : "Подстрока с началом и концом из «aeiouy»: " + str.Substring(Convert.ToInt32(firstIndex), Convert.ToInt32(endIndex) - Convert.ToInt32(firstIndex) + 1);
            return res;
        }
        public static NumLetters[] CountNumLetter(string str)
        {
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
            NumLetters[] res = new NumLetters[lettersSort.Count];
            for (int i = 0; i < lettersSort.Count; i++)
            {
                int num = 0;
                NumLetters obj= new NumLetters();
                for (int j = 0; j < lettersAll.Length; j++)
                {
                    if (lettersSort[i] == lettersAll[j])
                    {
                        num++;
                    }
                }
                obj.Letter = lettersSort[i];
                obj.Count= num;
                res[i] = obj;
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
        public static async Task<string> RandomSpace(string str, string url)
        {
            string strRes = "";
            WorkWithString p = new WorkWithString();
            int[] numbers = await p.GenerateAsync(str.Length, new RequestParameters(0, str.Length - 1, 1), url);
            int number;
            if (numbers == null)
            {
                Random random = new Random();
                number = random.Next(0, str.Length - 1);
                strRes="При работе с удаленным Api Random.org была возвращена непредвиденная ошибка\nСтрока, полученная при работе с системным генератором случайных чисел: ";
            }
            else
            {
                strRes="Строка, полученая при работе с API Random.org (генератором случайных чисел): ";
                number = numbers[0];
            }
            char[] strChars = str.ToCharArray();
            strChars[Convert.ToInt32(number)] = ' ';
            foreach (char c in strChars)
            { strRes += c; }
            return strRes;
        }
        public static string Work(string str)
        {
            string strRes = "";
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
}
