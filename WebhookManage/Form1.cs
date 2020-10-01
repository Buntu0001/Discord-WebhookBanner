using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace WebhookManage
{
    public partial class Form1 : Form
    {
        private static Random random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        public static string setting = Application.StartupPath + @"\setting.inf";
        FileInfo settingFile = new FileInfo(setting);
        public static string hookPath = Application.StartupPath + @"\WebHook.inf";
        FileInfo hookFile = new FileInfo(hookPath);
        public string Main_Message = "QnVudHUgV2ViaG9vayBNYW5hZ2Uh";
        public string Main_Interval = "10";
        public static string Main_name = "! BUNTU";
        public static string Main_image = @"https://thumbs.gfycat.com/InfatuatedSlimCicada-max-1mb.gif";
        public Form1()
        {
            InitializeComponent();
        }
        private static string RandomString(int _nLength = 7)
        {
            const string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chRandom = new char[_nLength];

            for (int i = 0; i < _nLength; i++)
            {
                chRandom[i] = strPool[random.Next(strPool.Length)];
            }
            string strRet = new String(chRandom);
            return strRet;
        }
        public void settingReload()
        {
            if (!(settingFile.Exists))
            {
                string[] write = { Main_Message, Main_Interval, Main_name, Main_image };
                System.IO.File.WriteAllLines(setting, write);
            }
            else if (settingFile.Exists)
            {
                settingFile.Delete();
                string[] write = { Main_Message, Main_Interval, Main_name, Main_image };
                System.IO.File.WriteAllLines(setting, write);
            }
        }
        public void hookReload()
        {
            if (!(hookFile.Exists))
            {
                string[] hookArr = new string[hookList.Count];
                for (int i = 0; i < hookList.Count; i++)
                {
                    hookArr[i] = hookList[i].name + "|||" + hookList[i].WebHook + "|||" + hookList[i].Message + "|||" + hookList[i].interval;
                }
                System.IO.File.WriteAllLines(hookPath, hookArr);
            }
            else if (hookFile.Exists)
            {
                hookFile.Delete();
                string[] hookArr = new string[hookList.Count];
                for (int i = 0; i < hookList.Count; i++)
                {
                    hookArr[i] = hookList[i].name + "|||" + hookList[i].WebHook + "|||" + hookList[i].Message + "|||" + hookList[i].interval;
                }
                System.IO.File.WriteAllLines(hookPath, hookArr);
            }
        }
        public void msgTransfer(string value)
        {
            if (value != null)
            {
                Main_Message = Convert.ToBase64String(Encoding.Default.GetBytes(value));
                settingReload();
            }

        }
        public void intervalTransfer(string value)
        {
            if (value != null)
            {
                Main_Interval = value;
                settingReload();
            }
        }
        public void nameTransfer(string value)
        {
            if (value != null)
            {
                Main_name = value;
                settingReload();
            }
        }
        public void imageTransfer(string value)
        {
            if (value != null)
            {
                Main_image = value;
                settingReload();
            }
        }
        public void webhookTransfer(string value, string value2)
        {
            if (value != null)
            {
                addList(value2, value, Main_Message, Main_Interval);
                hookReload();
                setList();
            }
        }
        public void looping(List<Webhook> value, int check)
        {
            while (true)
            {
                sendWebhook(value[check].WebHook, value[check].Message, check, value);
                Thread.Sleep(Convert.ToInt32(value[check].interval) * 60000);
            }
        }
        public void sendWebhook(string url, string msg, int i, List<Webhook> value)
        {
            try
            {
                HttpClient client = new HttpClient();
                Dictionary<string, string> contents = new Dictionary<string, string>
            {
            { "content", Encoding.Default.GetString(Convert.FromBase64String(msg)) },
            { "username", Main_name },
            { "avatar_url", Main_image }
            };
                client.PostAsync(url, new FormUrlEncodedContent(contents)).GetAwaiter().GetResult();
                value[i].time = DateTime.Now.ToString("MM-dd HH:mm:ss");
                this.Invoke(new Action(
                delegate ()
                {
                   setList();
                }));
            }
            catch
            {
                value[i].time = "전송실패";
                this.Invoke(new Action(
                delegate ()
                {
                    setList();
                }));
                hookList[i].check.Abort();
            }
        }
        private static List<Webhook> hookList = new List<Webhook>();
        public void addList(string _name, string url, string msg, string interval)
        {
            hookList.Add(new Webhook { name = _name, WebHook = url, Message = msg, interval = interval });
        }
        public void setList()
        {
            listView1.Items.Clear();

            foreach (var list in hookList)
            {
                var row = new string[] { list.name, Encoding.Default.GetString(Convert.FromBase64String(list.Message)), list.interval + "분", list.time };
                var item = new ListViewItem(row);
                listView1.Items.Add(item);
                if (list.alloc != "buntu")
                {
                    list.alloc = "buntu";
                    ThreadStart starter = delegate { looping(hookList, hookList.IndexOf(list)); };
                    var thread = new Thread(starter);
                    list.check = thread;
                    thread.Start();
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
            MessageBox.Show("Made By BUNTU", "WebhookManage", MessageBoxButtons.OK);
            if (settingFile.Exists)
            {
                try
                {
                    string chk = System.IO.File.ReadAllText(setting);
                    if (!(String.IsNullOrWhiteSpace(chk)))
                    {
                        string[] read = System.IO.File.ReadAllLines(setting);
                        if (String.IsNullOrWhiteSpace(read[0]) || String.IsNullOrWhiteSpace(read[1]) || String.IsNullOrWhiteSpace(read[2]) || String.IsNullOrWhiteSpace(read[3]))
                        {
                            string random = RandomString();
                            string path = @"\setting_bak_" + random + @".inf";
                            System.IO.File.Copy(setting, Application.StartupPath + path);
                            System.IO.File.Delete(setting);
                            string[] write = { Main_Message, Main_Interval, Main_name, Main_image };
                            System.IO.File.WriteAllLines(setting, write);
                            SystemSounds.Beep.Play();
                            MessageBox.Show("세팅 저장파일이 올바르지 않습니다. " + Application.StartupPath + path + " 에 파일이 백업되었습니다.", "WebhookManage", MessageBoxButtons.OK);
                        }
                        else
                        {
                            Main_Message = read[0];
                            Main_Interval = read[1];
                            Main_name = read[2];
                            Main_image = read[3];
                        }
                    }
                }
                catch
                {
                    string random = RandomString();
                    string path = @"\setting_bak_" + random + @".inf";
                    System.IO.File.Copy(setting, Application.StartupPath + path);
                    System.IO.File.Delete(setting);
                    string[] write = { Main_Message, Main_Interval, Main_name, Main_image };
                    System.IO.File.WriteAllLines(setting, write);
                    SystemSounds.Beep.Play();
                    MessageBox.Show("세팅 저장파일이 올바르지 않습니다. " + Application.StartupPath + path + " 에 파일이 백업되었습니다.", "WebhookManage", MessageBoxButtons.OK);
                }
            }
            else if (!(settingFile.Exists))
            {
                string[] write = { Main_Message, Main_Interval, Main_name, Main_image };
                System.IO.File.WriteAllLines(setting, write);
            }
            if (hookFile.Exists)
            {
                try
                {
                    string chk = System.IO.File.ReadAllText(hookPath);
                    if (!(String.IsNullOrWhiteSpace(chk)))
                    {
                        string[] hookArr = System.IO.File.ReadAllLines(hookPath);
                        for (int i = 0; i < hookArr.Length; i++)
                        {
                            string[] temp = new string[3];
                            string[] sp = { "|||" };
                            temp = hookArr[i].Split(sp, StringSplitOptions.None);
                            addList(temp[0], temp[1], temp[2], temp[3]);
                            hookList[i].time = "보내지않음";
                        }
                        setList();
                    }
                }
                catch
                {
                    string random = RandomString();
                    string path = @"\WebHook_bak_" + random + @".inf";
                    System.IO.File.Copy(hookPath, Application.StartupPath + path);
                    System.IO.File.Delete(hookPath);
                    System.IO.File.WriteAllText(hookPath, "");
                    SystemSounds.Beep.Play();
                    MessageBox.Show("웹훅 저장파일이 올바르지 않습니다. " + Application.StartupPath + path + " 에 파일이 백업되었습니다.", "WebhookManage", MessageBoxButtons.OK);
                }
            }
            else if (!(hookFile.Exists))
            {
                System.IO.File.WriteAllText(hookPath, "");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var list in hookList)
            {
                list.check.Abort();
            }
            Application.Exit();
        }
        private Point mousePoint;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 fm2 = new Form2(this, Main_Message);
            fm2.ShowDialog();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(this);
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 frm = new Form4(this, Main_Interval);
            frm.ShowDialog();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Form6 frm = new Form6(this, Main_name);
            frm.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form7 frm = new Form7(this, Main_image);
            frm.ShowDialog();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = listView1.SelectedItems[0].Index;
                Form5 frm = new Form5(this, hookList, i);
                frm.ShowDialog();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int i = listView1.SelectedItems[0].Index;
                foreach (var list in hookList)
                {
                    list.check.Abort();
                    list.alloc = "photo";
                }
                hookList[i].check = null;
                hookList[i].alloc = null;
                hookList[i].name = null;
                hookList[i].WebHook = null;
                hookList[i].Message = null;
                hookList[i].interval = null;
                hookList[i].time = null;
                hookList.RemoveAt(i);
                setList();
                hookReload();
            }
        }
        public class Webhook
        {
            public string name { get; set; }
            public string WebHook { get; set; }
            public string Message { get; set; }
            public string interval { get; set; }
            public string alloc { get; set; }
            public string time { get; set; }
            public Thread check { get; set; }
        }
    }
}
