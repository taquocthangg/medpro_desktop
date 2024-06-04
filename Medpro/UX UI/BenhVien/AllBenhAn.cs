using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Login.Component.EncodeToken;
using System.IO;

namespace Login.UX_UI.BenhVien
{
    public partial class AllBenhAn : Form
    {
        private const double ColumnWidthPercentage = 0.33; // Phần trăm chiều rộng cho mỗi cột
        private string activateDay = "";
        private Form parentForm;
        Bitmap MemoryImage;
        private PrintDocument printDocument1 = new PrintDocument();
        private PrintPreviewDialog previewdlg = new PrintPreviewDialog();
        private ApiRes apiResults;
        public AllBenhAn()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printdoc1_PrintPage);
            clearForm();
        }
        public AllBenhAn(Form parent) : this()
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
        public void GetPrintArea(Panel pnl)
        {
            MemoryImage = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(MemoryImage, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (MemoryImage != null)
            {
                e.Graphics.DrawImage(MemoryImage, 0, 0);
                base.OnPaint(e);
            }
        }
        void printdoc1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pagearea = e.PageBounds;
            e.Graphics.DrawImage(MemoryImage, (pagearea.Width / 2) - (this.form_hosokham.Width / 2), this.form_hosokham.Location.Y);
        }
        public void Print(Panel pnl)
        {
            Panel pannel = pnl;
            GetPrintArea(pnl);
            previewdlg.Document = printDocument1;
            previewdlg.ShowDialog();
        }
        public void PrintBenhAn(Panel pnl)
        {
            GetPrintArea(pnl);

            // Sử dụng máy in mặc định
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printdoc1_PrintPage);
            printDoc.Print();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Print(this.form_hosokham);
        }
        public void clearForm()
        {
            labelPatientName.Text = "";
            txt_ChuyenKhoa.Text = "";
            txt_gioiTinh.Text = "";
            txt_sdt.Text = "";
            txt_namSinh.Text = "";
            txtNgayKham.Text = "";
            txt_benhan.Text = "";
            txt_o_donThuoc.Text = "";
            txt_TenBenhVien.Text = "";
            txt_sdt_benhVien.Text = "";
            txt_diaChi.Text = "";
            txtTime.Text = "";
            btn_XuatPDF.Visible = false;
            btn_Done.Visible = false;
            separatorControl1.Visible = false;
            txt_TenBacSi.Text = "";
            txt_donThuoc.Text = "";
            txt_kqKham.Text = "";
        }
        public void ShowForm()
        {
            btn_XuatPDF.Visible = true;
            btn_Done.Visible = true;
            txt_kqKham.Text = "Kết quả khám";
            txt_donThuoc.Text = "Đơn Thuốc";
            separatorControl1.Visible = true;
            txt_TenBacSi.Text = "Phiếu Khám";
            txt_donThuoc.Text = "Đơn Thuốc";
            txt_kqKham.Text = "Kết quả khám ";
        }
        private async void AllBenhAn_Load(object sender, EventArgs e)
        {
            string userId = AuthManager.CurrentUser.id;

            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-hoan-thanh-bv/" + userId;
           

            // Gọi API bằng phương thức Get
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc dữ liệu JSON từ phản hồi
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    apiResults = JsonConvert.DeserializeObject<ApiRes>(jsonResult);

                    foreach (var schedule in apiResults.Data)
                    {
                        string[] row = {
                        string.Empty,
                        string.Empty,
                        schedule.appointmentDate,
                     };
                        var patientInfo = apiResults.Inforpatient.FirstOrDefault(p => p.id == schedule.patientId);

                        if (patientInfo != null)
                        {
                            row[0] = patientInfo.name;
                            row[1] = patientInfo.gioiTinh;
                        }

                        ListViewItem item = new ListViewItem(row);
                        // Thêm dữ liệu vào ListView
                        item.Tag = schedule.id; // Lưu ID vào Tag
                        listView1.Items.Add(item);
                    }


                }

                else
                {
                    // Xử lý lỗi nếu có
                    messBox.Show($"Error: {response.StatusCode}");
                }
            }
        }
        private async void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0  && listView1.SelectedItems[0].Tag != null)
            {
                // Lấy ID của lịch khám từ ListViewItem được chọn
                string selectedId = listView1.SelectedItems[0].Tag.ToString();
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lich-kham-hoan-thanh-by-lichkham/" + selectedId;

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
                            Application data = JsonConvert.DeserializeObject<Application>(jsonResponse);
                            foreach (var item in data.Data)
                            {
                               
                                txtNgayKham.Text = "Ngày khám: " + item.appointmentDate;
                                txtTime.Text = "Thời gian khám: " + item.timeSlot+"H";

                                txt_benhan.Text = "Chuẩn đoán : "+item.diagnosis;
                                txt_o_donThuoc.Text = "Đơn thuốc" +item.medication;
                            }

                            foreach (var schedule in data.InforBenhVien)
                            {
                                txt_TenBenhVien.Text = "Bác sĩ: " + schedule.name;
                                txt_sdt_benhVien.Text = "Số điện thoại: " + schedule.sdt;
                                txt_diaChi.Text = "Địa chỉ: " + schedule.diaChi;
                            }
                            foreach (var schedule in data.InforChuyenKhoa)
                            {
                                txt_ChuyenKhoa.Text = "Chuyên Khoa: " + schedule.name;
                            }
                            foreach(var item in data.Inforpatient)
                            {
                                labelPatientName.Text = "Tên Bệnh Nhân: " + item.name;
                                txt_gioiTinh.Text = "Giới Tính: " + item.gioiTinh;
                                txt_sdt.Text = "Số Điện Thoại: " + item.sdt;
                                //txt_namSinh.Text = "Ngày sinh: " + item.namSinh;
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

        private void btn_Done_Click(object sender, EventArgs e)
        {
            using (PrintDialog printDialog = new PrintDialog())
            {
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Thiết lập máy in mặc định cho printDocument1
                    printDocument1.PrinterSettings.PrinterName = printDialog.PrinterSettings.PrinterName;

                    Print(this.form_hosokham);
                }
            }
        }
        public static void ExportToExcel(ListView listView, string filePath)
        {
            try
            {
                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                // Tạo một đối tượng ExcelPackage
                using (var package = new ExcelPackage())
                {
                    // Tạo một Sheet trong ExcelPackage
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Bệnh Án");

                    // Ghi dữ liệu từ ListView vào Excel
                    int row = 1;
                    int col = 1;

                    // Ghi tên cột
                    foreach (ColumnHeader header in listView.Columns)
                    {
                        worksheet.Cells[row, col].Value = header.Text;
                        col++;
                    }

                    // Chuyển sang dòng tiếp theo
                    row++;

                    // Ghi dữ liệu từ ListView vào Excel
                    foreach (ListViewItem item in listView.Items)
                    {
                        col = 1;
                        foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                        {
                            worksheet.Cells[row, col].Value = subItem.Text;
                            col++;
                        }
                        // Chuyển sang dòng tiếp theo
                        row++;
                    }

                    // Lưu file Excel
                    FileInfo excelFile = new FileInfo(filePath);
                    package.SaveAs(excelFile);
                }

                MessageBox.Show("Xuất Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
            }
        }
        private void Btn_XuatExcel(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            saveFileDialog.Title = "Chọn nơi lưu file Excel";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(listView1, saveFileDialog.FileName);
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
            public string appointmentDate { get; set; }
            public string diagnosis { get; set; }
            public string medication { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public User User { get; set; }
            public Sescription Sescription { get; set; }

        }
        public class InforBenhVien
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sdt { get; set; }
            public string diaChi { get; set; }

        }
        public class InforChuyenKhoa
        {
            public string id { get; set; }
            public string name { get; set; }

        }
        public class Inforpatient
        {
            public string id { get; set; }
            public string name { get; set; }
            public string sdt { get; set; }
            public string diaChi { get; set; }
            public string gioiTinh { get; set; }

        }
        public class Application
        {
            public int err { get; set; }
            public string mess { get; set; }
            public Data[] Data { get; set; }
            public User[] User { get; set; }
            public IList<InforBenhVien> InforBenhVien { get; set; }
            public IList<Inforpatient> Inforpatient { get; set; }
            public IList<InforChuyenKhoa> InforChuyenKhoa { get; set; }

        }


        public class ApiRes
        {
            public int err { get; set; }
            public string mess { get; set; }
            public Data[] Data { get; set; }
            public Inforpatient[] Inforpatient { get; set; }

        }
    }
}
