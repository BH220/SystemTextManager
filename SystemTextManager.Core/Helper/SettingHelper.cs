using BH_Library.Utils.Cryptograph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTextManager.Core.Helper.Model;

namespace SystemTextManager.Core.Helper
{
    public class SettingHelper
    {
        private static SettingHelper _instance = null;
        private SettingModel settingModel = null;
        private string SettingSavePath = Common.SettingInfoPath + "setting.dat";

        #region 프로퍼티

        #region GET 전용
        public string SshHost { get { return settingModel.SshHost; } }
        public int SshPort { get { return settingModel.SshPort; } }
        public string SshUser { get { return settingModel.SshUser; } }
        public List<string> LoginEmailList { get { return settingModel.LoginEmailList; } }
        public DbInfoModel PrdDbInfo { get { return settingModel.PrdInfo; } }
        public DbInfoModel DevDbInfo { get { return settingModel.DevInfo; } }
        public DbInfoModel LocalDbInfo { get { return settingModel.LocalInfo; } }
        public string GitlabIp { get { return settingModel.GitLabIp; } }
        #endregion

        public string ID
        {
            get
            {
                return settingModel.Id;
            }
            set
            {
                settingModel.Id = value;
                SaveSetting();
            }
        }
        public string GitlabPem
        {
            get
            {
                return settingModel.GitLabPem;
            }
            set
            {
                settingModel.GitLabPem = value;
                SaveSetting();
            }
        }
        public bool SaveId { get
            {
                return settingModel.SaveId;
            }
            set
            {
                settingModel.SaveId = value;
                SaveSetting();
            }
        }    
        #endregion

        public static SettingHelper Instance
        {
            get
            {
                if(_instance == null )
                {
                    _instance = new SettingHelper();
                    _instance.InitLoad();
                }
                return _instance;
            }
        }

        public void SetID(string id)
        {
            ID = id;
            settingModel.Id = id;
            SaveSetting();
        }


        public void SetGitlabPEM(string gitlabpem)
        {
            GitlabPem = gitlabpem;
            settingModel.GitLabPem = gitlabpem;
            SaveSetting();
        }

        public void SetId(bool saveId)
        {
            SaveId = saveId;
            settingModel.SaveId = saveId;
            SaveSetting();
        }

        public void SaveSetting()
        {
            if (File.Exists(SettingSavePath))
                File.Delete(SettingSavePath);
            string jsonSm = AES.Encrypt(JsonConvert.SerializeObject(settingModel), Common.AES_KEY);
            File.WriteAllText(SettingSavePath, jsonSm);
        }

        private void InitLoad()
        {
            string jsonSm = AES.Decrypt(File.ReadAllText(SettingSavePath), Common.AES_KEY);
            settingModel = JsonConvert.DeserializeObject<SettingModel>(jsonSm); 

            ID = settingModel.Id; 
            SaveId = settingModel.SaveId;
            GitlabPem = settingModel.GitLabPem;

        }
    }
}
