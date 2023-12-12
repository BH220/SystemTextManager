using BH_Library.Utils;
using BH_Library.Utils.Cryptograph;
using DevExpress.Utils.Extensions;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTextManager.Core;
using SystemTextManager.Core.Helper.Model;

namespace SystemTextManager.SettingJsonMaker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } 

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog sfd = new FolderBrowserDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SettingModel sm = new SettingModel();
                    sm.GitLabPem = "";
                    sm.GitLabIp = "";
                    sm.PrdInfo = new DbInfoModel()
                    {
                        Ip = textEdit1011.Text,
                        Port = (uint)textEdit1211.Text.ToIntEx(),
                        Database = textEdit1311.Text,
                        Id = textEdit1411.Text,
                        Password = textEdit1111.Text
                    };
                    sm.DevInfo = new DbInfoModel()
                    {
                        Ip = textEdit101.Text,
                        Port = (uint)textEdit121.Text.ToIntEx(),
                        Database = textEdit131.Text,
                        Id = textEdit141.Text,
                        Password = textEdit111.Text
                    };
                    sm.LocalInfo = new DbInfoModel()
                    {
                        Ip = textEdit10.Text,
                        Port = (uint)textEdit12.Text.ToIntEx(),
                        Database = textEdit13.Text,
                        Id = textEdit14.Text,
                        Password = textEdit11.Text
                    };
                    sm.Id = "";
                    sm.SaveId = checkEdit1.Checked;
                    sm.SshHost = textEdit6.Text;
                    sm.SshPort = textEdit7.Text.ToIntEx();
                    sm.SshUser = textEdit8.Text;
                    sm.LoginEmailList = new List<string>();
                    if(textEdit9.Text.Contains(","))
                    {
                        var ss = StringUtil.SplitByString(textEdit9.Text, ",");
                        ss.ForEach(x =>
                        {
                            sm.LoginEmailList.Add(x.Trim());
                        });
                    }
                    else
                        sm.LoginEmailList.Add(textEdit9.Text.Trim());
                    string json = JsonConvert.SerializeObject(sm);
                    string encrypt_value = AES.Encrypt(json, Common.AES_KEY);

                    string filePath = sfd.SelectedPath + @"\setting.dat";
                    File.WriteAllText(filePath, encrypt_value);

                    Process.Start("Explorer.exe", "/select,\"" + filePath + "\"");
                }
            } 
        }
    }
}
