using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerWebApi.ModelsRandApi.Response
{
    internal class LowerBaseResonse <T>
    {
        public LowestBaseResponse<T> random { get; set; }
    }
}
