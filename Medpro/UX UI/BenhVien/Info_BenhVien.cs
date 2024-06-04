using DevExpress.XtraEditors;
using Login.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.UX_UI.BenhVien
{
    public partial class Info_BenhVien : DevExpress.XtraEditors.XtraForm
    {
        public Info_BenhVien()
        {
            InitializeComponent();
        }

        private async void Info_BenhVien_Load(object sender, EventArgs e)
        {
            var apiService = new ApiService();
            var userData = (await apiService.GetUserDataAsync())?.User;

            if (userData != null)
            {
                txt_Admin_name.Text = userData?.Name;
                txt_email.Text = userData?.Email;
                txt_numberPhone.Text = userData?.Sdt;
                txt_diaChi.Text = userData?.DiaChi;
                if (ApiService.TryDownloadImage(userData?.Avatar, out var userAvatar))
                {
                    img_avatar.Image = userAvatar;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thử đăng nhập lại");
            }
        }

        private void btn_update_Admin_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = true;
            guna2Panel1.BringToFront();
            Admins admin = new Admins();
            Update_BenhVien update_BenhVien = new Update_BenhVien();
            FormUtility.OpenChildForm(admin, update_BenhVien, guna2Panel1);
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}