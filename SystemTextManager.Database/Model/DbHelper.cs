using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Renci.SshNet;
using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemTextManager.Core;
using SystemTextManager.Core.Helper;

namespace SystemTextManager.Database.Model
{
    public class DbHelper
    {
        private static DbHelper _instance = null;

        private PrivateKeyFile PemKey = null;
        private SshClient sshClient = null;
        private ForwardedPortLocal port = null;
        private MySqlConnection conn = null;

        string strFatConnect = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
        string strPrdConn = null;
        string strDevConn = null;
        string strLocalConn = null;
        DbTypes nowDb = DbTypes.None;

        public static DbHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbHelper();
                    _instance.InitLoad();
                }
                return _instance;
            }
        }

        private void InitLoad()
        {
            PemKey = new PrivateKeyFile(SettingHelper.Instance.GitlabPem);
            sshClient = new SshClient(
                SettingHelper.Instance.SshHost, 
                SettingHelper.Instance.SshPort,
                SettingHelper.Instance.SshUser, 
                PemKey);
            sshClient.Connect();
        }

        public List<SearchModel> GetResult(DbTypes dbType, string query)
        {
            List<SearchModel> result = new List<SearchModel>();
            while (true)
            {
                if (ConnectDb(dbType))
                    break;
                Thread.Sleep(5000);
            }
            try
            {
                conn.Open();

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SearchModel sm = new SearchModel();
                            sm.Site = reader["site"].ToString();
                            sm.Page = reader["page"].ToString();
                            sm.Key = reader["key"].ToString();
                            sm.Ko = reader["ko"].ToString();
                            sm.En = reader["en"].ToString();
                            result.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return result;
        }

        private bool ConnectDb(DbTypes dbType)
        {
            bool result = false;
            try
            {
                if (sshClient.IsConnected == false)
                    sshClient.Connect();

                if (nowDb != dbType)
                {//포트를 변경해야함
                    nowDb = dbType;
                    string mysqlHost = "";
                    uint mysqlPort = 0;
                    if (dbType == DbTypes.PRD || dbType == DbTypes.DEV)
                    {//port와 mysql재설정
                        switch (dbType)
                        {
                            case DbTypes.PRD:
                                mysqlHost = SettingHelper.Instance.PrdDbInfo.Ip;
                                mysqlPort = SettingHelper.Instance.PrdDbInfo.Port;
                                break;
                            case DbTypes.DEV:
                                mysqlHost = SettingHelper.Instance.DevDbInfo.Ip;
                                mysqlPort = SettingHelper.Instance.DevDbInfo.Port;
                                break;
                        }
                        if (port != null)
                            sshClient.RemoveForwardedPort(port);
                        port = new ForwardedPortLocal("127.0.0.1", mysqlPort, mysqlHost, mysqlPort);
                        sshClient.AddForwardedPort(port);
                        port.Start();
                        switch (dbType)
                        {
                            case DbTypes.PRD:
                                strPrdConn = string.Format(strFatConnect,
                                    "127.0.0.1", port.BoundPort,
                                    SettingHelper.Instance.DevDbInfo.Database,
                                    SettingHelper.Instance.DevDbInfo.Id,
                                    SettingHelper.Instance.DevDbInfo.Password);
                                conn = new MySqlConnection(strPrdConn);
                                break;
                            case DbTypes.DEV:
                                strDevConn = string.Format(strFatConnect,
                                    "127.0.0.1", port.BoundPort,
                                    SettingHelper.Instance.PrdDbInfo.Database,
                                    SettingHelper.Instance.PrdDbInfo.Id,
                                    SettingHelper.Instance.PrdDbInfo.Password);
                                conn = new MySqlConnection(strDevConn);
                                break;
                        }
                    }
                    else
                    {//mysql만 재설정
                        sshClient.Disconnect();
                        strLocalConn = string.Format(strFatConnect,
                                "127.0.0.1", SettingHelper.Instance.LocalDbInfo.Port,
                                SettingHelper.Instance.LocalDbInfo.Database,
                                SettingHelper.Instance.LocalDbInfo.Id,
                                SettingHelper.Instance.LocalDbInfo.Password);
                        conn = new MySqlConnection(strLocalConn);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 입력 혹은 수정
        /// </summary>
        /// <param name="dbType">대상 데이터베이스</param>
        /// <param name="saveModel">저장할 데이터</param>
        /// <param name="updateForce">true 일 경우 중복 체크 없이 업데이트
        /// false일 경우 중복 상황을 리턴
        /// </param>
        /// <returns></returns>
        public SearchModel InsertOrUpdate(DbTypes dbType, SearchModel saveModel, bool updateForce = false)
        {
            SearchModel existsModel = null;
            while (true)
            {
                if (ConnectDb(dbType))
                    break;
                Thread.Sleep(5000);
            }
            try
            {
                conn.Open();

                string query = @"
                SELECT
                    COALESCE(en.`site`, ko.`site`) AS `site`,
                    COALESCE(en.`page`, ko.`page`) AS `page`,
                    COALESCE(en.`key`, ko.`key`) AS `key`,
                    en.`text` AS `en`,
                    ko.`text` AS `ko`
                FROM 
                    ( SELECT `site`, `page`, `key`, `text` FROM `ctk_clip`.`system_text` WHERE `language` = 'en' ) AS en
                    INNER JOIN
                    ( SELECT `site`, `page`, `key`, `text` FROM `ctk_clip`.`system_text` WHERE `language` = 'ko' ) AS ko
                    ON 
	                    en.`site` = ko.`site` AND
                        en.`page` = ko.`page` AND
                        en.`key` = ko.`key` 
                WHERE 
                    ( en.`site` = '" + saveModel.Site.Trim() + "' OR ko.`site` = '" + saveModel.Site.Trim() + @"')
                    AND ( en.`page` = '" + saveModel.Page.Trim() + "' OR ko.`page` = '" + saveModel.Page.Trim() + @"')
                    AND ( en.`key` = '" + saveModel.Key.Trim() + "' OR ko.`key` = '" + saveModel.Key.Trim() + "');";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            existsModel = new SearchModel();
                            existsModel.Site = reader["site"].ToString();
                            existsModel.Page = reader["page"].ToString();
                            existsModel.Key = reader["key"].ToString();
                            existsModel.Ko = reader["ko"].ToString();
                            existsModel.En = reader["en"].ToString();
                        }
                    }
                }


                if (existsModel != null)
                {//업데이트 할지 말지 미정
                    if (updateForce)
                    {//업데이트
                        string update_fat = "UPDATE `ctk_clip`.`system_text` SET `text` = '{0}', `updated_at` = '" 
                            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE `site` = '"
                            + saveModel.Site.Trim() + "' AND `page` = '" 
                            + saveModel.Page.Trim() + "' AND `key` = '" 
                            + saveModel.Key.Trim() + "' AND `language` = '{1}';";
                        string update_ko = string.Format(update_fat, saveModel.Ko.Trim(), "ko");
                        using (var cmd = new MySqlCommand(update_ko, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        string update_en = string.Format(update_fat, saveModel.En.Trim(), "en");
                        using (var cmd = new MySqlCommand(update_en, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {//인서트
                    string insert_query = "INSERT INTO `ctk_clip`.`system_text` (`site`,`page`,`key`,`text`,`language`,`created_at`) VALUES ('"
                        + saveModel.Site.Trim() + "','"
                        + saveModel.Page.Trim() + "','"
                        + saveModel.Key.Trim() + "','"
                        + saveModel.Ko.Trim() + "','ko','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'), ('"
                        + saveModel.Site.Trim() + "','"
                        + saveModel.Page.Trim() + "','"
                        + saveModel.Key.Trim() + "','"
                        + saveModel.En.Trim() + "','en','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                    using (var cmd = new MySqlCommand(insert_query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return existsModel;
        }
    }
}
