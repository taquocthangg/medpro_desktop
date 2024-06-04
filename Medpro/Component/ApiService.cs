using DevExpress.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using static Login.Component.EncodeToken;
using System.Windows.Forms;
namespace Login.Component
{
    internal class ApiService
    {
        private readonly HttpClient _httpClient;

        
        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<UserData> GetUserDataAsync()
        {
            string userId = AuthManager.CurrentUser.id;
            string apiUrl = "https://medprov2.onrender.com/api/v1/auth/getCurent/" + userId;
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UserData>(json);
                return userData;
            }

            return null;
        }
        public static bool TryDownloadImage(string imageUrl, out Image image)
        {
            image = null;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(imageUrl);
                    using (var stream = new System.IO.MemoryStream(data))
                    {
                        image = System.Drawing.Image.FromStream(stream);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Không thể tải ảnh: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> getCurent(Label name_Admin, PictureBox img_avatar)
        {
            var apiService = new ApiService();
            var userData = (await apiService.GetUserDataAsync())?.User;

            if (userData != null)
            {
                name_Admin.Text = $"Xin Chào {userData.Name} !!!";

                if (ApiService.TryDownloadImage(userData.Avatar, out var userAvatar))
                {
                    img_avatar.Image = userAvatar;
                }

                return true; // Đăng nhập thành công
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Vui lòng thử đăng nhập lại");
                return false; // Đăng nhập thất bại
            }
        }
        public class UserData
        {
            public string Message { get; set; }
            public User User { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string GioiTinh { get; set; }
            public string NamSinh { get; set; }
            public string Sdt { get; set; }
            public string DiaChi { get; set; }
            public string Avatar { get; set; }
            public string RoleId { get; set; }
            public object ResetToken { get; set; }
            public object ResetTokenExpiry { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }

    }
}
