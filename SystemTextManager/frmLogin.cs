using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTextManager.Core.Helper;

namespace SystemTextManager
{
    public partial class frmLogin : frmRoot
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            checkEdit1.Checked = SettingHelper.Instance.SaveId;
            if(checkEdit1.Checked)
            {
                txtId.Text = SettingHelper.Instance.ID;
                txtPw.Focus();
            }
        }

        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPw.Focus();
            }
        }

        private void txtPw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoLogin();
            }
        }

        private void DoLogin()
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                XtraMessageBox.Show("아이디를 입력하세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtId.Focus();
                return;
            }
            else
                btnLogin_Click(null, null);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if(checkEdit1.Checked && string.IsNullOrEmpty(txtId.Text) == false)
            {
                txtPw.Focus();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingHelper.Instance.SaveId = checkEdit1.Checked;
            SettingHelper.Instance.ID = txtId.Text;
            //SettingHelper.Instance.SaveSetting();
            base.OnClosing(e);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(SettingHelper.Instance.GitlabPem))
            {
                XtraMessageBox.Show("설정 화면에서 pem key 설정을 진행해 주세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("사용자 정보 조회");
            splashScreenManager1.SetWaitFormDescription("잠시만 기다려 주세요");
            string msg = LoginHelper.Instance.DoLogin(txtId.Text, txtPw.Text);
            splashScreenManager1.CloseWaitForm();
            if (string.IsNullOrEmpty(msg))
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
                XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            using (frmSetting frm = new frmSetting())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {

                }
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
