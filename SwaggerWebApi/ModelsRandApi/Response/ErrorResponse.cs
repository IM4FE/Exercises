﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerWebApi.ModelsRandApi.Response
{
    internal class ErrorResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string[] data { get; set; }
    }
}
