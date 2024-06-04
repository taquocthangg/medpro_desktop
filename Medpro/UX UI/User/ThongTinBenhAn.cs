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
using System.Windows;
using System.Windows.Forms;
using static Login.Component.EncodeToken;

namespace Login.UX_UI.User
{
    public partial class ThongTinBenhAn : Form
    {
        private ApiRes apiResults;
        private const double ColumnWidthPercentage = 0.33; // Phần trăm chiều rộng cho mỗi cột
        private Form parentForm;
        private Loadding loadingControl;

        public ThongTinBenhAn()
        {
            InitializeComponent();
            clearForm();
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            listView1.Controls.Add(loadingControl);
            loadingControl.Visible = false;
        }
        public ThongTinBenhAn(Form parent) : this()
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
        private async void ThongTinBenhAn_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string userId = AuthManager.CurrentUser.id;

            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-da-dat-by-id-benhnhan/" + userId;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    apiResults = JsonConvert.DeserializeObject<ApiRes>(jsonResult);

                    foreach (var schedule in apiResults.Data)
                    {
                    
                        string[] row = {
                              string.Empty,
                              schedule.timeSlot,
                            schedule.activateDay,
                        };
                        foreach (var info in apiResults.InforChuyenKhoa)
                        {
                            // Lấy giá trị cần thiết từ info và gán vào row[2]
                            row[0] = info.name;

                            // Thêm dữ liệu vào ListView
                            ListViewItem item = new ListViewItem(row);
                        item.Tag = schedule.id; // Lưu ID vào Tag
                            listView1.Items.Add(item);
                        }
                      //  ListViewItem item = new ListViewItem(row);
                        //listView1.Items.Add(item);
                        //   var item = new ListViewItem(apiResults.User.name);
                        //  item.SubItems.Add(user.Price.ToString());
                        // Thêm ListViewItem vào ListView
                       // listView1.Items.Add(item);
                    }

                }

                else
                {
                    // Xử lý lỗi nếu có
                    messBox.Show($"Error: {response.StatusCode}");
                }
            }
        }
        private async void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Lấy thông tin về hàng được chọn
                ListViewItem selectedItem = listView1.GetItemAt(e.X, e.Y);

                if (selectedItem != null)
                {
                    // Lấy ID từ thuộc tính Tag của ListViewItem
                    string id = selectedItem.Tag.ToString();

                    // Tạo ContextMenuStrip
                    ContextMenuStrip contextMenu = new ContextMenuStrip();

                    // Tạo ToolStripMenuItem cho nút Hủy
                    ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem("Hủy lịch khám");

                    // Gắn sự kiện cho nút Hủy
                    deleteMenuItem.Click += async (deleteSender, deleteEventArgs) =>
                    {
                        // Thực hiện xóa với ID đã lấy được
                        if (System.Windows.Forms.MessageBox.Show("Bạn có chắc chắn muốn hủy lịch khám?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            // Nếu người dùng ấn "OK", thực hiện xóa
                             await  XoaItem(id);
                             ThongTinBenhAn_Load(sender,e);
                        }
                    };
                    contextMenu.Items.Add(deleteMenuItem);

                    // Hiển thị ContextMenuStrip tại vị trí chuột
                    contextMenu.Show(listView1, e.Location);
                }
            }
        }


        private async Task XoaItem(string id)
        {
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/huylichkham/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    messBox.Show("Hủy lịch thành công");

                }

                else
                {
                    messBox.Show($"Error: {response.StatusCode}");
                }
            }
        }


        private async void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string userId = AuthManager.CurrentUser.id;
                string selectedId = listView1.SelectedItems[0].Tag.ToString();

                //string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-da-dat-by-id-benhnhan/" + selectedId;
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lichkhamdadat/" + selectedId;

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
                            foreach (var item in data.InforBacSi)
                             {
                            labelPatientName.Text = "Tên bác sĩ: " + item.name;
                            txt_gioiTinh.Text = "Giới Tính: " + item.gioiTinh;
                             txt_sdt.Text = "Số Điện Thoại: " + item.sdt;
                              txt_namSinh.Text = "Địa chỉ: " + item.diaChi;
                            }
                            txtNgayKham.Text = "Thời gian khám: " + data.Data.timeSlot;
                            txt_kqKham.Text = "Trạng Thái : " + data.Data.status;
                            txt_thongtin.Text = "Thông tin phiếu khám";
                            foreach (var schedule in data.InforBenhVien)
                           {
                           txt_TenBenhVien.Text = "Tên bệnh viện: " + schedule.name;
                             txt_sdt_benhVien.Text = "Hotline: " + schedule.sdt;
                            txt_diaChi.Text = "Địa Chỉ: " + schedule.diaChi;
                            }
                            foreach (var schedule in data.InforChuyenKhoa)
                             {
                            txtTime.Text = "Chuyên khoa: " + schedule.name;

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
            txt_gioiTinh.Text = "";
            txt_sdt.Text = "";
            txt_namSinh.Text = "";
            txtNgayKham.Text = "";
            txt_thongtin.Text = "";
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
            public string gioiTinh { get; set; }
        }
        public class InforChuyenKhoa
        {
            public string name { get; set; }
        }
        public class Application
        {
            public int err { get; set; }
            public string mess { get; set; }
            public Data Data { get; set; }
            public User[] User { get; set; }
            public IList<InforBenhVien> InforBenhVien { get; set; }
            public IList<InforBacSi> InforBacSi { get; set; }
            public IList<InforChuyenKhoa> InforChuyenKhoa { get; set; }

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
