using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class DataBase
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {

            string db = Environment.GetEnvironmentVariable("URLDB");
            string cacheConnection = db;
            ConfigurationOptions option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { cacheConnection }
            };
            return ConnectionMultiplexer.Connect(option);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
