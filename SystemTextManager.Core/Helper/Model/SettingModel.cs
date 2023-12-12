using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTextManager.Core.Helper.Model
{
    [JsonObject]
    public class SettingModel
    {
        [JsonProperty]
        public string GitLabPem { get; set; }
        [JsonProperty]
        public string GitLabIp { get; set; }
        [JsonProperty]
        public DbInfoModel PrdInfo { get; set; }
        [JsonProperty]
        public DbInfoModel DevInfo { get; set; }
        [JsonProperty]
        public DbInfoModel LocalInfo { get; set; }
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public bool SaveId { get; set; }
        [JsonProperty]
        public string SshHost { get; set; }
        [JsonProperty]
        public int SshPort { get; set; }
        [JsonProperty]
        public string SshUser { get; set; }
        [JsonProperty]
        public List<string> LoginEmailList { get; set; }
    }
}
