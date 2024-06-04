using DevExpress.XtraPrinting.Native.WebClientUIControl;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Login.UX_UI;
using static Login.Component.EncodeToken;
using Login.UX_UI.BenhVien;
using Login.UX_UI.BacSi;

namespace Login
{
    public partial class Auth : DevExpress.XtraEditors.XtraForm
    {
        private int imageX = 0; // Vị trí ban đầu của hình ảnh
        private Timer timer; // Khai báo biến timer ở mức lớp
        private bool movingRight = true;
        private bool isMoving = false; // Biến để xác định hình ảnh có đang di chuyển hay không

        //Api
        private const string apiUrl = "https://medprov2.onrender.com/api/v1/auth/login";
        private const string apiReg = "https://medprov2.onrender.com/api/v1/auth/";

        private Loadding loadingControl;
        public Auth()
        {
            InitializeComponent();
            InitializeTimer(); // Khởi tạo Timer
            // Khởi tạo LoadingControl và thêm nó vào Form
            loadingControl = new Loadding();
            loadingControl.Dock = DockStyle.Fill;
            this.Controls.Add(loadingControl);
            loadingControl.Visible = false; // Ban đầu ẩn đi
        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(Timer_Tick);
        }
        private void clearForm()
        {
            txt_fullName.Text = "";
            txt_email_register.Text = "";
            txt_password_register.Text = "";
            txt_password_register_nhapLai.Text = "";
            txt_namSinh.Text = "";
            txt_SDT_register.Text = "";
            txt_diachi_register.Text = "";
        }

