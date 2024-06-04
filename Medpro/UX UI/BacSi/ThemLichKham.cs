using DevExpress.XtraEditors;
using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using static Login.Component.EncodeToken;

namespace Login.UX_UI.BacSi
{
    public partial class ThemLichKham : DevExpress.XtraEditors.XtraForm
    {

        private List<Guna2GradientButton> selectedButtons = new List<Guna2GradientButton>();
        private string[] timeSlot;
        private string activateDay = "";
        private const string api = "https://medprov2.onrender.com/api/v1/auth/themlichkham";
        private const string apiUrl = "https://medprov2.onrender.com/api/v1/auth/lichkham";

        public ThemLichKham()
        {

            InitializeComponent();
            InitializeButtons(); // Gọi hàm để khởi tạo các nút
            guna2DateTimePicker1.MinDate = DateTime.Now;
        }
        public ThemLichKham(Form parent) : this()
        {
           // parentForm = parent;
           // guna2MessageDialog1.Parent = parentForm;
        }
        private void InitializeButtons()
        {
            // Tạo danh sách các nút
            List<Guna2GradientButton> buttons = new List<Guna2GradientButton>
            {
                btn_1, btn_2, btn_3, btn_4 ,btn_5, btn_6, btn_7, btn_8 // Thêm các nút của bạn vào đây
            };

            // Gán sự kiện Click cho tất cả các nút
            foreach (var button in buttons)
            {
                button.Click += Guna2GradientButton_Click;
            }
        }
        private void Guna2GradientButton_Click(object sender, EventArgs e)
        {
            Guna2GradientButton clickedButton = (Guna2GradientButton)sender;

            // Kiểm tra xem nút đã được chọn trước đó hay chưa
            if (selectedButtons.Contains(clickedButton))
            {
                // Nếu nút đã được chọn, hủy chọn nó
                selectedButtons.Remove(clickedButton);
                clickedButton.FillColor = Color.FromArgb(94, 148, 255); // Màu ban đầu của nút
                clickedButton.FillColor2 = Color.FromArgb(255, 77, 165); // Màu ban đầu của nút
            }
            else
            {
                // Nếu nút chưa được chọn, thêm vào danh sách nút đã chọn
                selectedButtons.Add(clickedButton);
                clickedButton.FillColor = Color.FromArgb(148, 148, 148); // Màu khi được chọn
                clickedButton.FillColor2 = Color.FromArgb(148, 148, 148); // Màu ban đầu của nút

            }

            // Hiển thị giá trị của tất cả các nút đã chọn (bạn có thể thay đổi hành động này tùy thuộc vào nhu cầu của bạn)
            string selectedValues = string.Join(", ", selectedButtons.Select(button => $"\"{button.Text}\""));


            timeSlot = selectedButtons.Select(button => button.Text).ToArray();

        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value;
            activateDay = selectedDate.ToShortDateString();
        }



        private async void add_lichKham_Click(object sender, EventArgs e)
        {
            string doctorId = AuthManager.CurrentUser.id;
            string specialtyId = AuthManager.CurrentUser.id_chuyenKhoa;
            var addData = new { doctorId, specialtyId, timeSlot, activateDay };
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Tạo JSON object chứa thông tin đăng ký
                    var jsonContent = new StringContent(
                            JsonConvert.SerializeObject(addData),
                            Encoding.UTF8,
                            "application/json");
                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(api, jsonContent);
                    string tokenResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(tokenResponse);
                    string messageFromAPI = apiResponse.mess;
                   // MessageBox.Show(apiResponse);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(messageFromAPI);

                        dateTimePicker1_ValueChanged(sender, e);
                    }
                    else
                    {
                        MessageBox.Show(messageFromAPI);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }
            }
        }


        private async void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = guna2DateTimePicker1.Value;
            activateDay = selectedDate.ToShortDateString();
            string doctorId = AuthManager.CurrentUser.id;
            string apiEndpoint = $"{apiUrl}/{doctorId}";

            // Tạo đối tượng HttpClient để thực hiện request API
            using (HttpClient client = new HttpClient())
            {
                // Tạo đối tượng để chứa dữ liệu gửi đi
                var postData = new { activateDay };

                // Chuyển đối tượng postData thành JSON và gửi request
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(postData),
                    Encoding.UTF8,
                    "application/json");

                // Gửi yêu cầu POST đến API
                HttpResponseMessage response = await client.PostAsync(apiEndpoint, jsonContent);

                // Kiểm tra nếu request thành công (status code 200)
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung JSON từ response
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (jsonResponse.Contains("Không có lịch khám"))
                    {
                        // Xóa tất cả các control hiện tại trong flowLayoutPanel1 và thêm Label thông báo
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
                        var scheduleData = JsonConvert.DeserializeObject<ScheduleResponse>(jsonResponse);
                        ShowSchedule(scheduleData);
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
            }
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
            MessageBox.Show($"Bạn đã chọn lịch khám có ID: {scheduleId}");
            try
            {
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/xoalich/" + scheduleId;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync(apiUrl);
                    string tokenResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(tokenResponse);
                    string messageFromAPI = apiResponse.mess;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show(messageFromAPI);
                        dateTimePicker1_ValueChanged(sender, e);
                    }
                    else
                    {
                        MessageBox.Show(messageFromAPI);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        public class ScheduleResponse
        {
            public int Err { get; set; }
            public string Mess { get; set; }
            public string Count { get; set; }
            public List<ScheduleSlot> Schedule { get; set; }
        }

        public class ScheduleSlot
        {
            public int Id { get; set; }
            public string TimeSlot { get; set; }
        }

        private void ThemLichKham_Load(object sender, EventArgs e)
        {

        }
    }



}