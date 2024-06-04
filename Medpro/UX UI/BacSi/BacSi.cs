using DevExpress.XtraEditors;
using Login.Component;
using Login.UX_UI.Admin;
using Login.UX_UI.BenhVien;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Login.UX_UI.BacSi
{
    public partial class BacSi : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;

        public BacSi()
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
        private async void BacSi_Load(object sender, EventArgs e)
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

        private void BacSi_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void btn_BenhVien_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_BenhVien_Sub);
            CloseMenu();
        }

        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_KhachHang_sub);
            CloseMenu();
        }

        private void btn_ThongKe_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_TinTuc_sub);
            CloseMenu();
        }


        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            new Auth().Show();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Infor_BacSi());
        }

        private void add_benhVien_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new ThemLichKham(this));

        }

        private void btn_chuyenKhoa_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new XacNhanKham(this));
        }

        private void btn_add_BacSi_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new LichSuKham(this));
        }

        private void name_Admin_Click(object sender, EventArgs e)
        {

        }

        private void iconButton10_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng đang cập nhập vui lòng thử lại sau !!!");
        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng đang cập nhập vui lòng thử lại sau !!!");
        }

        private void panel_KhachHang_sub_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new LichHuy());

        }

        private void guna2CirclePictureBox7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new LichSuKham());
        }

        private void guna2GradientPanel3_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new LichSuKham());
        }
    }
}