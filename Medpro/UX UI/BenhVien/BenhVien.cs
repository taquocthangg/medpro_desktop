using DevExpress.XtraEditors;
using Login.Component;
using Login.UX_UI.Admin;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Login.UX_UI.BenhVien
{
    public partial class BenhVien : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;
        public BenhVien()
        {
            InitializeComponent();
            hideSubMenu();
            // Khởi tạo LoadingControl và thêm nó vào Form
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
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
        private async void BenhVien_Load(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            var apiService = new ApiService();
            bool checkGetCurrent = await apiService.getCurent(name_Admin, img_avatar);
            loadingControl.HideLoading();
            if (!checkGetCurrent)
            {
                Close();
                new Auth().Show();
            }
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Info_BenhVien());
        }

        private void btn_chuyenKhoa_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Danh_Sach_Chuyen_Khoa());
        }

        private void btn_add_BacSi_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Chon_Chuyen_Khoa());
        }

        private void add_benhVien_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Them_ChuyenKhoa());
        }

        private void btn_ChuyenKhoa_menu_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_BenhVien_Sub);
            CloseMenu();
        }

        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_KhachHang_sub);
            CloseMenu();
        }

        private void btn_TinTuc_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_TinTuc_sub);
            CloseMenu();
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            this.Close();
            new Auth().Show();
        }

        private void panelChildForm_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Them_ChuyenKhoa());
        }

        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Chon_Chuyen_Khoa());
        }

        private void guna2CirclePictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel3_Paint(object sender, PaintEventArgs e)
        {
            openChildFormInPanel(new Add_Bac_Si());
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new DanhsachBacSi());

        }

        private void iconButton10_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new AllBenhAn(this));

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng đang cập nhập vui lòng thử lại sau !!!!");

        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {
            openChildFormInPanel(new Them_ChuyenKhoa());
        }
    }
}