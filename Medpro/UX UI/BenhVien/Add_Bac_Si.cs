using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using static Login.Component.getIdController;

namespace Login.UX_UI.BenhVien
{
    public partial class Add_Bac_Si : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;
        private string selectedImagePath;

        public Add_Bac_Si()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        private void Add_Bac_Si_Load(object sender, EventArgs e)
        {
        }

        private async void btn_add_BacSi_Click(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            string name = txt_userName.Text;
            string newEmail = txt_Email.Text;
            string sdt = txt_password.Text;
            string diaChi = txt_diaChi.Text;
            string password = txt_password.Text;
            string gioiTinh = txt_gioiTinh.Text;
            string namSinh = txt_namSinh.Text;
            string id_chuyenKhoa = TemporaryDataManager.SelectedId;
            MultipartFormDataContent formData = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(selectedImagePath);
                ByteArrayContent image = new ByteArrayContent(imageBytes);
                formData.Add(image, "image", Path.GetFileName(selectedImagePath));
            }

            // Đính kèm email và name
            formData.Add(new StringContent(name), "name");
            formData.Add(new StringContent(newEmail), "email");
            formData.Add(new StringContent(password), "password");
            formData.Add(new StringContent(sdt), "sdt");
            formData.Add(new StringContent(diaChi), "diaChi");
            formData.Add(new StringContent(gioiTinh), "gioiTinh");
            formData.Add(new StringContent(namSinh), "namSinh");
            formData.Add(new StringContent("R3"), "role_id");
            string apiReg = "https://medprov2.onrender.com/api/v1/auth/thembacsi/" + id_chuyenKhoa;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiReg, formData);

                    loadingControl.HideLoading();
                    if (response.IsSuccessStatusCode)
                    {
                        string tokenResponse = await response.Content.ReadAsStringAsync();
                        string mess = Mess(tokenResponse);
                        MessageBox.Show(mess + name);
                    }
                    else
                    {
                        MessageBox.Show("Đăng ký thất bại. Vui lòng kiểm tra lại thông tin đăng ký");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }
        private string Mess(string jsonResponse)
        {
            // Sử dụng JSON.NET để trích xuất mess từ chuỗi JSON
            JObject json = JObject.Parse(jsonResponse);
            string mess = json["mess"]?.ToString();

            return mess;
        }
        private void btn_upload_avt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;

                // Kiểm tra loại tệp
                string extension = Path.GetExtension(selectedImagePath).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp")
                {
                    // Hiển thị hình ảnh trên PictureBox
                    img_avatar.Image = Image.FromFile(selectedImagePath);
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một tệp hình ảnh hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txt_Sdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}