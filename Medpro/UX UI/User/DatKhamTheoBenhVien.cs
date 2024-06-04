using DevExpress.XtraEditors;
using Login.UX_UI.BacSi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Login.Component.EncodeToken;
using static Login.UX_UI.BacSi.ThemLichKham;

namespace Login.UX_UI.User
{
    public partial class DatKhamTheoBenhVien : DevExpress.XtraEditors.XtraForm
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private Loadding loadingControl;
        private Form parentForm;
        public DatKhamTheoBenhVien()
        {
            InitializeComponent();
            listView2.Visible = false;
            flowLayoutPanel1.Visible=false;
            listView3.Visible = false;
            guna2DateTimePicker1.Visible = false;
            txt.Visible = false;
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            listView1.Controls.Add(loadingControl);
            loadingControl.Visible = false;
        }
        public DatKhamTheoBenhVien(Form parent) : this()
        {
            parentForm = parent;
            messBox.Parent = parentForm;
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void DatKhamTheoBenhVien_Load(object sender, EventArgs e)
        {
            try
            {
                loadingControl.StartLoading();
                listView1.LargeImageList = imageList1;
                // Gọi API để lấy dữ liệu về
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/getAllBenhVien";
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
                }
            }
            catch(Exception ex) { messBox.Show("Lỗi : " + ex);  }
            finally { loadingControl.HideLoading(); }
        }
       

        private async void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            

            // Xóa tất cả các mục và cột header hiện tại trong listView2
            listView2.Items.Clear();
            listView2.Columns.Clear();

            // Thêm các cột header cho listView2 (title)
            listView2.Columns.Add("Tên Chuyên Khoa", 150); // Thay đổi chiều rộng cột nếu cần thiết
            listView2.Columns.Add("Mô Tả", 200);

            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    loadingControl.StartLoading();
                    ListViewItem selectedItem = listView1.SelectedItems[0];
                    string userId = selectedItem.ImageKey;

                    // Thực hiện một yêu cầu HTTP mới để lấy thông tin chuyenkhoa
                    string chuyenkhoaApiUrl = $"https://medprov2.onrender.com/api/v1/auth/chuyenkhoa/{userId}";
                    string chuyenkhoaJsonResponse = await _httpClient.GetStringAsync(chuyenkhoaApiUrl);
                    var chuyenkhoaData = JsonConvert.DeserializeObject<ChuyenKhoaApiData>(chuyenkhoaJsonResponse);

