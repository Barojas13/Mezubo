using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBase
{
    public class MethodHash
    {
        IDatabase cache;
        public MethodHash()
        {
            cache = DataBase.Connection.GetDatabase();
        }
        public string Save(string key, Dictionary<string, string> value)
        {

            cache.HashSet(key, ToHashEntryArray(value));
            string id = value.FirstOrDefault().Key;
            if (cache.HashExists(key, id))
            {
                return id;
            }
            else
            {
                return "-1";
            }

        }
        public string FetchValue(string key, string value)
        {
            try
            {
                if (cache.HashExists(key, value))
                {
                    RedisValue result = cache.HashGet(key, value);

                    return result.ToString();
                }
                else
                {
                    return "ERROR";
                }
            }
            catch
            {
                return "ERROR";
            }
        }
        public Dictionary<string, string> FetchAll(string key)
        {
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            try
            {
                HashEntry[] result = cache.HashGetAll(key);
                Dictionary = EntryArrayToDictionary(result);
            }
            catch
            {

            }

            return Dictionary;
        }
        public int HashLength(string key)
        {
            int result = 0;
            try
            {
                result = cache.HashGetAll(key).Length;
            }
            catch
            {
                result = -1;
            }

            return result;
        }
        public bool Exist(string key, RedisValue value)
        {
            return cache.HashExists(key, value);
        }

        public static HashEntry[] ToHashEntryArray(Dictionary<string, string> Items)
        {
            var Entries = new HashEntry[Items.Count()];
            int i = 0;
            foreach (var Item in Items)
            {

                Entries[i++] = new HashEntry(Item.Key, Item.Value);
            }

            return Entries;
        }

        public static Dictionary<string, string> EntryArrayToDictionary(HashEntry[] Items)
        {
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            foreach (var Item in Items)
            {
                Dictionary.Add(Item.Name, Item.Value);
            }

            return Dictionary;
        }
    }
}
