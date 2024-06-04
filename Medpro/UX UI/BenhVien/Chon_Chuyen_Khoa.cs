using DevExpress.XtraEditors;
using Login.Component;
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
using static Login.Component.getIdController;

namespace Login.UX_UI.BenhVien
{
    public partial class Chon_Chuyen_Khoa : DevExpress.XtraEditors.XtraForm
    {
        private Loadding loadingControl;
        private readonly HttpClient _httpClient = new HttpClient();
        public Chon_Chuyen_Khoa()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }
        private Form activeForm = null;
        private void openChildFormInPanel(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            listViewChuyenKhoa.Controls.Add(childForm);
            listViewChuyenKhoa.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private async void Add_BacSi_Load(object sender, EventArgs e)
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
                var item = new ListViewItem(user.Id);
                item.SubItems.Add(user.Name.ToString());
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
            public string Id { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
        }

        private void listViewChuyenKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            
        }

        private void listViewChuyenKhoa_Click(object sender, EventArgs e)
        {
            if (listViewChuyenKhoa.SelectedItems.Count > 0)
            {
                // Lấy mục được chọn
                ListViewItem selectedItem = listViewChuyenKhoa.SelectedItems[0];

                // Lấy ID từ subitem đầu tiên (subitem thứ 0)
                string id = selectedItem.SubItems[0].Text;
                TemporaryDataManager.SelectedId = id;
            }
            openChildFormInPanel(new Add_Bac_Si());
        }
    }
}