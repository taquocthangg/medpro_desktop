namespace Login.UX_UI.BenhVien
{
    partial class LichHuy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listViewChuyenKhoa = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // listViewChuyenKhoa
            // 
            this.listViewChuyenKhoa.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewChuyenKhoa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewChuyenKhoa.Font = new System.Drawing.Font("UTM Alexander", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewChuyenKhoa.HideSelection = false;
            this.listViewChuyenKhoa.Location = new System.Drawing.Point(0, 0);
            this.listViewChuyenKhoa.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
            this.listViewChuyenKhoa.Name = "listViewChuyenKhoa";
            this.listViewChuyenKhoa.Size = new System.Drawing.Size(1150, 713);
            this.listViewChuyenKhoa.TabIndex = 2;
            this.listViewChuyenKhoa.TileSize = new System.Drawing.Size(228, 100);
            this.listViewChuyenKhoa.UseCompatibleStateImageBehavior = false;
            this.listViewChuyenKhoa.View = System.Windows.Forms.View.Details;
            this.listViewChuyenKhoa.SizeChanged += new System.EventHandler(this.XacNhanKham_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tên bệnh nhân";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Thời gian";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Ngày khám";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 200;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(50, 50);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // LichHuy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 713);
            this.Controls.Add(this.listViewChuyenKhoa);
            this.MinimumSize = new System.Drawing.Size(1168, 760);
            this.Name = "LichHuy";
            this.Text = "LichHuy";
            this.Load += new System.EventHandler(this.LichHuy_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewChuyenKhoa;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ImageList imageList1;
    }
}