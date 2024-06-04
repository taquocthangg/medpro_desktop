using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net.Http;
using System.Xml.Linq;
using static Login.Component.EncodeToken;
using Newtonsoft.Json.Linq;
using DevExpress.XtraEditors.Mask;

namespace Login.UX_UI.Admin
{
    public partial class Add_BenhVien : DevExpress.XtraEditors.XtraForm
    {
        private string selectedImagePath;
        private Loadding loadingControl;
        private Form parentForm;
        public Add_BenhVien()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }
        public Add_BenhVien(Form parent) : this()
        {
            parentForm = parent;
            messBox.Parent = parentForm;
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
                    messBox.Show("Vui lòng chọn một tệp hình ảnh hợp lệ.");
                }
            }
        }
        private void clearForm()
        {
            txt_userName.Text = "";
            txt_Email.Text = "";
            txt_password.Text = "";
            txt_Sdt.Text = "";
            txt_diaChi.Text = "";
        }
        private string Mess(string jsonResponse)
        {
            // Sử dụng JSON.NET để trích xuất mess từ chuỗi JSON
            JObject json = JObject.Parse(jsonResponse);
            string mess = json["mess"]?.ToString();

            return mess;
        }
        private async void btn_update_Admin_Click(object sender, EventArgs e)
        {
            string name = txt_userName.Text;
            string newEmail = txt_Email.Text;
            string sdt = txt_password.Text;
            string diaChi = txt_diaChi.Text;
            string password = txt_password.Text;
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
            formData.Add(new StringContent("R2"), "role_id");
            string apiReg = "https://medprov2.onrender.com/api/v1/auth/";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    loadingControl.StartLoading();
                    HttpResponseMessage response = await client.PostAsync(apiReg, formData);
                    string data = await response.Content.ReadAsStringAsync();

                    string mess = Mess(data);
                    if (response.IsSuccessStatusCode)
                    {
                        messBox.Show(mess);
                        clearForm();
                    }
                    else
                    {
                        messBox.Show("Đăng ký thất bại. Vui lòng kiểm tra lại thông tin đăng ký");
                    }
                }
                catch (Exception ex)
                {
                    messBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
                finally
                { loadingControl.HideLoading(); }    
            }
        }

        private void Add_BenhVien_Load(object sender, EventArgs e)
        {

        }
    }
}