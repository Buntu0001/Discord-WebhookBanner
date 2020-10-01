using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace WebhookManage
{
    public partial class Form6 : Form
    {
        private Form1 _frm = new Form1();
        private string _value;
        public Form6(Form1 frm, string value)
        {
            InitializeComponent();
            _frm = frm;
            _value = value;
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            textBox1.Text = _value;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(textBox1.Text)))
            {
                _frm.nameTransfer(textBox1.Text);
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
        private void Form6_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form6_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
    }
}
