using BH_Library.Utils.SystemInfo;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SystemTextManager.Core.Helper.Model;

namespace SystemTextManager.Core.Helper
{
    public class LoginHelper
    {
        private static LoginHelper _instance = null;
        public static LoginHelper Instance
        {
            get
            {
                if(_instance == null )
                {
                    _instance = new LoginHelper();
                }
                return _instance;
            }
        }

        public string DoLogin(string pEmail, string pPassword)
        {
            string resultMsg = "";

            try
            {
                string baseUrl = "https://api2.ctkclip.com/api/auth_login";


                var client = new RestClient(baseUrl);

                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "password");
                request.AddParameter("email", pEmail);
                request.AddParameter("password", pPassword);

                string res = "";
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res = response.Content;
                    TokenModel tm = null;
                    if (!string.IsNullOrEmpty(res))
                        tm = JsonConvert.DeserializeObject<TokenModel>(res);

                    if(tm == null)
                        resultMsg = "계정 정보가 올바르지 않습니다.";
                    else if (SettingHelper.Instance.LoginEmailList.Contains(pEmail.ToLower().Trim()) == false)
                        resultMsg = "접근이 허용되지 않은 사용자 입니다.";
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    resultMsg = "계정 정보가 올바르지 않습니다.";
                }
                else
                {
                    resultMsg = "통신 오류가 발생하였습니다."
                        + Environment.NewLine
                        + "Api Status:" + response.StatusCode.ToString();
                }
                

            }
            catch (Exception ex)
            {
                resultMsg = "오류가 발생하였습니다." + Environment.NewLine + ex.Message;
            }

            return resultMsg;
        }
    }
}
