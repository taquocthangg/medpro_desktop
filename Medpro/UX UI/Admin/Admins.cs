using DevExpress.XtraEditors;
using Login.Component;
using Login.UX_UI.Admin;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace Login.UX_UI
{
    public partial class Admins : DevExpress.XtraEditors.XtraForm 
    {

        private Loadding loadingControl;
        public Admins()
        {
            InitializeComponent();
            hideSubMenu();
            // Khởi tạo LoadingControl và thêm nó vào Form
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
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
           // hideSubMenu();
        }
        //Dong menu
        
        private async void Admin_Load(object sender, EventArgs e)
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
            catch(Exception ex) { messBox.Show("Lỗi : " +ex); }
            finally { loadingControl.HideLoading(); }
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

        private void btn_BenhVien_Click(object sender, EventArgs e)
        {
            showSubMenu(panel_BenhVien_Sub);
            CloseMenu();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Info_Admin());
        }

        private void toolboxControl1_Click(object sender, EventArgs e)
        {

        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.Close();
            Auth auth = new Auth();
            auth.Show();
        }

        private void add_benhVien_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Add_BenhVien(this));
        }

        private void Admins_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Danh_Sach_Benh_Vien());
        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng sắp ra mắt vui lòng thử lại sau :<< ");
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng sắp ra mắt vui lòng thử lại sau :<< ");
        }

        private void iconButton10_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng sắp ra mắt vui lòng thử lại sau :<< ");

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            messBox.Show("Chức năng sắp ra mắt vui lòng thử lại sau :<< ");

        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new AllUser());

        }

        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Add_BenhVien());
        }

        private void guna2GradientPanel2_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new Add_BenhVien());

        }

        private void guna2GradientPanel3_Click(object sender, EventArgs e)
        {
            openChildFormInPanel(new AllUser());

        }
    }
}