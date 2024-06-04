using System;
using System.Windows.Forms;

namespace Login
{
    public partial class Loadding : UserControl
    {

        public Loadding()
        {
            InitializeComponent();
        }
        public void StartLoading()
        {
            // Ẩn nút tải dữ liệu và hiển thị ProgressBar
           
            this.Visible = true;
            // Khởi tạo và bắt đầu BackgroundWorker
            backgroundWorker1.RunWorkerAsync();
        }
        public void HideLoading()
        {
           
            this.Visible = false;
            this.BringToFront();
        }
        private void Loadding_Load(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
