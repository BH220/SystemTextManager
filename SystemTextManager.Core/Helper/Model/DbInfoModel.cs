using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTextManager.Core.Helper.Model
{
    [JsonObject]
    public class DbInfoModel
    {
        [JsonProperty]
        public string Ip { get; set; }
        [JsonProperty]
        public uint Port { get; set; }
        [JsonProperty]
        public string Database { get; set; }
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Password { get; set; }
    }
}
