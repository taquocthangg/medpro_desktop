using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using static Login.Component.EncodeToken;

namespace Login.UX_UI
{
    public partial class Update_Admin : DevExpress.XtraEditors.XtraForm
    {
        private string selectedImagePath;
        private Loadding loadingControl;
        public Update_Admin()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        public void btn_upload_img_Click(object sender, EventArgs e)
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

        private void Update_Admin_Load(object sender, EventArgs e)
        {
            txt_gioiTinh.SelectedIndex = 0;
        }

        private async void btn_update_Admin_Click(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            string name = txt_userName.Text;
            string newEmail = txt_Email.Text;
            string gioiTinh = txt_gioiTinh.SelectedItem.ToString();
            string sdt = txt_Sdt.Text;
            string diaChi = txt_diaChi.Text;
            MultipartFormDataContent formData = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(selectedImagePath);
                ByteArrayContent image = new ByteArrayContent(imageBytes);
                formData.Add(image, "image", Path.GetFileName(selectedImagePath));
            }

            // Đính kèm email và name
            formData.Add(new StringContent(newEmail), "newEmail");
            formData.Add(new StringContent(name), "name");
            formData.Add(new StringContent(gioiTinh), "gioiTinh");
            formData.Add(new StringContent(sdt), "sdt");
            formData.Add(new StringContent(diaChi), "diaChi");

            //var loginData = new { name, newEmail, image };
            string userId = AuthManager.CurrentUser.id;

            string apiReg = "https://medprov2.onrender.com/api/v1/auth/updateUser/" + userId;
            MessageBox.Show(apiReg);
            //loadingControl.StartLoading();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiReg, formData);
                    loadingControl.HideLoading();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Cập nhập thành công vui lòng đăng nhập lại!!!");
                        Application.Restart();
                        new Auth().Show();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhập thất bại, Vui lòng thử lại");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}