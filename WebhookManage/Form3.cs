using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
namespace WebhookManage
{
    public partial class Form3 : Form
    {
        private Form1 _frm = new Form1();
        public Form3(Form1 frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(textBox1.Text)) && !(String.IsNullOrWhiteSpace(textBox2.Text)))
            {
                _frm.webhookTransfer(textBox1.Text, textBox2.Text);
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
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
    }
}
