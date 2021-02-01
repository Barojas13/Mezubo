using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class JsonWager
    {
        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("Number")]
        public string Number { get; set; }

        [JsonProperty("Cash")]
        public string Cash { get; set; }

        [JsonProperty("IdRoulette")]
        public string IdRoulette { get; set; }

        [JsonProperty("User")]
        public string User { get; set; }
    }
}
