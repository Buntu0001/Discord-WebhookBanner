using System;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
namespace WebhookManage
{
    public partial class Form2 : Form
    {
        private Form1 _frm;
        private string _value;
        public Form2(Form1 frm, string value)
        {
            InitializeComponent();
            _frm = frm;
            _value = value;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Encoding.Default.GetString(Convert.FromBase64String(_value));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(richTextBox1.Text)))
            {
                _frm.msgTransfer(richTextBox1.Text);
                this.Close();
            }
            else
            {
                this.Close();
                SystemSounds.Beep.Play();
                MessageBox.Show("빈칸이 없도록 해주세요.", "WebhookManage", MessageBoxButtons.OK);
            }
        }

        private Point mousePoint;
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
    }
}
