using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace NotasProject.Models.Jsons
{
    [JsonObject("tokenManagement")]
    public class SinglePropJson
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
