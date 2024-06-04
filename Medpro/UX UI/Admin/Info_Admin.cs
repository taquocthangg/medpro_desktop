using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using Login.Component;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Login.Component.EncodeToken;

namespace Login.UX_UI
{
    public partial class Info_Admin : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;
        public Info_Admin()
        {
            InitializeComponent();
            // Khởi tạo LoadingControl và thêm nó vào Form
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_update_Admin_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = true;
            guna2Panel1.BringToFront();
            Admins admin = new Admins();
            Update_Admin update_Admin = new Update_Admin();
            FormUtility.OpenChildForm(admin, update_Admin, guna2Panel1);
        }

        private async void Info_Admin_Load(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            var apiService = new ApiService();

            var userDataResponse = await apiService.GetUserDataAsync();

            if (userDataResponse != null)
            {
                loadingControl.HideLoading();
                var userData = userDataResponse.User;
                txt_Admin_name.Text = userData.Name;
                txt_email.Text = userData.Email;
                txt_numberPhone.Text = userData.Sdt;
                txt_gioiTinh.Text = userData.GioiTinh;
                //txt_diaChi.Text = userData.DiaChi;
                // Kiểm tra và hiện thị ảnh
                Image userAvatar;
                if (ApiService.TryDownloadImage(userData.Avatar, out userAvatar))
                {
                    img_avatar.Image = userAvatar;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng thử đăng nhập lại");
                Auth auth = new Auth();
                this.Close();
                auth.Show(); 
            }
        }

        private void btn_back_Click_1(object sender, EventArgs e)
        {
            this.Close();
            //new Admins().Show();
        }
    }
}