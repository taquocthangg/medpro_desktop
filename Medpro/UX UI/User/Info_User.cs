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

namespace Login.UX_UI.User
{
    public partial class Info_User : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;

        public Info_User()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        private async void Info_User_Load(object sender, EventArgs e)
        {
            try
            {
                loadingControl.StartLoading();
                var apiService = new ApiService();
                var userData = (await apiService.GetUserDataAsync())?.User;

                if (userData != null)
                {

                    txt_email.Text = userData?.Email;
                    txt_Admin_name.Text = userData?.Name;
                    txt_namsinh.Text = userData?.NamSinh;
                    txt_numberPhone.Text = userData?.Sdt;
                    txt_diaChi.Text = userData?.DiaChi;
                    txt_gioiTinh.Text = userData?.GioiTinh;

                    if (ApiService.TryDownloadImage(userData.Avatar, out var userAvatar))
                    {
                        img_avatar.Image = userAvatar;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng thử đăng nhập lại");
                }
            }
            catch(Exception ex) { MessageBox.Show("Lỗi: " + ex); }
            finally { loadingControl.HideLoading(); }
        }

        private void btn_update_Admin_Click(object sender, EventArgs e)
        {
            guna2Panel1.Visible = true;
            guna2Panel1.BringToFront();
            Admins admin = new Admins();
            Update_User update_User = new Update_User(this);
            FormUtility.OpenChildForm(admin, update_User, guna2Panel1);
        }

        private void img_avatar_Click(object sender, EventArgs e)
        {

        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}