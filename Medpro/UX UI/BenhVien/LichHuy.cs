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
using static Login.Component.EncodeToken;
using static Login.UX_UI.BenhVien.AllBenhAn;

namespace Login.UX_UI.BenhVien
{
    public partial class LichHuy : Form
    {
        private Loadding loadingControl;
        private readonly HttpClient _httpClient = new HttpClient();
        private const double ColumnWidthPercentage = 0.33; // Phần trăm chiều rộng cho mỗi cột


        public LichHuy()
        {
            InitializeComponent();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }
        private void XacNhanKham_SizeChanged(object sender, EventArgs e)
        {
            // Khi kích thước của listView thay đổi, cập nhật chiều rộng của các cột
            UpdateColumnWidths();
        }
        private void UpdateColumnWidths()
        {
            int totalWidth = listViewChuyenKhoa.ClientSize.Width;

            // Tính toán chiều rộng mới cho mỗi cột dựa trên phần trăm
            int newColumnWidth = (int)(totalWidth * ColumnWidthPercentage);

            // Gán giá trị mới cho chiều rộng của từng cột
            columnHeader1.Width = newColumnWidth;
            columnHeader2.Width = newColumnWidth;
            columnHeader3.Width = newColumnWidth;
        }
        private async void LichHuy_Load(object sender, EventArgs e)
        {
            loadingControl.StartLoading();
            // Gọi API để lấy dữ liệu về
            string id_benhVien = AuthManager.CurrentUser.id;
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-da-huy/" + id_benhVien;
            string jsonResponse = await _httpClient.GetStringAsync(apiUrl);
            var data = JsonConvert.DeserializeObject<Application>(jsonResponse);
            listViewChuyenKhoa.Items.Clear();

            foreach (var schedule in data.Data)
            {
                string[] row = {
                        string.Empty,
                        schedule.timeSlot,
                        schedule.activateDay,
                     };
                var patientInfo = data.Inforpatient.FirstOrDefault(p => p.id == schedule.patientId);

                if (patientInfo != null)
                {
                    row[0] = patientInfo.name;
                }

                ListViewItem item = new ListViewItem(row);
                // Thêm dữ liệu vào ListView
                item.Tag = schedule.id; // Lưu ID vào Tag
                listViewChuyenKhoa.Items.Add(item);
                loadingControl.HideLoading();
            }
        }
        public class User
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sdt { get; set; }
            public string diaChi { get; set; }
            public string namSinh { get; set; }
            public string gioiTinh { get; set; }

        }
        public class Sescription
        {
            public string id_benhvien { get; set; }
            public string name { get; set; }

        }
        public class Data
        {
            public string id { get; set; }

            public string patientId { get; set; }
            public string doctorId { get; set; }
            public string specialtyId { get; set; }
            public string hospitalId { get; set; }
            public string timeSlot { get; set; }
            public string activateDay { get; set; }
            public string status { get; set; }
            public string appointmentDate { get; set; }
            public string diagnosis { get; set; }
            public string medication { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public User User { get; set; }
            public Sescription Sescription { get; set; }
            public InforChuyenKhoa[] InforChuyenKhoa { get; set; }

        }
        public class Inforpatient
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sdt { get; set; }
            public string gioiTinh { get; set; }
            public string diaChi { get; set; }
        }
        public class InforBacSi
        {
            public string name { get; set; }
            public string sdt { get; set; }
            public string diaChi { get; set; }
        }
        public class InforChuyenKhoa
        {
            public string name { get; set; }
        }
        public class Application
        {
            public int err { get; set; }
            public string mess { get; set; }
            public Data[] Data { get; set; }
            public Inforpatient[] Inforpatient { get; set; }

        }


        public class ApiRes
        {
            public int err { get; set; }
            public string mess { get; set; }
            public Data[] Data { get; set; }
            public InforBenhVien[] InforBenhVien { get; set; }
            public IList<InforChuyenKhoa> InforChuyenKhoa { get; set; }

        }
    }
}
