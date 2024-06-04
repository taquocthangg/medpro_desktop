using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using Login.UX_UI.BenhVien;
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

namespace Login.UX_UI.BacSi
{
    public partial class XacNhanKham : DevExpress.XtraEditors.XtraForm
    {
        private const double ColumnWidthPercentage = 0.33; // Phần trăm chiều rộng cho mỗi cột
        private string activateDay = "";
        private Form parentForm;
        private YourDataModel data;
        public XacNhanKham()
        {
            InitializeComponent();
            guna2DateTimePicker1.MinDate = DateTime.Now;
        }

        public XacNhanKham(Form parent) : this()
        {
            parentForm = parent;
            guna2MessageDialog1.Parent = parentForm;
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
            columnHeader4.Width = newColumnWidth;
            columnHeader5.Width = newColumnWidth;
            columnHeader6.Width = newColumnWidth;
        }



        private async void guna2DateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value;
            activateDay = selectedDate.ToShortDateString();
            string doctorId = AuthManager.CurrentUser.id;
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lichDatKham/" + doctorId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Tạo đối tượng chứa dữ liệu cần gửi
                    var requestData = new { activateDay };

                    // Chuyển đối tượng thành JSON và tạo nội dung cho yêu cầu POST
                    string jsonData = JsonConvert.SerializeObject(requestData);
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Thực hiện yêu cầu POST
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();

