using DevExpress.XtraEditors;
using Login.Component;
using Login.UX_UI.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.UX_UI
{
    public partial class FormUser : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;

        public FormUser()
        {
            InitializeComponent();
            hideSubMenu();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false;

        }
        private Form activeForm = null;

        private void openChildFormInPanel(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void hideSubMenu()
        {
            panel_BenhVien_Sub.Visible = false;
            panel_KhachHang_sub.Visible = false;
            panel_TinTuc_sub.Visible = false;
        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200)
            {
                panelMenu.Width = 100;
                btn_info.Visible = false;
                name_Admin.Visible = false;
                img_avatar.Dock = DockStyle.Top;
                btn_Menu.Dock = DockStyle.Top;
                panel_info.Size = new Size(280, 150);
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            //Mo menu
            else
            {
                panelMenu.Width = 280;
                btn_info.Visible = true;
                name_Admin.Visible = true;
                btn_Menu.Dock = DockStyle.None;
                img_avatar.Dock = DockStyle.None;
                panel_info.Size = new Size(280, 210);
                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10, 0, 0, 0);
                }
            }
        }
        private void CloseMenu()
        {
            panelMenu.Width = 280;
            btn_info.Visible = true;
            btn_Menu.Dock = DockStyle.None;
            img_avatar.Dock = DockStyle.None;
            panel_info.Size = new Size(280, 210);
            foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
            {
                menuButton.Text = "   " + menuButton.Tag.ToString();
                menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                menuButton.Padding = new Padding(10, 0, 0, 0);
            }
        }
        private void btn_Menu_Click(object sender, EventArgs e)
        {
            CollapseMenu();
            hideSubMenu();
        }
        private async void Users_Load(object sender, EventArgs e)
        {
            try
            {
                loadingControl.StartLoading();
                var apiService = new ApiService();
                bool checkGetCurrent = await apiService.getCurent(name_Admin, img_avatar);
                if (!checkGetCurrent)
                {
                    Close();
                    new Auth().Show();
                }
            }
            catch (Exception ex) { messBox.Show("Lỗi : " +ex); }
            finally { loadingControl.HideLoading(); }
        }
        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_BenhVien_Sub);
            CloseMenu();
        }
        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            Auth auth = new Auth();
            auth.Show();
        }
        private void Users_FormClosing(object sender, FormClosingEventArgs e)
        {
           // Application.Exit();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            Info_User info_User = new Info_User();
            openChildFormInPanel(info_User);
        }

        private void btn_KhachHang_Click_1(object sender, EventArgs e)
        {
            showSubMenu(panel_KhachHang_sub);
            CloseMenu();
        }

        private void btn_TinTuc_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_TinTuc_sub);
            CloseMenu();
        }

        private void guna2HtmlLabel5_Click(object sender, EventArgs e)
        {

        }

        private void guna2CirclePictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel17_Click(object sender, EventArgs e)
        {

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new DatKhamTheoBenhVien(this));
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new DatKhamTheoBenhVien(this));
        }

        private void panelChildForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new BenhAn(this));
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new ThongTinBenhAn(this));
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng sắp ra mắt vui lòng thử lại sau !!!");
        }

        private void iconButton4_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Auth auth = new Auth();
            auth.Show();
        }
    }
}