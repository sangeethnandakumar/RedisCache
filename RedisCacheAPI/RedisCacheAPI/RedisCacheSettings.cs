using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCacheAPI
{
    public class RedisCacheSettings
    {
        public bool Enable { get; set; }
        public string Connection { get; set; }
    }
}