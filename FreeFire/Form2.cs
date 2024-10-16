using Guna.UI2.WinForms;
using KeyAuth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static free.Form1;
using Memory;
using static TestAcrylicBlur.EffectBlur;
using System.Diagnostics.Eventing.Reader;
using System.Media;
using System.Windows.Forms;
using System.IO;
using DiscordRPC;
namespace Loader
{

    public partial class Form2 : Form
    {

        private List<Point> snowflakes = new List<Point>();
        private Color selectedColor = Color.Black; // Màu mặc định
        int frameCount = 0;
        DateTime lastTime = DateTime.Now;

        private Random random = new Random();
        public static api KeyAuthApp = new api(
   name: "Monster", // Application Name
    ownerid: "71vaAevlNx", // Owner ID
    secret: "24090769f625bb63feb6b906172de114595cad45fd49cf4fd34df8896d653605", // Application Secret
    version: "1.0"
);
        public Form2()
        {
            InitializeComponent();

        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {


        }
        Mem MemLib = new Mem();
        private void guna2Panel7_Paint(object sender, PaintEventArgs e)
        {

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

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private uint _blurOpacity;

        public double BlurOpacity
        {
            get { return _blurOpacity; }
            set { _blurOpacity = (uint)value; EnableBlur(); }
        }

        private uint _blurBackgroundColor = 0x990000;

        internal void EnableBlur()
        {

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;
            SetWindowCompositionAttribute(this.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        private bool isFormVisible = true;
        private void Form2_Load(object sender, EventArgs e)
        {
            DiscordRPc.rpctimestamp = Timestamps.Now;
            DiscordRPc.InitializeRPC();
            timer1.Start();
            EnableBlur();

            timer2.Start();

        }

        private int snowflakeFrequency = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Tăng biến snowflakeFrequency sau mỗi bước thời gian
            snowflakeFrequency++;

            // Thêm tuyết mới vào danh sách theo tần số
            if (snowflakeFrequency >= 4) // Ví dụ: thêm tuyết mỗi 2 giây (30 x 0.067 giây)
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
                int newX = snowflakes[i].X + random.Next(6, 7); // Thay đổi x ngẫu nhiên để tạo hiệu ứng chéo
                int newY = snowflakes[i].Y + 15; // Dịch chuyển tuyết xuống
                snowflakes[i] = new Point(newX, newY);
            }
            this.Invalidate(); // Gọi để vẽ lại màn hình
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            frameCount++;

            // Tính toán thời gian đã trôi qua
            TimeSpan elapsed = DateTime.Now - lastTime;

            if (elapsed.TotalSeconds >= 1)
            {
                double fps = frameCount / elapsed.TotalSeconds;


                // Đặt lại biến đếm
                frameCount = 0;
                lastTime = DateTime.Now;
            }
        }


        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool isSwitchEnabled = true;

        private async void guna2ToggleSwitch9_CheckedChanged(object sender, EventArgs e)
        {
            string search = "0A 00 A0 E3 6E 00 54 E3 3F 00 00 13 10 8C BD E8";
            string replace = "00 F0 20 E3";
            string search1 = "49 44 48 48 42 47 42 4E 48 4D 44";
            string replace1 = "00 00 00 00 00 00 00 00 00 00 00";
            string search2 = "0A 00 A0 E3 09 10 A0 E1 DE 06 00 EB 2C 70 99 E5";
            string replace2 = "00 F0 20 E3";
            bool k = false;

            if (Process.GetProcessesByName("HD-Player").Length == 0)
            {
                guna2HtmlLabel21.Text = "Failed to apply - Emulator Not Found";
                Console.Beep(240, 300);
            }
            else
            {
                MemLib.OpenProcess("HD-Player");
                guna2HtmlLabel21.Text = "Aplicando...";

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
                int i21 = 22000000;
                IEnumerable<long> wl1 = await MemLib.AoBScan(search1, writable: true);
                string u1 = "0x" + wl.FirstOrDefault().ToString("X");
                if (wl.Count() != 0)
                {
                    for (int i = 0; i < wl.Count(); i++)
                    {
                        i2++;
                        MemLib.WriteMemory(wl.ElementAt(i).ToString("X"), "bytes", replace1);
                    }
                    k = true;
                }
                int i22 = 22000000;
                IEnumerable<long> wl2 = await MemLib.AoBScan(search2, writable: true);
                string u2 = "0x" + wl.FirstOrDefault().ToString("X");
                if (wl.Count() != 0)
                {
                    for (int i = 0; i < wl.Count(); i++)
                    {
                        i2++;
                        MemLib.WriteMemory(wl.ElementAt(i).ToString("X"), "bytes", replace2);
                    }
                    k = true;
                }
                if (k == true)
                {
                    guna2HtmlLabel21.Text = "Anti BlackList - Aplicado";
                    Console.Beep(400, 300);

                }
                else
                {
                    guna2HtmlLabel21.Text = "Failed to apply - Try Again";
                }
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/H7zZuywc");
        }

        public void ExecuteCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();

                // Write the command to the command prompt
                process.StandardInput.WriteLine(command);
                process.StandardInput.Flush();
                process.StandardInput.Close();

                // Wait for the command to finish
                process.WaitForExit();
            }
        }
        private void guna2CustomCheckBox7_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox7.Checked)
            {
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi2\\HD-Player.exe\"");
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi2\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET BLOCKED";
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET BLOCKED";
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET BLOCKED";
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=in action=block profile=any program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");
                ExecuteCommand("netsh advfirewall firewall add rule name=\"TemporaryBlock2\" dir=out action=block profile=any program=\"C:\\Program Files\\BlueStacks\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET BLOCKED";
            }
            else
            {
                ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks_msi2\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET UNBLOCKED";
                ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks_nxt\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET UNBLOCKED";
                ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks_msi5\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET UNBLOCKED";
                ExecuteCommand("netsh advfirewall firewall delete rule name=all program=\"C:\\Program Files\\BlueStacks\\HD-Player.exe\"");
                guna2HtmlLabel21.Text = "INTERNET UNBLOCKED";
            }

        }
        public static bool Streaming;
        [DllImport("user32.dll")]
        public static extern uint SetWindowDisplayAffinity(IntPtr hwnd, uint dwAffinity);

