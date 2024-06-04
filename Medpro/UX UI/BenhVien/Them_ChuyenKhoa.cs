using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using Guna.UI2.WinForms.Suite;
using static Login.Component.EncodeToken;

namespace Login.UX_UI.BenhVien
{
    public partial class Them_ChuyenKhoa : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;

        public Them_ChuyenKhoa()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false;
        }
        private void clearForm()
        {
            txt_Name.Text = "";
            txt_description.Text = "";
            txt_price.Text = "";
        }
        private async void btn_update_Admin_Click(object sender, EventArgs e)
        {
            string name = txt_Name.Text;
            string description = txt_description.Text;
            string price = txt_price.Text;
            string id_benhVien = AuthManager.CurrentUser.id;
            string apiReg = "https://medprov2.onrender.com/api/v1/auth/themchuyenkhoa/" + id_benhVien;
            var loginData = new { name, description , price };
            loadingControl.StartLoading();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var jsonContent = new StringContent(
                            JsonConvert.SerializeObject(loginData),
                            Encoding.UTF8,
                            "application/json");
                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiReg, jsonContent);
                    loadingControl.HideLoading();
                    // Kiểm tra phản hồi từ API

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đã thêm chuyên khoa: "+name);
                        clearForm();
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại vui lòng kiểm tra lại thông tin");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }

        private void Them_ChuyenKhoa_Load(object sender, EventArgs e)
        {

        }
    }
}