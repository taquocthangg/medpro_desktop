using DevExpress.XtraEditors;
using Login.UX_UI.Admin;
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
using static Login.Component.EncodeToken;

namespace Login.UX_UI.BenhVien
{
    public partial class Danh_Sach_Chuyen_Khoa : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;
        private readonly HttpClient _httpClient = new HttpClient();
        public Danh_Sach_Chuyen_Khoa()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }

        private async void Danh_Sach_Chuyen_Khoa_Load(object sender, EventArgs e)
        {
                loadingControl.StartLoading();
            // Gọi API để lấy dữ liệu về
            string id_benhVien = AuthManager.CurrentUser.id;
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/chuyenkhoa/"+ id_benhVien;
            string jsonResponse = await _httpClient.GetStringAsync(apiUrl);
            var data = JsonConvert.DeserializeObject<ApiData>(jsonResponse);
            listViewChuyenKhoa.Items.Clear();

            foreach (var user in data.ChuyenKhoa)
            {
                // Tạo một ListViewItem với tên của bệnh viện
                var item = new ListViewItem(user.Name);
                item.SubItems.Add(user.Description.ToString());
                item.SubItems.Add(user.Price.ToString());
                // Thêm ListViewItem vào ListView
                listViewChuyenKhoa.Items.Add(item);
                loadingControl.HideLoading();
            }
        }
        public class ApiData
        {
            public string Message { get; set; }
            public int Count { get; set; }
            public ChuyenKhoa[] ChuyenKhoa { get; set; }
        }

        public class ChuyenKhoa
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
        }
        private void listViewChuyenKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}