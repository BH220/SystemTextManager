using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTextManager.Core.Helper;

namespace SystemTextManager
{
    public partial class frmSetting : frmRoot
    {
        public frmSetting()
        {
            InitializeComponent();
        }
         

        private void btnDev_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                if(string.IsNullOrEmpty(btnGitPem.Text) == false)
                    Process.Start(btnGitPem.Text);
            }
            else if (e.Button.Index == 1)
            {
                btnGitPem.Text = getFile();
            }
        }

        private string getFile()
        {
            string result = "";

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PEM KEY(*.pem)|*.pem";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    result = ofd.FileName;
                }
            }
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            btnGitPem.Text = SettingHelper.Instance.GitlabPem;
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingHelper.Instance.GitlabPem = btnGitPem.Text;
            base.OnClosing(e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
