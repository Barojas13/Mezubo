using DataBase;
using Microsoft.VisualStudio.Services.Account;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class Wager
    {
        MethodHash WagerDb;
        Roulette Roulette;
        Customer Customer;
        public Wager()
        {
            WagerDb = new MethodHash();
            Roulette = new Roulette();
            Customer = new Customer();
        }

        public string CreateWager(WagerModel ObjWager, string user)
        {
            WagerSave FinalWager = JoinObj(ObjWager, user);
            string ResultValidate = ValidateWager(FinalWager);
            if (ResultValidate.Equals("OK"))
            {
                string resultjson = "";
                List<WagerSave> ListResult = new List<WagerSave>();
                Dictionary<string, string> dictionary = WagerDb.FetchAll(FinalWager.IdRoulette);
                if (dictionary.Count > 0)
                {
                    string json = dictionary["Wager"];
                    ListResult = JsonConvert.DeserializeObject<List<WagerSave>>(json);
                    ListResult.Add(FinalWager);
                    resultjson = JsonConvert.SerializeObject(ListResult);
                    dictionary["Wager"] = resultjson;
                    ListResult.Clear();
                }
                else
                {
                    ListResult.Add(FinalWager);
                    resultjson = JsonConvert.SerializeObject(ListResult);
                    dictionary.Add("Wager", resultjson);
                    ListResult.Clear();
                }
                if (WagerDb.Save(FinalWager.IdRoulette, dictionary).Equals("-1"))
                {
                    return "Hubo una desconexion en el guardado";
                }
                else
                {
                    Customer.UpdateBalanceCustomer(FinalWager.User, Convert.ToDouble(FinalWager.Cash));

                    return "OK";
                }
            }
            else
            {
                return ResultValidate;
            }
        }

        public WagerSave JoinObj(WagerModel ObjWager, string user)
        {
            WagerSave Obj = new WagerSave();
            Obj.IdRoulette = ObjWager.IdRoulette;
            Obj.Cash = ObjWager.Cash;
            Obj.Color = ObjWager.Color.ToUpper();
            Obj.Number = ObjWager.Number;
            Obj.User = user;

            return Obj;
        }

        public string ValidateWager(WagerSave ObjWager)
        {
            int temp = 0;
            if (!Enum.IsDefined(typeof(EColor), ObjWager.Color.ToUpper()))
            {
                return "Solo se admite el color Negro y Rojo";
            }

            if (!Enumerable.Range(0, 36).Contains(Convert.ToInt32(ObjWager.Number)))
            {
                return "Solo se admiten numeros de 0 a 36";
            }

            if (!int.TryParse(ObjWager.Cash, out temp))
            {
                return "El valor a apostar debe ser numerico";
            }
            else
            {
                if (Convert.ToDouble(ObjWager.Cash) > 10000)
                {
                    return "El valor a apostar no debe exceder de los 10.000 dolares";
                }
            }

            if (!Roulette.ValidateStateRoulette(ObjWager.IdRoulette, "OPEN"))
            {
                return "La ruleta debe estar abierta para poder hacer sus apuestas";
            }

            if (!Customer.ValidateCustomerWager(ObjWager.User, ObjWager.Cash))
            {
                return "El usuario no tiene suficiente saldo";
            }

            return "OK";
        }

        public string ListWager(string IdRoulette)
        {
            Roulette.CloseRoulette(IdRoulette);
            List<WagerSave> ListResult = new List<WagerSave>();
            Dictionary<string, string> dictionary = WagerDb.FetchAll(IdRoulette);
            string json = dictionary["Wager"].Replace('\"', '"');

            return ResultRoulette(IdRoulette, json); ;
        }

        public string ResultRoulette(string IdRoulette, string dictionary)
        {
            string CasheErned = string.Empty;
            string ResultWager = string.Empty;

            int NumberWinner = NumberRandom();
            string ColorWinner = ValidatePrimeNumber(NumberWinner);

            List<JsonWager> ObjOrderList = JsonConvert.DeserializeObject<List<JsonWager>>(dictionary);
            List<WagerResult> Obj = new List<WagerResult>();

            foreach (var item in ObjOrderList)
            {
                CasheErned = "0";
                ResultWager = "PERDEDOR";

                if (NumberWinner.ToString() == item.Number)
                {
                    CasheErned = (Convert.ToInt32(item.Cash) * 5).ToString();
                    ResultWager = "GANADOR";
                }

                if (ColorWinner == item.Color)
                {
                    CasheErned = ((Convert.ToInt32(item.Cash) * 1.8) + CasheErned).ToString();
                    ResultWager = "GANADOR";
                }

                Obj.Add(new WagerResult { IdRoulette = item.IdRoulette,
                Cash = item.Cash,
                Color = item.Color.ToUpper(),
                Number = item.Number,
                User = item.User,
                CasheErned = CasheErned,
                NumberWinner = NumberWinner.ToString(),
                ResultWager = ResultWager,});

                
            }

            return JsonConvert.SerializeObject(Obj);
        }

        public int NumberRandom()
        {
            Random rnd = new Random();
            return rnd.Next(0, 32);
        }

        public string ValidatePrimeNumber(int NumberValidate)
        {
            for (int i = 2; i < NumberValidate; i++)
            {
                if ((NumberValidate % i) == 0)
                {
                    return "NEGRO";
                }
            }
            return "ROJO";
        }
    }
}
