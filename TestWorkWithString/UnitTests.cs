using NuGet.Frameworks;
using SwaggerWebApi;
using SwaggerWebApi.Controllers;
using SwaggerWebApi.Models;
using System.Text.RegularExpressions;

namespace TestWorkWithString
{
    public class Tests
    {
        #region Задание 1
        [TestCase("teststring", "stsetgnirt")]
        [TestCase("getup", "puteggetup")]
        public void TestWorkWithString(string input, string expectResult)
        {
            string res = WorkWithString.Work(input);

            Assert.AreEqual(expectResult, res);
        }
        #endregion
        #region Задание 2
        [TestCase("teststring", true)]
        [TestCase("tesTstring", false)]
        [TestCase("s1ster", false)]
        [TestCase("дадая", false)]
        public void TestStringCheck(string input, bool expectResult)
        {
            string pattern = "^[a-z]+$";
            Regex rg = new Regex(pattern);
            bool res = WorkWithString.StringCheck(input, rg);

            Assert.AreEqual(expectResult, res);
        }
        #endregion
        #region Задание 3
        [Test]
        public void TestNumLetters()
        {
            string str = "stsetgnirt";
            NumLetters[] result=WorkWithString.CountNumLetter(str);

            //Assert.IsInstanceOf<NumLetters>(result);
            Assert.AreEqual(2, result[0].Count);
            Assert.AreEqual(3, result[1].Count);
            Assert.AreEqual(1, result[2].Count);
            Assert.AreEqual(1, result[3].Count);
            Assert.AreEqual(1, result[4].Count);
            Assert.AreEqual(1, result[5].Count);
            Assert.AreEqual(1, result[6].Count);
        }
        #endregion
        #region Задание 4
        [TestCase("stsetgnirt", "etgni")]
        [TestCase("uteggetu", "uteggetu")]
        [TestCase("wqtr",null)]
        public void TestSearchVowelRegion(string input, string expectResult)
        {
            string result = WorkWithString.SearchVowelRegion(input);

            Assert.AreEqual(expectResult, result);
        }
        #endregion
        #region Задание 5
        [Test]
        public void TestTreeSort()
        {
            string str = "teststring";
            var treeNode = new TreeNode(str[0]);
            for (int i = 1; i < str.Length; i++)
            {
                treeNode.Insert(new TreeNode(str[i]));
            }

            Assert.AreEqual("eginrssttt", treeNode.Transform());
        }

        [Test]
        public void TestQuickSort()
        {
            string str = "teststring";
            char[] chars = str.ToCharArray();

            Assert.AreEqual("eginrssttt", WorkWithString.QuickSort(chars));
        }
        #endregion
    }
}