                    // Cập nhật listView2 với dữ liệu chuyenkhoa nhận được
                    if (chuyenkhoaData.Count > 0)
                    {
                        foreach (var chuyenkhoa in chuyenkhoaData.ChuyenKhoa)
                        {
                            // Tạo một ListViewItem mới cho mỗi mục chuyenkhoa
                            var newItem = new ListViewItem(new[] { chuyenkhoa.Name, chuyenkhoa.Description });

                            // Lưu ID vào thuộc tính Tag của ListViewItem
                            newItem.Tag = chuyenkhoa.Id;

                            // Thêm ListViewItem vào listView2
                            listView2.Items.Add(newItem);
                        }
                    }
                    else {
                        messBox.Show("Bệnh viện đang bảo trì vui lòng thử lại sau!!!");
                        this.Close();
                    }
                   
                }
                catch (Exception ex) { messBox.Show("Bệnh viện đang bảo trì vui lòng thử lại sau!!!" +ex); }
                finally 
                { 
                    loadingControl.HideLoading();
                    // Ẩn listView1
                    listView1.Visible = false;
                    listView2.Visible = true; 
                }  
                
            }
        }


        private async void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                // Lấy ID từ thuộc tính Tag của ListViewItem
                if (listView2.SelectedItems[0].Tag != null)
                {
                    try
                    {
                        listView2.Visible = false;
                        listView1.Visible = true;
                        loadingControl.StartLoading();
                        string chuyenkhoaId = listView2.SelectedItems[0].Tag.ToString();
                        // messBox.Show(chuyenkhoaId);
                        // Gọi API để lấy danh sách bác sĩ dựa trên chuyenkhoaId
                        string bacsiApiUrl = $"https://medprov2.onrender.com/api/v1/auth/getBacSiByChuyenKhoa/{chuyenkhoaId}";
                        string bacsiJsonResponse = await _httpClient.GetStringAsync(bacsiApiUrl);
                        var bacsiData = JsonConvert.DeserializeObject<ApiData>(bacsiJsonResponse);
                        messBox.Show(bacsiJsonResponse);

                        // Xóa tất cả các mục và cột header hiện tại trong listView3
                        listView3.LargeImageList = imageList3;

                        // Thêm các cột header cho listView3


                        // Cập nhật listView3 với dữ liệu bác sĩ nhận được
                        foreach (var user in bacsiData.Users)
                        {
                            // Tạo một ListViewItem với tên của bệnh viện
                            var item = new ListViewItem(user.Name);
                            item.SubItems.Add(user.GioiTinh);

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
                                    imageList3.Images.Add(imageKey, image);
                                }
                            }
                            else
                            {
                                item.ImageKey = "default_avatar";
                            }

                            // Thêm ListViewItem vào ListView
                            listView3.Items.Add(item);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        messBox.Show("Bệnh viện đang trong quá trình cập nhập chức năng vui lòng thử lại sau ít phút !!!");
                        this.Close();
                    }

                    finally
                    { 
                        loadingControl.HideLoading();
                        listView1.Visible = false;
                        listView3.Visible = true;
                    }
                }
            }
        }
        private string activateDay = "";
        private string id = "";
        private const string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lichkham";

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                listView3.Visible=false;
                flowLayoutPanel1.Visible = true;
                guna2DateTimePicker1.Visible = true;
                txt.Visible = true;
                checkNgay(sender, e);
            }
        }
            private async void checkNgay(object sender, EventArgs e)
            {
                guna2DateTimePicker1.MinDate = DateTime.Now;
                DateTime selectedDate = guna2DateTimePicker1.Value;
                activateDay = selectedDate.ToShortDateString();
                ListViewItem selectedItem = listView3.SelectedItems[0];
                string DoctorId = selectedItem.ImageKey;
                string apiEndpoint = "https://medprov2.onrender.com/api/v1/auth/lichkham/" + DoctorId;
                var postData = new { activateDay };
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Chuyển đối tượng postData thành JSON và gửi request
                    var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(postData),
                    Encoding.UTF8,
                    "application/json");
                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiEndpoint, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string tokenResponse = await response.Content.ReadAsStringAsync();
                        if (tokenResponse.Contains("Không có lịch khám"))
                        {
                            flowLayoutPanel1.Controls.Clear();
                            Label lblNoSchedule = new Label
                            {
                                Text = "Không có lịch khám",
                                Width = 150,
                                Height = 30,
                                Margin = new Padding(5)
                            };

                            flowLayoutPanel1.Controls.Add(lblNoSchedule);
                        }
                        else
                        {
                            var apiResponse = JsonConvert.DeserializeObject<ScheduleResponse>(tokenResponse);
                            ShowSchedule(apiResponse);
                        }
                    }
                    else
                    {
                        // Xử lý lỗi, ví dụ: hiển thị một thông báo lỗi hoặc ghi log
                        messBox.Show("Lỗi trong yêu cầu API: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex) { messBox.Show("Lỗi: " +ex); }
            finally { loadingControl.HideLoading(); };

        }
            private void ShowSchedule(ScheduleResponse scheduleData)
            {

                // Xóa tất cả các control hiện tại trong flowLayoutPanel1
                flowLayoutPanel1.Controls.Clear();

                if (scheduleData.Schedule != null && scheduleData.Schedule.Any())
                {
                    // Hiển thị thông tin lịch khám lên các button
                    foreach (var slot in scheduleData.Schedule)
                    {
                        Button btn = new Button
                        {
                            Text = slot.TimeSlot,
                            Tag = slot.Id,
                            Width = 180,
                            Height = 45,
                            Margin = new Padding(5)
                        };
                        btn.Click += ScheduleButton_Click; // Thêm sự kiện Click cho button
                        flowLayoutPanel1.Controls.Add(btn);
                    }
                }
                else
                {
                    // Thêm một Label thông báo khi không có lịch khám
                    Label lblNoSchedule = new Label
                    {
                        Text = "Không có lịch khám",
                        Width = 150,
                        Height = 30,
                        Margin = new Padding(5)
                    };

                    flowLayoutPanel1.Controls.Add(lblNoSchedule);
                }
            }
        private async void ScheduleButton_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi button được click, ví dụ:
            Button clickedButton = (Button)sender;
            int scheduleId = (int)clickedButton.Tag;
            messBox.Show($"Bạn đã chọn lịch khám có ID: {scheduleId}");
            try
            {
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/datlich/" + scheduleId;
                string id_benhNhan = AuthManager.CurrentUser.id;

                using (HttpClient client = new HttpClient())
                {
                    // Tạo đối tượng để chứa dữ liệu gửi đi
                    var postData = new { id_benhNhan };

                    // Chuyển đối tượng postData thành JSON và gửi request
                    var jsonContent = new StringContent(
                        JsonConvert.SerializeObject(postData),
                        Encoding.UTF8,
                        "application/json");

                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
                    string tokenResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(tokenResponse);
                    string messageFromAPI = apiResponse.mess;
                    if (response.IsSuccessStatusCode)
                    {
                        messBox.Show(messageFromAPI);
                        checkNgay(sender, e);
                    }
                    else
                    {
                        messBox.Show(messageFromAPI);
                    }
                }
            }
            catch (Exception ex)
            {
                messBox.Show($"Lỗi: {ex.Message}");
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
            public string Email { get; set; }
            public string GioiTinh { get; set; }
            public string Avatar { get; set; }
        }
        public class ChuyenKhoaApiData
        {
            public string Message { get; set; }
            public int Count { get; set; }
            public ChuyenKhoa[] ChuyenKhoa { get; set; }
        }

        public class ChuyenKhoa
        {
            public int Id { get; set; }
            public int Id_BenhVien { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
        public class BacSiApiData
        {
            public string Message { get; set; }
            public List<User> Users { get; set; }
        }
    }
}