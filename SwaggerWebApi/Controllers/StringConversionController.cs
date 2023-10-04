using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using SwaggerWebApi.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace SwaggerWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StringConversionController : ControllerBase
    {
        private readonly ILogger<StringConversionController> _logger;

        public StringConversionController(ILogger<StringConversionController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Get something...
        /// </summary>
        /// <param name="str">�������� ����� ����������:
        /// 1) Quick;
        /// 2) Tree.
        /// ���� �� ����� �����, �������� ��� � ������, �� ����� �������������� ���������� Quick! </param>
        /// <param name="sort"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public ActionResult Get(
            [FromQuery(Name = "string")] string str, int sort)
        {
            string pattern = "^[a-z]+$";
            Regex rg = new Regex(pattern);
            if (rg.IsMatch(str))
            {
                string strRes = WorkWithString.Work(str);
                string sortMethod;
                char[] sortOut;
                switch (sort)
                {
                    case 1: //Quick
                        {
                            sortMethod = "Quick";
                            char[] arr = strRes.ToCharArray();
                            sortOut = WorkWithString.QuickSort(arr);
                            break;
                        }
                    case 2://Tree
                        {
                            sortMethod = "Tree";
                            var treeNode = new TreeNode(strRes[0]);
                            for (int i = 1; i < strRes.Length; i++)
                            {
                                treeNode.Insert(new TreeNode(strRes[i]));
                            }
                            sortOut = treeNode.Transform();
                            break;
                        }
                    default:
                        {
                            goto case 1;
                        }
                }
                BaseResponse output = new BaseResponse
                {
                    result = new LowerBaseResponse
                    {
                        input = str,
                        result = strRes,
                        numLetters = WorkWithString.CountNumLetter(strRes),
                        vowelRegion = WorkWithString.SearchVowelRegion(strRes),
                        sortResult = new Sort { method=sortMethod, output= new string(sortOut)},
                        randomSpaceString = WorkWithString.RandomSpace(strRes).Result,
                    }
                };
                return Ok(output);
            }
            else
            {
                string errorMessage = "������. ���� ������� �� ���������� �������! ������������ �������:";
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
                errorMessage += string.Join(",", lettersUp);
                ErrorResponse output = new ErrorResponse { errorCode = 400, message= errorMessage };
                return BadRequest(output);
            }
        }
    }
}