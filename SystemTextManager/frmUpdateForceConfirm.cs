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
using SystemTextManager.Database.Model;

namespace SystemTextManager
{
    public partial class frmUpdateForceConfirm : frmRoot
    {
        SearchModel existsModel;
        SearchModel saveModel;
        public frmUpdateForceConfirm()
        {
            InitializeComponent();
        }

        public frmUpdateForceConfirm(SearchModel existsModel, SearchModel saveModel)
        {
            InitializeComponent();
            this.existsModel = existsModel;
            this.saveModel = saveModel;
        }

        protected override void OnLoad(EventArgs e)
        {
            textEdit01.Text = existsModel.Site;
            textEdit02.Text = existsModel.Page;
            textEdit03.Text = existsModel.Key;
            textEdit04.Text = existsModel.Ko;
            textEdit05.Text = saveModel.Ko;
            textEdit06.Text = existsModel.En;
            textEdit07.Text = saveModel.En;
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            simpleButton1.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("기존 정보를 변경하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
