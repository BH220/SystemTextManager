using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemTextManager.Core
{
    public static class Common
    {
        public static Mutex RunMutex { get; set; }
        public static string AES_KEY = "sTm_c1iP";
        public static bool IsExitPossible { get; set; }
        public static string SettingInfoPath
        {
            get
            {
                string result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                result = result + "\\CTK\\STM\\";
                if(Directory.Exists(result) == false)
                    Directory.CreateDirectory(result);  
                return result;
            }
        }
    }
}
