using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerWebApi.ModelsRandApi.Request
{
    internal class BaseRequestRpc<T>
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public T @params { get; set; }
        public int id { get; set; }

        public BaseRequestRpc(T @params)
        {
            this.jsonrpc = "2.0";
            this.method = "generateSignedIntegers";
            this.@params = @params;
        }
    }
}
