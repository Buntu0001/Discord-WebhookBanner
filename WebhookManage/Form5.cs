using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebhookManage
{
    public partial class Form5 : Form
    {
        private Form1 _frm;
        private List<Form1.Webhook> _list;
        private int _index;
        public Form5(Form1 frm, List<Form1.Webhook> list, int index)
        {
            InitializeComponent();
            _frm = frm;
            _list = list;
            _index = index;
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            textBox3.Text = _list[_index].name;
            textBox1.Text = _list[_index].WebHook;
            richTextBox1.Text = Encoding.Default.GetString(Convert.FromBase64String(_list[_index].Message));
            textBox2.Text = _list[_index].interval;
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrWhiteSpace(textBox1.Text)) && !(String.IsNullOrWhiteSpace(richTextBox1.Text)) && !(String.IsNullOrWhiteSpace(textBox2.Text)) && !(String.IsNullOrWhiteSpace(textBox3.Text)))
            {
                _list[_index].name = textBox3.Text;
                _list[_index].WebHook = textBox1.Text;
                _list[_index].Message = Convert.ToBase64String(Encoding.Default.GetBytes(richTextBox1.Text));
                _list[_index].interval = textBox2.Text;
                _frm.hookReload();
                _frm.setList();
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
        private void Form5_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form5_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
    }
}
