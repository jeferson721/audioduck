using System.Diagnostics;

namespace AudioDuck
{
    public partial class Form1 : Form
    {
        bool rodando = false;
        float volumm = 0;

        private void AtualizaCor()
        {
            if (rodando)
            {
                button1.BackColor = Color.PaleGreen;
                button2.BackColor = Color.LightCoral;
            }
            else
            {
                button1.BackColor = Color.Gainsboro;
                button2.BackColor = Color.Gainsboro;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            rodando = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            rodando = false;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {

        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {           
            float fd = trackBar1.Value;
            float v = fd / 100;   
            label3.Text = fd.ToString();
            volumm = v;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            AtualizaCor();
        }
    }
}
