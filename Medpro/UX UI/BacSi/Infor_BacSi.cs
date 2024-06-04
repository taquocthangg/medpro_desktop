using DevExpress.XtraEditors;
using Login.Component;
using Login.UX_UI.BenhVien;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.UX_UI.BacSi
{
    public partial class Infor_BacSi : DevExpress.XtraEditors.XtraForm
    {
        public Infor_BacSi()
        {
            InitializeComponent();
        }

        private async void Infor_BacSi_Load(object sender, EventArgs e)
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

        private void btn_update_BacSi_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = true;
            guna2Panel1.BringToFront();
            Admins admin = new Admins();
            Update_BacSi update_Bacsi = new Update_BacSi();
            FormUtility.OpenChildForm(admin, update_Bacsi, guna2Panel1);
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}