using DataBase;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class Customer
    {
        MethodHash CustomerDb;
        string hash = "customer";
        public Customer()
        {
            CustomerDb = new MethodHash();
        }
        public int CreateCustomer(User user)
        {
            int length = CustomerDb.HashLength(hash);
            if (length <= 0)
            {
                length = 0;
            }
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            string resultjson = JsonConvert.SerializeObject(JoinObj(user));
            Dictionary.Add(user.Id, resultjson);

            return Convert.ToInt32((CustomerDb.Save(hash, Dictionary)));
        }
        public int UpdateBalanceCustomer(string id, double cash)
        {
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            string json = "";
            json = CustomerDb.FetchValue(hash, id);
            DetailCustomer result = JsonConvert.DeserializeObject<DetailCustomer>(json);
            result.consume = (Convert.ToDouble(result.consume) + cash).ToString();
            result.balance = (Convert.ToDouble(result.balance) - cash).ToString();
            json = JsonConvert.SerializeObject(result);
            Dictionary.Add(id, json);

            return Convert.ToInt32((CustomerDb.Save(hash, Dictionary)));
        }
        public bool ValidateCustomerWager(string id, string cash)
        {
            string json = CustomerDb.FetchValue(hash, id);
            DetailCustomer result = JsonConvert.DeserializeObject<DetailCustomer>(json);
            if (Convert.ToInt32(result.balance) >= Convert.ToInt32(cash))
            {
                return true;
            }

            return false;
        }

        public Dictionary<string, string> ListCustomer()
        {
            return CustomerDb.FetchAll(hash);
        }

        public DetailCustomer JoinObj(User user)
        {
            DetailCustomer obj = new DetailCustomer();
            obj.credit = user.Credit;
            obj.name = user.Name;
            obj.consume = "0";
            obj.balance = user.Credit;

            return obj;
        }
    }
}
