using DevExpress.XtraEditors.Mask;
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

namespace Login.UX_UI.User
{
    public partial class BenhAn : Form
    {
        private ApiRes apiResults;
        private const double ColumnWidthPercentage = 0.33; // Phần trăm chiều rộng cho mỗi cột
        private Form parentForm;
        public BenhAn()
        {
            InitializeComponent();
            clearForm();

        }
        public BenhAn(Form parent) : this()
        {
            parentForm = parent;
            messBox.Parent = parentForm;
        }
        private void XacNhanKham_SizeChanged(object sender, EventArgs e)
        {
            // Khi kích thước của listView thay đổi, cập nhật chiều rộng của các cột
            UpdateColumnWidths();
        }
        private void UpdateColumnWidths()
        {
            int totalWidth = listView1.ClientSize.Width;

            // Tính toán chiều rộng mới cho mỗi cột dựa trên phần trăm
            int newColumnWidth = (int)(totalWidth * ColumnWidthPercentage);

            // Gán giá trị mới cho chiều rộng của từng cột
            columnHeader1.Width = newColumnWidth;
            columnHeader2.Width = newColumnWidth;
            columnHeader3.Width = newColumnWidth;
        }
        private async void BenhAn_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string userId = AuthManager.CurrentUser.id;

            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-hoan-thanh/" + userId;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        apiResults = JsonConvert.DeserializeObject<ApiRes>(jsonResult);

                        if (apiResults != null && apiResults.Data.Any())
                        {
                            foreach (var schedule in apiResults.Data)
                            {
                                string[] row = {
                        string.Empty,
                        string.Empty,
                        schedule.appointmentDate,
                    };

                                foreach (var info in apiResults.InforBenhVien)
                                {
                                    // Lấy giá trị cần thiết từ info và gán vào row[0]
                                    row[0] = info.name;

                                    // Thêm dữ liệu vào ListView
                                    // ListViewItem item = new ListViewItem(row);
                                    // item.Tag = schedule.id; // Lưu ID vào Tag
                                    // listView1.Items.Add(item);
                                }

                                foreach (var info in apiResults.InforChuyenKhoa)
                                {
                                    // Lấy giá trị cần thiết từ info và gán vào row[1]
                                    row[1] = info.name;

                                    // Thêm dữ liệu vào ListView
                                    ListViewItem item = new ListViewItem(row);
                                    item.Tag = schedule.id; // Lưu ID vào Tag
                                    listView1.Items.Add(item);
                                }
                            }
                        }
                        else
                        {
                            // Hiển thị thông báo nếu apiResults rỗng
                            messBox.Show("Bạn chưa có bệnh án nào ");
                        }
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
                        messBox.Show($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private async void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedId = listView1.SelectedItems[0].Tag.ToString();

                //string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-da-dat-by-id-benhnhan/" + selectedId;
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/laysulichkham/" + selectedId;

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        clearForm();
                        // Gửi yêu cầu GET đến API
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung JSON từ phản hồi
                            string jsonResponse = await response.Content.ReadAsStringAsync();

                            // Phân tích JSON thành đối tượng C#
                            Application data = JsonConvert.DeserializeObject<Application>(jsonResponse);
                             foreach (var item in data.Data)
                             {
                                labelPatientName.Text = "Tên Bệnh Nhân: " + item.User.name;
                                 txt_gioiTinh.Text = "Giới Tính: " + item.User.gioiTinh;
                                 txt_sdt.Text = "Số Điện Thoại: " + item.User.sdt;
                                  txt_namSinh.Text = "Chuẩn đoán: " + item.diagnosis;
                                txtNgayKham.Text = "Ngày khám: " + item.appointmentDate;
                                 txtTime.Text = "Thời gian khám: " + item.timeSlot + "H";
                                donThuoc.Text = "Đơn thuốc:" +item.medication;
                                // txt_ChuyenKhoa.Text = "Chuyên Khoa: " + item.Sescription.name;
                                txt_ChuyenKhoa.Text = "Chuyên Khoa: " + item.Sescription.name;

                            }
                            // txt_benhan.Text = data.Data.Sescription.name;
                            foreach (var schedule in data.InforBenhVien)
                            {
                            txt_TenBenhVien.Text = "Tên Bệnh Viện: " + schedule.name;
                              txt_sdt_benhVien.Text = "Hotline: " + schedule.sdt;
                             txt_diaChi.Text = "Địa Chỉ: " + schedule.diaChi;
                             }
                           
                            ShowForm();
                        }
                        else
                        {
                            messBox.Show("Không thể kết nối đến API");
                        }
                    }
                    catch (Exception ex)
                    {
                        messBox.Show(ex.Message);
                    }
                }
            }
        }
        public void clearForm()
        {
            labelPatientName.Text = "";
            txt_ChuyenKhoa.Text = "";
            txt_gioiTinh.Text = "";
            txt_sdt.Text = "";
            txt_namSinh.Text = "";
            txtNgayKham.Text = "";
            //txt_benhan.Text = "";
            txt_TenBenhVien.Text = "";
            txt_sdt_benhVien.Text = "";
            txt_diaChi.Text = "";
            txtTime.Text = "";
            separatorControl1.Visible = false;
        }
        public void ShowForm()
        {
            separatorControl1.Visible = true;
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
        public class InforBenhVien
        {
            public string name { get; set; }
            public string sdt { get; set; }
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
            public User[] User { get; set; }
            public IList<InforBenhVien> InforBenhVien { get; set; }
            public IList<InforBacSi> InforBacSi { get; set; }
            public InforChuyenKhoa InforChuyenKhoa { get; set; }

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
