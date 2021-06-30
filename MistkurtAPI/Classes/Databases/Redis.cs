using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeRedis;

namespace MistkurtAPI.Classes.Databases
{
    public static class Redis
    {
        public static RedisClient cli = new("127.0.0.1:6379");
    }
}
