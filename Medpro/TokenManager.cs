using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public class TokenManager
    {
        private static TokenManager instance;
        private string accessToken;

        // Constructor riêng tư để ngăn tạo ra các phiên bản mới từ bên ngoài
        private TokenManager() { }

        // Thuộc tính để truy cập duy nhất một phiên bản của lớp TokenManager
        public static TokenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TokenManager();
                }
                return instance;
            }
        }

        // Thuộc tính AccessToken để lưu trữ và truy xuất token
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
    }

}
