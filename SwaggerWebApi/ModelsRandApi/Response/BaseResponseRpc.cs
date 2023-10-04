using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerWebApi.ModelsRandApi.Response
{
    internal class BaseResponseRpc<T>
    {
        public T result { get; set; }
        public ErrorResponse error { get; set; }
        public int id { get; set; }
    }
}
