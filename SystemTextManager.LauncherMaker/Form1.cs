using BH_Library.Utils.Zip.GZip;
using DevExpress.Data.ExpressionEditor;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
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
using static DevExpress.XtraEditors.RoundedSkinPanel;

namespace SystemTextManager.LauncherMaker
{
    public partial class Form1 : XtraForm
    {
        BackgroundWorker worker = null;
        /// <summary>
        /// 0:파일전체경로
        /// 1:버전(없으면공란)
        /// 2:크기
        /// 3:수정한날짜
        /// </summary>
        string strFileCheck = "{0};{1};{2};{3};";

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            layoutControlItem4.Text = string.Format("진행률({0}%)", e.ProgressPercentage);
            memoEdit1.AppendText(e.UserState.ToString());
            memoEdit1.AppendText(Environment.NewLine);
            memoEdit1.Select(memoEdit1.Text.Length, 0);
            memoEdit1.ScrollToCaret();
            progressBarControl1.Position = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonEdit1.Enabled = true;
            Process.Start(buttonEdit1.Text + "\\deploy_ctk\\");
        }

        List<FileInfo> lstFile = new List<FileInfo>();
        List<FileModel> lstFileModel = new List<FileModel>();
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            lstFile = new List<FileInfo>();
            GetFile(buttonEdit1.Text);
            int now = 1;
            int tot = lstFile.Count;
            int percent = 0;
            string logFat = "({0})[{1:#,#}/{2:#,#}] {3} {4}";
            string log = "";
            string rootPath = buttonEdit1.Text;
            string deployPath = buttonEdit1.Text + "\\deploy_ctk";
            string relFile = "";
            string targetFile = "";
            string hashString = "";
            FileModel fm = null;
            foreach (FileInfo fifo in lstFile)
            {
                fm = new FileModel();
                fm.Name = fifo.FullName.Replace(buttonEdit1.Text, "");
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fifo.FullName);
                fm.Version = versionInfo.FileVersion; // 파일 버전 정보
                fm.Size = fifo.Length;
                
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(fifo.FullName))
                    {
                        byte[] hash = md5.ComputeHash(stream);
                        hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    }
                }
                fm.MD5 = hashString;

                percent = (int)((now * 100) / tot);
                relFile = fifo.FullName.Replace(rootPath, "");
                targetFile = deployPath + relFile;
                if(Directory.Exists(Path.GetDirectoryName(targetFile)) == false)
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                GzHelper.Compress(fifo);
                File.Move(fifo.FullName + ".gz", targetFile + ".gz");

                lstFileModel.Add(fm);
                log = string.Format(logFat, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), now, tot, fifo.FullName, "Compressed.");
                worker.ReportProgress(percent, log);
                now++;
            }
            File.WriteAllText(deployPath + "\\update.json", JsonConvert.SerializeObject(lstFileModel));
        }

        private void GetFile(string path)
        {
            string[] dics = Directory.GetDirectories(path);
            foreach(string dic in dics)
            {
                GetFile(dic);
            }
            string[] files = Directory.GetFiles(path);
            foreach(string file in files)
            {
                lstFile.Add(new FileInfo(file));
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                using (FolderBrowserDialog frm = new FolderBrowserDialog())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        buttonEdit1.Text = frm.SelectedPath;
                    }
                }
            }
            else if (e.Button.Index == 1)
            {
                if (string.IsNullOrEmpty(buttonEdit1.Text) == false)
                    Process.Start(buttonEdit1.Text);
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string deployPath = buttonEdit1.Text + "\\deploy_ctk";
            progressBarControl1.Position = 0;
            if(Directory.Exists(deployPath))
            {
                Directory.Delete(deployPath, true);
            }
            buttonEdit1.Enabled = false;
            worker.RunWorkerAsync();
        }
    }
}
