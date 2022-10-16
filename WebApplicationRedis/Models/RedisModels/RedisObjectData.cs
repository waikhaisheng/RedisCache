using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationRedis.Models.RedisModels
{
    public class RedisObjectData
    {
        public string DataType { get; set; }
        public object Data { get; set; }
    }
}
