using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Net.Http;
using System.IO;
using System.Windows.Forms;
using static Login.Component.EncodeToken;
using DevExpress.XtraEditors.Mask;

namespace Login.UX_UI.User
{
    public partial class Update_User : DevExpress.XtraEditors.XtraForm
    {
        private string selectedImagePath;
        private Loadding loadingControl;
        private Form parentForm;

        public Update_User()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }
        public Update_User(Form parent) : this()
        {
            parentForm = parent;
            messBox.Parent = parentForm;
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

            // Tạo MultipartFormDataContent để chứa dữ liệu form
            MultipartFormDataContent formData = new MultipartFormDataContent();

            // Hàm thêm field vào formData nếu giá trị không rỗng
            void AddField(string key, string value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    formData.Add(new StringContent(value), key);
                }
            }

            // Thêm ảnh nếu có
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(selectedImagePath);
                formData.Add(new ByteArrayContent(imageBytes), "image", Path.GetFileName(selectedImagePath));
            }
            // Thêm các field dựa trên điều kiện không rỗng
            AddField("name", txt_userName.Text);
            AddField("newEmail", txt_Email.Text);
            AddField("namSinh", txt_namSinh.Text);
            AddField("gioiTinh", txt_gioiTinh.SelectedItem?.ToString());
            AddField("sdt", txt_Sdt.Text);
            AddField("diaChi", txt_diaChi.Text);

            string userId = AuthManager.CurrentUser.id;
            string apiReg = "https://medprov2.onrender.com/api/v1/auth/updateUser/" + userId;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiReg, formData);
                    

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Cập nhập thành công vui lòng đăng nhập lại!!!");
                        Application.Restart();
                        new Auth().Show();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhập thông tin thất bại vui lòng kiểm tra lại!!!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
                finally
                {
                    loadingControl.HideLoading();
                }
            }
        }

        private void Update_User_Load(object sender, EventArgs e)
        {

        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}