using Guna.UI2.WinForms;
using KeyAuth;
using Loader.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static free.Form1;
using Memory;
using NetFwTypeLib; // Thư viện Windows Firewall
using System.ServiceProcess;
using Loader;

namespace free
{
    public partial class Form1 : Form
    {
        private bool isLabelVisible = false;
        private Color[] rainbowColors = new Color[]
          {
            Color.Red, Color.Blue, Color.White
      };

        private int colorIndex = 0;
        private int transparency = 255; // Độ trong suốt ban đầu
        private Color currentColor = Color.Red; // Màu ban đầu
        private int toolboxX = 0;
        private List<Point> snowflakes = new List<Point>();
        private Random random = new Random();
        int timerInterval, curWidth, curHeight, incWidth, incHeight, maxWidth, maxHeight;
        private List<Point> chemicalBondPoints = new List<Point>();
        private int step = 2;
        [DllImport("user32.dll")] static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll")] static extern Point GetCaretPos(out Point point);
        public Form1()

        {

            /*string fileUrl = "resume_internet.bat LINK";

            // Đường dẫn để lưu tệp sau khi tải về
            string savePath = "C:\\Users\\Nhan\\Documents\\New folder\\resume_internet.bat";
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(fileUrl, savePath);

                }
                catch (WebException ex)
                {

                }
            }*/

            
            this.InitializeComponent();
            this.timer1.Start();
            Point targetCaretPos;
            GetCaretPos(out targetCaretPos);
            username.TextChanged += (s, e) =>
            {
                Point temp;
                GetCaretPos(out temp);
                SetCaretPos(targetCaretPos.X, targetCaretPos.Y);
                targetCaretPos = temp;
            };
            Form2 nhan = new Form2();
            nhan.Show();
            Thread t = new Thread(() =>
            {
                Point current = targetCaretPos;
                while (true)
                {
                    if (current != targetCaretPos)
                    {

                        if (Math.Abs(current.X - targetCaretPos.X) + Math.Abs(current.Y - targetCaretPos.Y) > 23)
                            current = targetCaretPos;
                        else
                        {
                            current.X += Math.Sign(targetCaretPos.X - current.X);
                            current.Y += Math.Sign(targetCaretPos.Y - current.Y);
                        }


                        username.Invoke((Action)(() => SetCaretPos(current.X, current.Y)));
                    }
                    Thread.Sleep(20); // set speed effect slow or fast
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        public static api KeyAuthApp = new api(
  name: "World", // Application Name
    ownerid: "3srXNUCGte", // Owner ID
    secret: "", // Application Secret
    version: "1.0"
);
        public Mem MemLib = new Mem();
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //đoạn code dưới đây là có chức năng sử dụng tường lửa trong thư viện của window để chặn vào internet. thư viện tên là NetFwTypeLib
        public class FirewallManager
        {
            public static void BlockApplication(string applicationPath)
            {
                INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));

                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                firewallRule.ApplicationName = applicationPath;
                firewallRule.Description = "Block Internet Access for Application";
                firewallRule.Enabled = true;
                firewallRule.InterfaceTypes = "All";
                firewallRule.Name = "Block Internet Access Rule";
                firewallRule.Profiles = (int)(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL);

                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                firewallPolicy.Rules.Add(firewallRule);
            }
        }
        public void ShowPanelFromForm1(Panel panel)
        {
            // Thêm Panel từ Form1 vào Form2
            this.Controls.Add(panel);
            panel.Location = new Point(0, 0); // Đặt vị trí của Panel trên Form2
            panel.Visible = true; // Hiển thị Panel
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
           
            string processNameToCheck = "HD-Player";

            if (IsProcessRunning(processNameToCheck))
            {
                label2.Text = "Emulator Found";
            }
            else
            {
                label2.Text = "Emulator Not Found";
            }
          
          
       //     timer2.Start();
            slow();
            /*string fileUrl = "https://cdn.discordapp.com/attachments/1058655874215845999/1162981444050755584/block_internet.bat?ex=653de9df&is=652b74df&hm=7460f7ae6b4cb0cd2415d37abff785cb3c4b5bbbb8e51bbede5d5f0690cda356&";

            // Đường dẫn để lưu tệp sau khi tải về
            string savePath = "C:\\Users\\Nhan\\Documents\\New folder\\block_internet.bat";

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(fileUrl, savePath);

                }
                catch (WebException ex)
                {

                }
            }*/
            KeyAuthApp.init();
       //     timer2.Start();
            
            animationTimer.Start();
            curWidth = this.Location.X + this.Width;
            curHeight = this.Location.Y + this.Height;
            incWidth = 100;
            incHeight = 20;
            maxWidth = 2000;
            maxHeight = 1500;
            timerInterval = 100;
            timer1.Enabled = false;
            timer1.Interval = timerInterval;
            Point targetCaretPos;
            GetCaretPos(out targetCaretPos);
            password.TextChanged += (s, r) =>
            {
                Point temp;
                GetCaretPos(out temp);
                SetCaretPos(targetCaretPos.X, targetCaretPos.Y);
                targetCaretPos = temp;
            };

            Thread t = new Thread(() =>
            {
                Point current = targetCaretPos;
                while (true)
                {
                    if (current != targetCaretPos)
                    {

                        if (Math.Abs(current.X - targetCaretPos.X) + Math.Abs(current.Y - targetCaretPos.Y) > 23)
                            current = targetCaretPos;
                        else
                        {
                            current.X += Math.Sign(targetCaretPos.X - current.X);
                            current.Y += Math.Sign(targetCaretPos.Y - current.Y);
                        }


                        password.Invoke((Action)(() => SetCaretPos(current.X, current.Y)));
                    }
                    Thread.Sleep(20); // set speed effect slow or fast
                }
            });
            t.IsBackground = true;
            t.Start();


        }
        private void slow()
        {
            curWidth = this.Location.X + this.Width;
            curHeight = this.Location.Y + this.Height;
            incWidth = 100;
            incHeight = 20;
            maxWidth = 2000;
            maxHeight = 1500;
            timerInterval = 100;
            timer1.Enabled = false;
            timer1.Interval = timerInterval;
            Point targetCaretPos;
            GetCaretPos(out targetCaretPos);
            key.TextChanged += (s, r) =>
            {
                Point temp;
                GetCaretPos(out temp);
                SetCaretPos(targetCaretPos.X, targetCaretPos.Y);
                targetCaretPos = temp;
            };

            Thread t = new Thread(() =>
            {
                Point current = targetCaretPos;
                while (true)
                {
                    if (current != targetCaretPos)
                    {

                        if (Math.Abs(current.X - targetCaretPos.X) + Math.Abs(current.Y - targetCaretPos.Y) > 23)
                            current = targetCaretPos;
                        else
                        {
                            current.X += Math.Sign(targetCaretPos.X - current.X);
                            current.Y += Math.Sign(targetCaretPos.Y - current.Y);
                        }


                        key.Invoke((Action)(() => SetCaretPos(current.X, current.Y)));
                    }
                    Thread.Sleep(20); // set speed effect slow or fast
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }
       
        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            isLabelVisible = !isLabelVisible; // Thay đổi trạng thái hiển thị
            colorIndex = (colorIndex + 1) % rainbowColors.Length; // Chuyển sang màu sắc mới

            if (isLabelVisible)
            {
                transparency = 255; // Độ trong suốt rõ dần
                currentColor = rainbowColors[colorIndex]; // Màu mới
            }
            else
            {
                transparency = 100; // Độ trong suốt mờ dần
            }

            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            KeyAuthApp.register(username.Text, password.Text, key.Text);
            if (KeyAuthApp.response.success)
            {
               
            }
            else
                label2.Text = KeyAuthApp.response.message;
        }
        private int brightness = 255;

       private async void guna2Button3_Click_1(object sender, EventArgs e)
        {
            Form2 nhan = new Form2();
            nhan.Show();
            this.Hide();
        }
        private async void Bypass(object sender, EventArgs e)
        {
            string search = "0A 00 A0 E3 6E 00 54 E3 3F 00 00 13 10 8C BD E8";
            string replace = "00 F0 20 E3";
            bool k = false;

            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                label2.Text = "Emulador não encontrado";
            }
            else
            {
                MemLib.OpenProcess("HD-Player");
                label2.Text = "Aplicando...";

                int i2 = 22000000;
                IEnumerable<long> wl = await MemLib.AoBScan(search, writable: true);
                string u = "0x" + wl.FirstOrDefault().ToString("X");
                if (wl.Count() != 0)
                {
                    for (int i = 0; i < wl.Count(); i++)
                    {
                        i2++;
                        MemLib.WriteMemory(wl.ElementAt(i).ToString("X"), "bytes", replace);
                    }
                    k = true;
                }

                if (k == true)
                {
                    label2.Text = "Aplicado";
                }
                else
                {
                    label2.Text = "Falha ao aplicar";
                }
            }
        }
        public static void ToggleInternetAccess(string programPath, bool allowAccess)
        {
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Tìm luật tường lửa cho ứng dụng cụ thể
            foreach (INetFwRule rule in firewallPolicy.Rules)
            {
                if (rule.ApplicationName.Equals(programPath, StringComparison.OrdinalIgnoreCase))
                {
                    // Thay đổi trạng thái cho phép hoặc chặn kết nối Internet
                    
                    rule.Enabled = allowAccess;
                    return;
                }
            }
        }
        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string programPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // Đường dẫn đến chương trình cần chặn

            // Tạo một đối tượng Windows Firewall Policy
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Tạo một đối tượng Rule cho outbound traffic
            Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(ruleType);

            // Đặt các thuộc tính cho Rule
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK; // Hành động: Block
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT; // Hướng: Outbound
            
            firewallRule.ApplicationName = programPath; // Đường dẫn đến chương trình
            firewallRule.Enabled = true; // Kích hoạt Rule
            firewallRule.Name = "1111outbound"; // Tên Rule
            firewallRule.Description = "Block outbound traffic for " + programPath; // Mô tả Rule

            // Thêm Rule vào Firewall Rules
            fwPolicy.Rules.Add(firewallRule);
            MessageBox.Show("Rule created to block outbound traffic for " + programPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string programPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // Đường dẫn đến chương trình cần chặn

            // Tạo một đối tượng Windows Firewall Policy
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Tạo một đối tượng Rule cho outbound traffic
            Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(ruleType);

            // Đặt các thuộc tính cho Rule
            firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK; // Hành động: Block
            firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN; // Hướng: Outbound

            firewallRule.ApplicationName = programPath; // Đường dẫn đến chương trình
            firewallRule.Enabled = true; // Kích hoạt Rule
            firewallRule.Name = "1111outbound"; // Tên Rule
            firewallRule.Description = "Block outbound traffic for " + programPath; // Mô tả Rule

            // Thêm Rule vào Firewall Rules
            fwPolicy.Rules.Add(firewallRule);
            MessageBox.Show("Rule created to block outbound traffic for " + programPath);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string programPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // Đường dẫn đến chương trình cần chặn

            // Tạo một đối tượng Windows Firewall Policy
            Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(type);

            // Tạo một đối tượng Rule cho outbound traffic
            Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(ruleType);
            if (checkBox1.Checked)
            {
                

                // Đặt các thuộc tính cho Rule
                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK; // Hành động: Block
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT; // Hướng: Outbound

                firewallRule.ApplicationName = programPath; // Đường dẫn đến chương trình
                firewallRule.Enabled = true; // Kích hoạt Rule
                firewallRule.Name = "1111inbound"; // Tên Rule
                firewallRule.Description = "Block outbound traffic for " + programPath; // Mô tả Rule

                // Thêm Rule vào Firewall Rules
                fwPolicy.Rules.Add(firewallRule);


               
                MessageBox.Show("Block ok for " + programPath);

            }
            else
            {
                // Đặt các thuộc tính cho Rule
               

                firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW; // Hành động: Block
                firewallRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT; // Hướng: Outbound

                firewallRule.ApplicationName = programPath; // Đường dẫn đến chương trình
                firewallRule.Enabled = true; // Kích hoạt Rule
                firewallRule.Name = "1111outbound"; // Tên Rule
                firewallRule.Description = "Block outbound traffic for " + programPath; // Mô tả Rule

                // Thêm Rule vào Firewall Rules
                fwPolicy.Rules.Add(firewallRule);
                MessageBox.Show("Allow ok " + programPath);


            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form2 nhan = new Form2();
            nhan.Show();
            this.Hide();
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;

            foreach (Point snowflake in snowflakes)
            {
                g.FillEllipse(Brushes.MediumPurple, snowflake.X, snowflake.Y, 7, 7); // Vẽ một điểm trắng nhỏ để làm tuyết
            }
        }
        private void guna2Button1_Click_2(object sender, EventArgs e)
        {

            KeyAuthApp.login(username.Text, password.Text);
            if (KeyAuthApp.response.success)
            {
                label2.Text = "Logged";
              
                // Hiển thị Panel từ Form2 lên Form1
                username.Visible = false; password.Visible = false; key.Visible = false; guna2Button1.Visible = false; guna2Button2.Visible = false;
            }
            else
                label2.Text = KeyAuthApp.response.message;
        }
        private int snowflakeFrequency = 0;
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            // Tăng biến snowflakeFrequency sau mỗi bước thời gian
            snowflakeFrequency++;

            // Thêm tuyết mới vào danh sách theo tần số
            if (snowflakeFrequency >= 6) // Ví dụ: thêm tuyết mỗi 2 giây (30 x 0.067 giây)
            {
                int x = random.Next(0, this.Width); // Chọn một tọa độ x ngẫu nhiên
                int y = 0; // Tuyết bắt đầu từ trên cùng
                snowflakes.Add(new Point(x, y));
                snowflakeFrequency = 0; // Đặt lại biến đếm
            }

            // Xoá các điểm tuyết khi chúng ra khỏi màn hình
            snowflakes.RemoveAll(p => p.Y > this.Height);

            // Cập nhật vị trí của tuyết
            for (int i = 0; i < snowflakes.Count; i++)
            {
                int newX = snowflakes[i].X + random.Next(-1, 10); // Thay đổi x ngẫu nhiên để tạo hiệu ứng chéo
                int newY = snowflakes[i].Y + 18; // Dịch chuyển tuyết xuống
                snowflakes[i] = new Point(newX, newY);
            }

            this.Invalidate(); // Gọi để vẽ lại màn hình
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/27SKFuQvgq");
        }
    }
}
