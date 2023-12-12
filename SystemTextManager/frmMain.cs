using DevExpress.Xpo.DB.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraLayout.Adapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemTextManager.Core;
using SystemTextManager.Database.Model;

namespace SystemTextManager
{
    public partial class frmMain : frmRoot
    {
        List<SearchModel> db = new List<SearchModel>();
        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            gridControl1.DataSource = db;
            
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("데이터를 조회 하고 있습니다.");
            splashScreenManager1.SetWaitFormDescription("터널링 후 조회 됩니다. 잠시만 기다려주세요..");
            string condition = "";

            if (string.IsNullOrEmpty(txtSearchSite.Text) == false)
                condition += "AND ( en.`site` LIKE '%" + txtSearchSite.Text.Trim() + "%' OR ko.`site` LIKE '%" + txtSearchSite.Text.Trim() + "%' )";
            if (string.IsNullOrEmpty(txtSearchPage.Text) == false)
                condition += "AND ( en.`page` LIKE '%" + txtSearchPage.Text.Trim() + "%' OR ko.`page` LIKE '%" + txtSearchPage.Text.Trim() + "%' )";
            if (string.IsNullOrEmpty(txtSearchKey.Text) == false)
                condition += "AND ( en.`key` LIKE '%" + txtSearchKey.Text.Trim() + "%' OR ko.`key` LIKE '%" + txtSearchKey.Text.Trim() + "%' )";
            if (string.IsNullOrEmpty(txtSearchText.Text) == false)
                condition += "AND ( en.`text` LIKE '%" + txtSearchText.Text.Trim() + "%' OR ko.`text` LIKE '%" + txtSearchText.Text.Trim() + "%' )";
            if (string.IsNullOrEmpty(condition))
            {
                XtraMessageBox.Show("검색 값은 하나 이상 입력해야 합니다.");
                return;
            }
            string queryFat = @"
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
                    1=1 
	                {0};
            ";
            string query = string.Format(queryFat, condition);

            if (rgType.SelectedIndex == 0) db = DbHelper.Instance.GetResult(DbTypes.PRD, query);
            else if (rgType.SelectedIndex == 1) db = DbHelper.Instance.GetResult(DbTypes.DEV, query);
            else if (rgType.SelectedIndex == 2) db = DbHelper.Instance.GetResult(DbTypes.LOCAL, query);
            gridControl1.DataSource = db;
            gridControl1.RefreshDataSource();
            labelControl1.Text = string.Format("총 {0:#,#}건 검색됨.", db.Count);
            splashScreenManager1.CloseWaitForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (chkDev.Checked == false && chkPrd.Checked == false && chkLocal.Checked == false)
            {
                XtraMessageBox.Show("저장 대상이 선택되지 않았습니다.");
                return;
            }
            if (string.IsNullOrEmpty(txtSite.Text))
            {
                XtraMessageBox.Show("Site 항목은 필수값 입니다.");
                txtSite.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPage.Text))
            {
                XtraMessageBox.Show("Page 항목은 필수값 입니다.");
                txtPage.Focus(); 
                return;
            }
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                XtraMessageBox.Show("Key 항목은 필수값 입니다.");
                txtKey.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtEn.Text))
            {
                XtraMessageBox.Show("영문 항목은 필수값 입니다.");
                txtEn.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtKo.Text))
            {
                XtraMessageBox.Show("국문 항목은 필수값 입니다.");
                txtKo.Focus();
                return;
            }

            SearchModel saveModel = new SearchModel();
            saveModel.Site = txtSite.Text;
            saveModel.Page = txtPage.Text;
            saveModel.Key = txtKey.Text;    
            saveModel.En = txtEn.Text;  
            saveModel.Ko = txtKo.Text;  

            splashScreenManager1.ShowWaitForm();

            SearchModel exists = null;
            if (chkPrd.Checked)
            {
                splashScreenManager1.SetWaitFormCaption("운영서버 데이터 저장");
                splashScreenManager1.SetWaitFormDescription("터널링 후 업뎃 됩니다. 잠시만 기다려주세요..");
                exists = DbHelper.Instance.InsertOrUpdate(DbTypes.PRD, saveModel);
                if (exists != null)
                {
                    splashScreenManager1.CloseWaitForm();
                    if(UpdateForceConfirm(exists, saveModel))
                        DbHelper.Instance.InsertOrUpdate(DbTypes.PRD, saveModel, true);
                }
            }
            if (chkDev.Checked)
            {
                splashScreenManager1.SetWaitFormCaption("개발서버 데이터 저장");
                splashScreenManager1.SetWaitFormDescription("터널링 후 업뎃 됩니다. 잠시만 기다려주세요..");
                exists = DbHelper.Instance.InsertOrUpdate(DbTypes.DEV, saveModel);
                if (exists != null)
                {
                    splashScreenManager1.CloseWaitForm();
                    if(UpdateForceConfirm(exists, saveModel))
                        DbHelper.Instance.InsertOrUpdate(DbTypes.DEV, saveModel, true);
                }
            }
            if (chkLocal.Checked)
            {
                splashScreenManager1.SetWaitFormCaption("로컬서버 데이터 저장");
                splashScreenManager1.SetWaitFormDescription("터널링 후 업뎃 됩니다. 잠시만 기다려주세요..");
                DbHelper.Instance.InsertOrUpdate(DbTypes.LOCAL, saveModel, true);
            }
            splashScreenManager1.CloseWaitForm();
            XtraMessageBox.Show("저장완료", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool UpdateForceConfirm(SearchModel existsModel, SearchModel saveModel)
        {
            bool result = false;
            frmUpdateForceConfirm frm = new frmUpdateForceConfirm(existsModel, saveModel);
            frm.StartPosition = FormStartPosition.CenterParent;
            if (frm.ShowDialog(this) == DialogResult.OK)
                result = true;
            splashScreenManager1.ShowWaitForm();
            return result;
        }

        private void grdView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
                SetSearchModel(db[e.FocusedRowHandle]);
        }

        private void SetSearchModel(SearchModel sm)
        {
            txtSite.Text = sm.Site;
            txtPage.Text = sm.Page;
            txtKey.Text = sm.Key;
            txtKo.Text = sm.Ko;
            txtEn.Text = sm.En;
            textEdit1.Text = "{{ $common_value['text']['" + sm.Page + "']['" + sm.Key + "'] }}";
        }

        private void txtSearchSite_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                txtSearchKey.Focus();
            }
        }

        private void txtSearchPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearchKey.Focus();
            }
        }

        private void txtSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSearchText.Focus();
            }
        }

        private void txtSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        private void txtSite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPage.Focus();
            }
        }

        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtKey.Focus();
            }
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtKo.Focus();  
            }
        }

        private void txtKo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEn.Focus();
            }
        }

        private void txtEn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(null, null);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textEdit1.Text);
        }

        private void grdView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = e.RowHandle.ToString();
            }
        }

        private void txtSite_Leave(object sender, EventArgs e)
        {
            textEdit1.Text = "{{ $common_value['text']['" + txtPage.Text.Trim() + "']['" + txtKey.Text.Trim() + "'] }}";
        }

    }
}
