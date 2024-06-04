using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login.UX_UI.BenhVien
{
    public partial class DanhsachBacSi : Form
    {
        private Loadding loadingControl;
        private readonly HttpClient _httpClient = new HttpClient();
        public DanhsachBacSi()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        private async void DanhsachBacSi_Load(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            listView1.LargeImageList = imageList1;
            // Gọi API để lấy dữ liệu về
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/getAllBacSi";
            string jsonResponse = await _httpClient.GetStringAsync(apiUrl);
            var data = JsonConvert.DeserializeObject<ApiData>(jsonResponse);
            listView1.Items.Clear();

            foreach (var user in data.Users)
            {
                // Tạo một ListViewItem với tên của bệnh viện
                var item = new ListViewItem(user.Name);

                // Đặt hình ảnh từ URL (nếu có)
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    // Tạo một key duy nhất cho hình ảnh bằng cách sử dụng ID của bệnh viện
                    var imageKey = user.Id.ToString();

                    // Đặt key cho ListViewItem
                    item.ImageKey = imageKey;

                    // Tải hình ảnh từ URL
                    var imageBytes = await _httpClient.GetByteArrayAsync(user.Avatar);

                    // Chuyển dữ liệu bytes thành đối tượng Image
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        var image = Image.FromStream(stream);
                        imageList1.Images.Add(imageKey, image);
                    }
                }
                else
                {
                    item.ImageKey = "default_avatar";
                }

                // Thêm ListViewItem vào ListView
                listView1.Items.Add(item);
                loadingControl.HideLoading();
            }
        }
        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string userId = selectedItem.ImageKey;
                MessageBox.Show("ID của bệnh viện là: " + userId);
            }
        }
        public class ApiData
        {
            public string Message { get; set; }
            public int Count { get; set; }
            public User[] Users { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
        }
    }
}
