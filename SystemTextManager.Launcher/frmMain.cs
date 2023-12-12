using BH_Library.Utils.SystemInfo;
using BH_Library.Utils.Zip.GZip;
using DevExpress.Utils.DPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static DevExpress.XtraEditors.RoundedSkinPanel;

namespace SystemTextManager.Launcher
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        private string CDN_URL = "https://dwjmi9yb8x8cg.cloudfront.net/system_text/new";
        private string RootPath = @"c:\ctk_clip\SystemTextManager";

        private Point _MousePoint;
        BackgroundWorker worker = null;

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Control[] controls = new Control[] { panel1, lbNow, lbPercent, labelControl4, pictureEdit1, progressNow, progressTotal, this };
            foreach (Control ctl in controls)
            {
                ctl.MouseDown += FrmLogin_MouseDown;
                ctl.MouseMove += FrmLogin_MouseMove;
                ctl.MouseDoubleClick += FrmLogin_MouseDoubleClick;
            }
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SetSystemSettingJson();
            worker.RunWorkerAsync();
        }

        private void SetSystemSettingJson()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = path + "\\CTK\\STM\\";
            string file = path + "setting.dat";
            if (File.Exists(file) == false)
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                string url = CDN_URL + "/setting.dat";
                WebClient wc = new WebClient();
                wc.DownloadFile(url, file);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressInfo pi = e.UserState as ProgressInfo;
            if (pi == null) return;
            if (pi.OnlyNowStepChange)
            {
                progressNow.Position = pi.NowStep;
            }
            else
            {
                lbPercent.Text = string.Format("{0}%", pi.TotalStep);
                lbNow.Text = string.Format("({0:#,#}/{1:#,#}) {2}", pi.NowFileCnt, pi.TotalFileCnt, pi.DisplayText);
                progressNow.Position = pi.NowStep;
                progressTotal.Position = pi.TotalStep;
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Process.Start(RootPath + "\\SystemTextManager.exe");
            Application.Exit();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgressInfo pi = new ProgressInfo();
            pi.DisplayText = "서버에서 업데이트 대상 파일을 확인합니다.";
            pi.NowStep = 0;
            pi.TotalStep = 0;
            worker.ReportProgress(0, pi);
            List<FileModel> lstServerFile = GetServerFileList();
            List<FileModel> lstUpdateTarget = new List<FileModel>();
            lstServerFile.ForEach(x =>
            {
                if(IsUpdateTarget(x))
                    lstUpdateTarget.Add(x);
            });

            //string rootPath = Application.StartupPath;
            string url = "";
            string downGzFile = "";
            string downLocalFile = "";
            string newFile = "";
            int now = 1;
            int tot = lstUpdateTarget.Count;
            //WebClient wc = new WebClient();
            //wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            foreach(FileModel fm in lstUpdateTarget)
            {
                pi.OnlyNowStepChange = false;
                pi.NowStep = 0;
                pi.TotalStep = (int)((now * 100) / tot);
                pi.DisplayText = fm.Name + " - 다운로드";
                pi.NowFileCnt = now;
                pi.TotalFileCnt = tot;
                worker.ReportProgress(0, pi);
                //파일을 다운로드
                url = CDN_URL + fm.Name.Replace("\\", "/") + ".gz";
                downGzFile = RootPath + "\\Updates" + fm.Name + ".gz";
                string dic = Path.GetDirectoryName(downGzFile);
                if(Directory.Exists(dic) == false)
                    Directory.CreateDirectory(dic);
                
                //wc.DownloadFile(new Uri(url), downGzFile); 

                Uri uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Close();
                Int64 iSize = response.ContentLength;
                Int64 iRunningByteTotal = 0;
                using (WebClient client = new WebClient())
                {
                    using (Stream streamRemote = client.OpenRead(new Uri(url)))
                    {
                        using (Stream streamLocal = new FileStream(downGzFile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int iByteSize = 0;
                            byte[] byteBuffer = new byte[iSize];
                            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                streamLocal.Write(byteBuffer, 0, iByteSize);
                                iRunningByteTotal += iByteSize;
                                double dIndex = (double)(iRunningByteTotal);
                                double dTotal = (double)byteBuffer.Length;
                                double dProgressPercentage = (dIndex / dTotal);
                                int iProgressPercentage = (int)(dProgressPercentage * 100);
                                pi.NowStep = iProgressPercentage;
                                worker.ReportProgress(0, pi);
                            }
                            streamLocal.Close();
                        }
                        streamRemote.Close();
                    }
                }
                 
                //downLocalFile = Path.GetFileNameWithoutExtension(downGzFile);
                //압축풀고이동
                pi.OnlyNowStepChange = false;
                pi.NowStep = 0;
                pi.TotalStep = (int)((now * 100) / tot);
                pi.DisplayText = fm.Name + " - 업데이트";
                worker.ReportProgress(0, pi);
                downLocalFile = RootPath + "\\Updates" + fm.Name;
                GzHelper.Decompress(downGzFile, downLocalFile);
                newFile = RootPath + fm.Name;
                if(Directory.Exists(Path.GetDirectoryName(newFile)) == false)
                    Directory.CreateDirectory(Path.GetDirectoryName(newFile));
                File.Move(downLocalFile, newFile);
                now++;
            }
            pi.OnlyNowStepChange = false;
            pi.NowStep = 100;
            pi.TotalStep = 100;
            pi.DisplayText = "업데이트완료. 잠시 후 프로그램을 시작합니다.";
            worker.ReportProgress(0, pi);
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressInfo pi = new ProgressInfo();
            pi.OnlyNowStepChange = true;
            pi.NowStep = e.ProgressPercentage;
            worker.ReportProgress(0, pi);
        }

        private bool IsUpdateTarget(FileModel serverFile)
        {
            //1. 파일이 있는지 확인
            if(File.Exists(RootPath + serverFile.Name) == false)
            {
                return true;
            }

            FileInfo fifo = new FileInfo(RootPath + serverFile.Name);

            //2. 버전이 있는 파일인지 확인 후 버전 비교
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fifo.FullName);
            if(versionInfo != null)
            {
                if (string.Compare(serverFile.Version, versionInfo.FileVersion) == 1)
                    return true;
            }
            //3. 파일 크기를 확인
            if(fifo.Length != serverFile.Size)
            {
                return true;
            }

            //4. 파일의 MD5 해시값 확인
            string hashString = "";
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fifo.FullName))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
            if (hashString != serverFile.MD5)
            {
                return true;
            }

            return false;
        }

        private List<FileModel> GetServerFileList()
        {
            List<FileModel> result = new List<FileModel>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CDN_URL + "/update.json");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream dataStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(dataStream))
            {
                string htmlContent = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<List<FileModel>>(htmlContent);
            }
            return result;
        }

        private void FrmLogin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Minimized;
        }

        private void FrmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                Location = new Point(Left - (_MousePoint.X - e.X), Top - (_MousePoint.Y - e.Y));
        }

        private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _MousePoint = new Point(e.X, e.Y);
        }
    }
}
