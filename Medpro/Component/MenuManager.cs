using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Login.Component
{


    internal class MenuQl
    {
        public static void CollapseExpandMenu(Panel panelMenu, Button btn_info, Label name_Admin, PictureBox img_avatar, Button btn_Menu, Panel panel_info)
        {
            if (panelMenu.Width > 200)
            {
                panelMenu.Width = 100;
                btn_info.Visible = false;
                name_Admin.Visible = false;
                img_avatar.Dock = DockStyle.Top;
                btn_Menu.Dock = DockStyle.Top;
                panel_info.Size = new Size(280, 150);

                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "";
                    menuButton.ImageAlign = ContentAlignment.MiddleCenter;
                    menuButton.Padding = new Padding(0);
                }
            }
            else
            {
                panelMenu.Width = 280;
                btn_info.Visible = true;
                name_Admin.Visible = true;
                btn_Menu.Dock = DockStyle.None;
                img_avatar.Dock = DockStyle.None;
                panel_info.Size = new Size(280, 210);

                foreach (Button menuButton in panelMenu.Controls.OfType<Button>())
                {
                    menuButton.Text = "   " + menuButton.Tag.ToString();
                    menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                    menuButton.Padding = new Padding(10, 0, 0, 0);
                }
            }
        }
    }


}
