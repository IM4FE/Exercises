using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public static int currentNumberOfUsers = 0;
        private static object _lockObject = new object();

        private IConfiguration configuration;
        public StringConversionController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        //private readonly ILogger<StringConversionController> _logger;

        //public StringConversionController(ILogger<StringConversionController> logger)
        //{
        //    _logger = logger;
        //}
        /// <summary>
        /// Get something...
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sort">Выберите метод сортировки:
        /// 1) Quick;
        /// 2) Tree.
        /// Если вы ввели число, которого нет в списке, то будет воспроизведена сортировка Quick! </param>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(typeof(BaseResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 503)]
        public ActionResult Get(
            [FromQuery(Name = "string")] string str, int sort)
        {
            if (CheckRequestsLimited())
            {
                string errorMessage = "Ошибка. Программа не расчитана на обработку большего количества запросов ";
                ErrorResponse output = new ErrorResponse { errorCode = 503, message = errorMessage };
                return StatusCode(StatusCodes.Status503ServiceUnavailable, output);
            }
            try
            {
                lock (_lockObject)
                {
                    currentNumberOfUsers++;
                }
                string pattern = "^[a-z]+$";
                Regex rg = new Regex(pattern);
                if (!configuration.GetSection("Settings:Blacklist").Get<string[]>().Contains(str))
                {
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
                                sortResult = new Sort { method = sortMethod, output = new string(sortOut) },
                                randomSpaceString = WorkWithString.RandomSpace(strRes, configuration["RandomApi"].ToString()).Result,
                            }
                        };
                        return Ok(output);
                    }
                    else
                    {
                        string errorMessage = "Ошибка. Были введены не подходящие символы! Неприемлемые символы:";
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
                        ErrorResponse output = new ErrorResponse { errorCode = 400, message = errorMessage };
                        return BadRequest(output);
                    }
                }
                else
                {
                    string errorMessage = "Ошибка. Входная строка входит в черный список строк: " + str;
                    ErrorResponse output = new ErrorResponse { errorCode = 400, message = errorMessage };
                    return BadRequest(output);
                }
            }
            catch
            {
                string errorMessage = "Ошибка. Работа на сервере закончилась ошибкой: " + str;
                ErrorResponse output = new ErrorResponse { errorCode = 500, message = errorMessage };
                return BadRequest(output);
            }
            finally
            {
                lock(_lockObject)
                {
                    currentNumberOfUsers--;
                }
            }
        }
        private bool CheckRequestsLimited()
        {
            int maxLimit = configuration.GetValue<int>("Settings:ParallelLimit");
            lock (_lockObject)
            {
                return currentNumberOfUsers >= maxLimit;
            }
        }
    }
}