        // Hiệu ứng 
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Di chuyển hình ảnh
            if (isMoving)
            {
                if (movingRight && imageX < 485)
                {
                    imageX += 20; // Tốc độ di chuyển (có thể điều chỉnh)
                    img_Login.Left = imageX;
                }
                else if (!movingRight && imageX > 30)
                {
                    imageX -= 20; // Tốc độ di chuyển (có thể điều chỉnh)
                    img_Login.Left = imageX;
                    //img_Login.Left = 20;
                }
                else
                {
                    isMoving = false; // Dừng di chuyển khi đạt đến 300px hoặc vị trí ban đầu
                }
            }
        }
        // Hiện thị mật khẩu người dùng nhập
        private void showPass_Click(object sender, EventArgs e)
        {
            if (txt_password_Login.UseSystemPasswordChar)
            {
                // Hiển thị mật khẩu
                txt_password_Login.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu
                txt_password_Login.UseSystemPasswordChar = true;
            }
        
    }
        // Bắt sự thay đổi khi người dùng nhập mật khẩu vào để hiện lên ShowPass
        private void txt_password_Login_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_password_Login.Text))
            {
                showPass.Visible = true; // Hiển thị nút khi có ký tự
            }
            else
            {
                showPass.Visible = false; // Ẩn nút khi không có ký tự
            }
        }
        //Mở panel đăng nhập
        private void link_login_Click(object sender, EventArgs e)
        {
            movingRight = false;
            isMoving = true;
            timer.Start();
        }
        //Mở panel đăng ký
        private void linkRegister_Click(object sender, EventArgs e)
        {
            movingRight = true;
            isMoving = true;
            timer.Start();
        }
        //Mở panel quên mật khẩu
        private void btn_forgotPassword_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = false;
            panelForgotpassword.Visible = true;
        }
        //Quay lại panel đăng nhập
        private void btnBack_login_Click(object sender, EventArgs e)
        {
            panelForgotpassword.Visible = false;
            panelLogin.Visible = true;
        }
        //Đóng form auth
        private void close_panel_login_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }




        // Xử lý khi người dùng đăng nhập
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txt_email_Login.Text;
            string password = txt_password_Login.Text;
            if (IsValidEmail(email))
            {
                var loginData = new { email, password };
                loadingControl.StartLoading();
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var jsonContent = new StringContent(
                            JsonConvert.SerializeObject(loginData),
                            Encoding.UTF8,
                            "application/json");

                        var response = await client.PostAsync(apiUrl, jsonContent);

                            loadingControl.HideLoading();
                        if (response.IsSuccessStatusCode)
                        {
                            string tokenResponse = await response.Content.ReadAsStringAsync();
                            string token = ExtractTokenFromResponse(tokenResponse);
                            string mess = Mess(tokenResponse);
                            if (token == "")
                            {
                                messBox.Show(mess);
                            }
                            else
                            {
                                TokenManager.Instance.AccessToken = token;
                                AuthManager.SetAccessToken(token);
                                // Giải mã token để truy cập payload
                                var handler = new JwtSecurityTokenHandler();
                                var jwtToken = handler.ReadJwtToken(token);

                                // Lấy giá trị role_id từ payload
                                string userRoleId = jwtToken.Claims.FirstOrDefault(c => c.Type == "role_id")?.Value;


                                // Kiểm tra vai trò và phân quyền người dùng
                                if (userRoleId == "R1")
                                {
                                    // Phân quyền cho vai trò R1
                                    messBox.Show(mess);
                                    Admins admin = new Admins();
                                    this.Hide();
                                    admin.ShowDialog();

                                }
                                else if (userRoleId == "R2")
                                {
                                    // Phân quyền cho vai trò R2
                                    messBox.Show(mess);
                                    BenhVien benhvien = new BenhVien();
                                    this.Hide();
                                    benhvien.Show();
                                }
                                else if (userRoleId == "R3")
                                {
                                    // Phân quyền cho vai trò R3
                                    messBox.Show(mess);
                                    BacSi bacSi = new BacSi();
                                    this.Hide();
                                    bacSi.Show();
                                }
                                else if (userRoleId == "R4")
                                {
                                    // Phân quyền cho vai trò R3
                                    messBox.Show(mess);
                                    FormUser users = new FormUser();
                                    this.Hide();
                                    users.Show();
                                }
                                else
                                {
                                    messBox.Show(mess);
                                }
                            }

                        }
                        else
                        {
                            messBox.Show("Đăng nhập không thành công. Vui lòng kiểm nhập đầy đủ email và mật khẩu.");
                        }
                    }
                }
                catch(Exception ex ) {
                    messBox.Show("Lỗi không thể kết nối đến api... " + ex);
                    loadingControl.HideLoading();
                }
            }
            else
            {
                messBox.Show("Vui lòng nhập email hợp lệ !!!");
            }
        }
        // Xử lý khi người dùng đăng ký
                        
        private async void btnRegister_Click(object sender, EventArgs e)
        {
           if(txt_gioiTinh.SelectedItem != null) {
                string name = txt_fullName.Text;
                string email = txt_email_register.Text;
                string password = txt_password_register.Text;
                string Nhaplai_password = txt_password_register_nhapLai.Text;
                string namSinh = txt_namSinh.Text;
                string gioiTinh = txt_gioiTinh.SelectedItem.ToString();
                string sdt = txt_SDT_register.Text;
                string diaChi = txt_diachi_register.Text;
                if (password == Nhaplai_password)
                {
                    var loginData = new { name, email, password, namSinh,gioiTinh, sdt, diaChi };
                    loadingControl.StartLoading();
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            // Tạo JSON object chứa thông tin đăng ký
                            var jsonContent = new StringContent(
                                    JsonConvert.SerializeObject(loginData),
                                    Encoding.UTF8,
                                    "application/json");
                            // Gửi yêu cầu POST đến API
                            HttpResponseMessage response = await client.PostAsync(apiReg, jsonContent);
                            string tokenResponse = await response.Content.ReadAsStringAsync();
                            string mess = Mess(tokenResponse);
                            loadingControl.HideLoading();
                            // Kiểm tra phản hồi từ API

                            if (response.IsSuccessStatusCode)
                            {
                                messBox.Show(mess);
                                clearForm();
                                link_login_Click(sender, e);
                            }
                            else
                            {
                                messBox.Show("Đăng ký thất bại. Vui lòng kiểm tra lại thông tin đăng ký");
                            }
                        }
                        catch (Exception ex)
                        {
                            messBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                        }
                    }
                }
                else
                {
                    messBox.Show("Mật Khẩu không khớp !!!");
                }
            }
            else
            {
                messBox.Show("Vui lòng chọn giới tính !!!");
            }
        }
        // Xử lý khi người dùng quên mật khẩu
        private async void btnRestPassword_Click(object sender, EventArgs e)
        {
            string email = txt_emailForgotPassword.Text;
           
            using (HttpClient client = new HttpClient())
            {
                // Địa chỉ URL của API Forgot Password
                string apiUrl = "https://medprov2.onrender.com/api/v1/auth/forgot-password";

                try
                {
                    string jsonBody = $"{{ \"email\": \"{email}\" }}";
                        loadingControl.StartLoading();
                    StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string tokenResponse = await response.Content.ReadAsStringAsync();
                    string emailForgotPassword = txt_emailForgotPassword.Text;
                    if (IsValidEmail(emailForgotPassword))
                    {
                        loadingControl.HideLoading();
                        if (response.IsSuccessStatusCode)
                        {
                            // Phản hồi thành công từ API
                            messBox.Show("Đã gửi yêu cầu về email!! Vui lòng truy cập vào email để xác nhận quên mật khẩu!!!");
                            panelForgotpassword.Visible = false;
                            panelLogin.Visible = true;
                        }
                        else
                        {
                            messBox.Show("Tài khoản chưa được đăng kí.");

                            // Xử lý lỗi phản hồi từ API
                            //labelMessage.Text = "Lỗi khi gửi yêu cầu. Vui lòng thử lại sau.";
                        }
                    }
                    else
                    {
                        // Kiểm tra phản hồi từ API
                        messBox.Show("Vui lòng nhập vào email hợp lệ !!!");
                    }

                }
                catch (Exception ex)
                {
                    // Xử lý lỗi khi gửi yêu cầu
                    // labelMessage.Text = "Lỗi khi gửi yêu cầu. Vui lòng kiểm tra kết nối mạng.";
                    messBox.Show("Lỗi khi gửi yêu cầu. Vui lòng kiểm tra kết nối mạng."+ex);
                }
            }
        }
        // Giải mã token để lấy accset_token từ server
        private string ExtractTokenFromResponse(string jsonResponse)
        {
            // Sử dụng JSON.NET để trích xuất token từ chuỗi JSON
            JObject json = JObject.Parse(jsonResponse);
            string token = json["access_token"]?.ToString(); 

            return token;
        }
        // Giải mã token để lấy mess từ server
        private string Mess(string jsonResponse)
        {
            // Sử dụng JSON.NET để trích xuất mess từ chuỗi JSON
            JObject json = JObject.Parse(jsonResponse);
            string mess = json["mess"]?.ToString(); 

            return mess;
        }
        // Kiểm tra email hợp lệ
        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
           // string pattern = "";

            // Sử dụng Regex.IsMatch để kiểm tra xem chuỗi email có khớp với biểu thức chính quy hay không
            return Regex.IsMatch(email, pattern);
        }


        private void Auth_Load(object sender, EventArgs e)
        {
            showPass.Visible = false;
            showPassDangKi.Visible = false;
            showPassDangKi1.Visible = false;
            txt_password_Login.UseSystemPasswordChar = true;
            txt_password_register.UseSystemPasswordChar = true;
            txt_password_register_nhapLai.UseSystemPasswordChar = true;

            txt_gioiTinh.SelectedIndex = 0;
        }

        private void txt_password_register_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_password_register.Text))
            {
                showPassDangKi.Visible = true; // Hiển thị nút khi có ký tự
            }
            else
            {
                showPassDangKi.Visible = false; // Ẩn nút khi không có ký tự
            }
        }

        private void showPassDangKi_Click(object sender, EventArgs e)
        {
            if (txt_password_register.UseSystemPasswordChar)
            {
                // Hiển thị mật khẩu
                txt_password_register.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu
                txt_password_register.UseSystemPasswordChar = true;
            }
        }

        private void showPassDangKi1_Click(object sender, EventArgs e)
        {
            if (txt_password_register_nhapLai.UseSystemPasswordChar)
            {
                // Hiển thị mật khẩu
                txt_password_register_nhapLai.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu
                txt_password_register_nhapLai.UseSystemPasswordChar = true;
            }
        }

        private void txt_password_register_nhapLai_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_password_register_nhapLai.Text))
            {
                showPassDangKi1.Visible = true; // Hiển thị nút khi có ký tự
            }
            else
            {
                showPassDangKi1.Visible = false; // Ẩn nút khi không có ký tự
            }
        }
    }
}
