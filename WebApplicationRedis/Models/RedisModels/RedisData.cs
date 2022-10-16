using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationRedis.Models.RedisModels
{
    public class RedisData<T>
    {
        public Type DataType { get; set; }//to know what type data to store
        public T Data { get; set; }
    }
}
