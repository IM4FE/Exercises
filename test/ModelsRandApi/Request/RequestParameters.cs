using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.ModelsRandApi.Request
{
    internal class RequestParameters
    {
        public string apiKey { get; set; }
        public int n { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int @base { get; set; }
         public RequestParameters(int min, int max, int n)
        {
            this.apiKey = "72b88b6c-7769-4dff-a11b-7a6c0dbab58c";
            this.@base = 10;
            this.n = n;
            this.min = min;
            this.max = max;
        }
    }
}