                        // Kiểm tra xem chuỗi JSON có bằng "Không có lịch khám" hay không
                        if (jsonResult.Contains("Không có lịch khám"))
                        {
                            listView1.Items.Clear(); // Xóa các items cũ nếu có
                            guna2MessageDialog1.Show("Không có lịch khám mới được đặt !!!");
                            txt_mess.Visible = true;// Hiển thị label thông báo không có lịch
                        }
                        else
                        {
                            // Tiếp tục quá trình deserialization nếu không phải là chuỗi "Không có lịch khám"
                            DataScheduleBooked apiResult = JsonConvert.DeserializeObject<DataScheduleBooked>(jsonResult);

                            if (apiResult.err == 0)
                            {
                                // Xử lý dữ liệu nhận được từ API
                                listView1.Items.Clear(); // Xóa các items cũ nếu có

                                foreach (var schedule in apiResult.schedule)
                                {
                                    string[] row = {
                                    schedule.timeSlot,
                                    schedule.activateDay,
                                    schedule.Users.name
                                };

                                    ListViewItem item = new ListViewItem(row);
                                    item.Tag = schedule.id; // Lưu ID vào Tag
                                    listView1.Items.Add(item);
                                }

                                // Ẩn label nếu có lịch khám
                                txt_mess.Visible = false;
                                listView1.Visible = true;
                            }
                            else
                            {
                                MessageBox.Show(apiResult.mess, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve data from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }


        private async void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // Lấy ID của lịch khám từ ListViewItem được chọn
                string selectedId = listView1.SelectedItems[0].Tag.ToString();

                // Sử dụng ID của lịch khám theo nhu cầu của bạn
                //MessageBox.Show($"Bạn đã chọn lịch khám có ID: {selectedId}");
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lichkhamdadat/" + selectedId;
                MessageBox.Show(apiUrl);
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Gửi yêu cầu GET đến API
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // Đọc nội dung JSON từ phản hồi
                            string jsonResponse = await response.Content.ReadAsStringAsync();

                            // Phân tích JSON thành đối tượng C#
                            data = JsonConvert.DeserializeObject<YourDataModel>(jsonResponse);
                            // Hiển thị thông tin lên Label
                            //txt_ChuyenKhoa.Text = "Chuyên Khoa: " + data.Data.Sescription.name;
                            labelPatientName.Text = "Tên Bệnh Nhân: " + data.Data.User.name;
                            txt_gioiTinh.Text = "Giới Tính: " + data.Data.User.gioiTinh;
                            txt_sdt.Text = "Số Điện Thoại: " + data.Data.User.sdt;
                            txt_namSinh.Text = "Ngày sinh: " + data.Data.User.namSinh;
                            txtNgayKham.Text = "Ngày khám: " + data.Data.activateDay;
                            txtTime.Text = "Ngày khám: " + data.Data.timeSlot;
                            showFormPhieuKham();
                        }
                        else
                        {
                            MessageBox.Show("Không thể kết nối đến API");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void XacNhanKham_Load(object sender, EventArgs e)
        {
            clearFormPhieuKham();
        }

        private void clearFormPhieuKham()
        {
            txt_ChuyenKhoa.Text = "";
            labelPatientName.Text = "";
            txt_gioiTinh.Text = "";
            txt_sdt.Text = "";
            txt_namSinh.Text = "";
            txtNgayKham.Text = "";
            txtTime.Text = "";
            txt_kqKham.Text = "";
            txt_donThuoc.Text = "";
            txt_donThuoc.Text = "";
            txt_kq.Visible = false;
            btn_Done.Visible = false;
            r_donThuoc.Visible = false;
        }

        private void showFormPhieuKham()
        {
            txt_kq.Visible = true;
            btn_Done.Visible = true;
            txt_kqKham.Text = "Kết Quả Khám";
            txt_donThuoc.Text = "Đơn Thuốc";
            r_donThuoc.Visible = true;
        }




        private async void btn_Done_Click(object sender, EventArgs e)
        {
            string scheduleId = data.Data.id.ToString();
            string patientId = data.Data.User.id.ToString();
            string hospitalId = data.InforChuyenKhoa[0].id_benhVien.ToString();
            string specialtyId = data.InforChuyenKhoa[0].id.ToString();
            string timeSlot = data.Data.timeSlot;
            string appointmentDate = data.Data.activateDay;
            string diagnosis = txt_kq.Text;
            string medication = r_donThuoc.Text;
            string doctorId = AuthManager.CurrentUser.id;
            string apiReg = "https://medprov2.onrender.com/api/v1/auth/themsulichkham/" + scheduleId;
            var loginData = new { doctorId, patientId, hospitalId, specialtyId, timeSlot, appointmentDate, diagnosis, medication };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var jsonContent = new StringContent(
                            JsonConvert.SerializeObject(loginData),
                            Encoding.UTF8,
                            "application/json");
                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiReg, jsonContent);
                    // Kiểm tra phản hồi từ API

                    if (response.IsSuccessStatusCode)
                    {
                        string mess = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(mess);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại vui lòng kiểm tra lại thông tin");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
            clearFormPhieuKham();
            guna2DateTimePicker1_ValueChanged_1(sender, e);
        }

        private void txt_kqKham_Click(object sender, EventArgs e)
        {

        }






        // Định nghĩa các lớp tương ứng với cấu trúc JSON nhận được từ API

        public class YourDataModel
        {
            public int err { get; set; }
            public string mess { get; set; }
            public DataModel Data { get; set; }
            public List<InforBenhVien> InforBenhVien { get; set; }
            public List<InforChuyenKhoa> InforChuyenKhoa { get; set; }
        }

        public class DataModel
        {
            public int id { get; set; }
            public string timeSlot { get; set; }
            public string activateDay { get; set; }
            public User User { get; set; }
            public Sescription Sescription { get; set; }
        }

        public class User
        {
            public int id { get; set; }
            public string name { get; set; }
            public string diaChi { get; set; }
            public string namSinh { get; set; }
            public string gioiTinh { get; set; }
            public string sdt { get; set; }
        }
        // Phòng khám
        public class Sescription
        {
            public int id { get; set; }
            public int id_benhvien { get; set; }
            public string name { get; set; }
        }

        private class DataScheduleBooked
        {
            public int err { get; set; }
            public string mess { get; set; }
            public int count { get; set; }
            public Schedule[] schedule { get; set; }
        }
        // Lịch khám
        private class Schedule
        {
            public int id { get; set; }
            public string timeSlot { get; set; }
            public string activateDay { get; set; }
            public Users Users { get; set; }
            public IdBenhVien id_benhVien { get; set; }
        }
        // lớp cho Schedule ( Thông tin chuyên khoa và tên chuyên khoa và id bệnh viện đặt khám)
        private class Users
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        private class IdBenhVien
        {
            public int id_benhvien { get; set; }
        }
        // Thông tin  bệnh viện
        public class InforBenhVien
        {
            public string Name { get; set; }
            public string Sdt { get; set; }
            public string DiaChi { get; set; }
        }
        public class InforChuyenKhoa
        {
            public string Name { get; set; }
            public string id_benhVien { get; set; }
            public string id { get; set; }
        }
    }

}