        private void guna2CustomCheckBox8_Click(object sender, EventArgs e)
        {
            bool @checked = this.guna2CustomCheckBox8.Checked;
            if (@checked)
            {
                base.ShowInTaskbar = false;
                Form2.Streaming = true;
                Form2.SetWindowDisplayAffinity(base.Handle, 17U);
            }
            else
            {
                base.ShowInTaskbar = true;
                Form2.Streaming = false;
                Form2.SetWindowDisplayAffinity(base.Handle, 0U);
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;
        private WebClient webclient = new WebClient();
        private void guna2CustomCheckBox5_Click(object sender, EventArgs e)
        {
            string fileName = "C:\\Windows\\System32\\ChamsMenu2.0.dll";
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string adress = "https://cdn.discordapp.com/attachments/1223752265551314954/1227367432822194266/ChamsMenu.dll?ex=662825ff&is=6615b0ff&hm=42be77997e2f92c83a53ecc756bd486d91ddb3798a72ef5222a8412860adc825&";
            bool flag = File.Exists(fileName);
            if (flag)
            {
                File.Delete(fileName);
            }
            this.webclient.DownloadFile(adress, fileName);
            Process targetProcess = Process.GetProcessesByName("HD-Player")[0];
            IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            string dllName = "ChamsMenu2.0.dll";
            IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            UIntPtr bytesWritten;
            WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
            CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
        }

        private async void guna2CustomCheckBox1_Click_1(object sender, EventArgs e)
        {
            Mem memory = new Mem();
            if (guna2CustomCheckBox1.Checked)
            {
                Console.Beep();
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                memory.OpenProcess(proc);

                var result = await memory.AoBScan("00 00 00 00 00 00 A5 43 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 BF", true, true);

                if (result.Any())
                {
                    foreach (var CurrentAddress in result)
                    {
                        Int64 dopa = CurrentAddress + 0x5c;
                        Int64 dopaxd = CurrentAddress + 0x28;

                        var Read = memory.ReadMemory<int>(dopa.ToString("X"));
                        memory.WriteMemory(dopaxd.ToString("X"), "int", Read.ToString());


                    }
                    guna2HtmlLabel21.Text = "Aimbot External - Aplicado";
                    Console.Beep(400, 300);


                }
                else
                {
                    guna2HtmlLabel21.Text = "Failed to apply - Try Again";
                }
            }

        }

        private async void guna2CustomCheckBox4_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox4.Checked)
            {
                string search = "cd cc 8c 3f 8f c2 f5 3c cd cc cc 3d 07 00 00 00 00 00 00 00 00 00 00 00 00 00 f0 41 00 00 48 42 00 00 00 3f 33 33 13 40 00 00 b0 3f 00 00 80 3f 01 00 00";
                string replace = "CD CC 8C 3F 8F C2 F5 3C CD CC CC 3D 07 00 00 00 00 00 FF FF 00 00 00 00 00 00 F0 41 00 00 48 42 00 00 00 3F 33 33 13 40 00 00 B0 3F 00 00 80 3F 01 00 0A";
                bool k = false;

                if (Process.GetProcessesByName("HD-Player").Length == 0)
                {
                    guna2HtmlLabel21.Text = "Failed to apply - Emulator Not Found";
                    Console.Beep(240, 300);
                }
                else
                {
                    MemLib.OpenProcess("HD-Player");
                    guna2HtmlLabel21.Text = "Aplicando...";

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
                        guna2HtmlLabel21.Text = "Aimbot Scope AWM - Aplicado";
                        Console.Beep(400, 300);
                    }
                    else
                    {
                        guna2HtmlLabel21.Text = "Failed to apply - Try Again";
                    }
                }
            }
        }

        private async void guna2CustomCheckBox3_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox3.Checked)
            {
                string search = "00 00 00 00 3F 00 00 80 3E 00 00 00 00 05 00 00 00 00 00 80 3F 00 00 20 41 00 00 34 42 01 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F 33 33 33 3F 9A 99 99 3F 00 00 80 3F 00 00 00 00 00 00 80 3F CD CC 4C 3F";
                string replace = "00 EC 51 B8 3D 8F C2 F5 3C";
                bool k = false;

                if (Process.GetProcessesByName("HD-Player").Length == 0)
                {
                    guna2HtmlLabel21.Text = "Failed to apply - Emulator Not Found";
                    Console.Beep(240, 300);
                }
                else
                {
                    MemLib.OpenProcess("HD-Player");
                    guna2HtmlLabel21.Text = "Aplicando...";

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
                        guna2HtmlLabel21.Text = "Sniper Switch - Aplicado";
                        Console.Beep(400, 300);
                    }
                    else
                    {
                        guna2HtmlLabel21.Text = "Failed to apply - Try Again";
                    }
                }
            }
        }

        private async void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            Mem memory = new Mem();
            if (guna2CustomCheckBox2.Checked)
            {
                Console.Beep();
                Int32 proc = Process.GetProcessesByName("HD-Player")[0].Id;
                memory.OpenProcess(proc);

                var result = await memory.AoBScan("00 00 00 00 00 00 A5 43 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 BF", true, true);

                if (result.Any())
                {
                    foreach (var CurrentAddress in result)
                    {
                        Int64 dopa = CurrentAddress + 0x5c;
                        Int64 dopaxd = CurrentAddress + 0x28;

                        var Read = memory.ReadMemory<int>(dopa.ToString("X"));
                        memory.WriteMemory(dopaxd.ToString("X"), "int", Read.ToString());


                    }
                    guna2HtmlLabel21.Text = "Aimbot Drag - Aplicado";
                    Console.Beep(400, 300);


                }
                else
                {
                    guna2HtmlLabel21.Text = "Failed to apply - Try Again";
                }
            }

        }
    }
}