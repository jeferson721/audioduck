using System.Data;
using System.Diagnostics;
using System.Text;
using NAudio.CoreAudioApi;

namespace AudioDuck
{
    public partial class Form1 : Form
    {
        bool rodando = false;
        float volumemestre = 0;
        string meupid = "";
        bool parar = false;

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

        private bool ProcessoAindaAberto()
        {
            string s = meupid;
            string numeroStr = s.Split(':')[1].Split('_')[0];
            uint pid = uint.Parse(numeroStr);

            try
            {
                var proc = System.Diagnostics.Process.GetProcessById((int)pid);
                return !proc.HasExited;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        void Parada()
        {
            if (!rodando) return;
            comboBox1.SelectedItem = null;
            comboBox1.Items.Clear();
            meupid = "";
            try
            {              
                rodando = false;
                MessageBox.Show(" Ducking parado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao parar : {ex.Message}");
                rodando = true;
            }
            AtualizaCor();
            parar = false;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            meupid = "";
            if (comboBox1.SelectedItem != null)
            {
                string? itemSelecionado = comboBox1.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(itemSelecionado))
                {
                    meupid = itemSelecionado;
                    //rodando = true;
                    if (ProcessoAindaAberto())
                    {
                        rodando = true;
                    }
                    else
                    {
                        rodando = false;
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum item selecionado");
                    // rodando = false;
                }
            }
            else
            {
                MessageBox.Show("Nenhum item selecionado");
                //rodando = false;
            }
          

            AtualizaCor();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            parar = true;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            var enumerador = new MMDeviceEnumerator();
            var dispositivo = enumerador.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var sessões = dispositivo.AudioSessionManager.Sessions;
            int tamanhodalista = 0;

            for (int i = 0; i < sessões.Count; i++)
            {
                var sessão = sessões[i];
                if ((!sessão.DisplayName.Contains("@%SystemRoot%", StringComparison.OrdinalIgnoreCase))) { tamanhodalista++; }
            }

            object[] arraydeprocessos = new object[tamanhodalista];
            int contagem = 0;

            for (int i = 0; i < sessões.Count; i++)
            {
                var sessão = sessões[i];
                StringBuilder tag_processo = new();

                if ((!sessão.DisplayName.Contains("@%SystemRoot%", StringComparison.OrdinalIgnoreCase)))
                {
                    int processoId = (int)sessão.GetProcessID;
                    try
                    {
                        Process processo = Process.GetProcessById(processoId);
                        tag_processo.Append($"{processo.ProcessName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro: {ex.Message}");
                    }
                    tag_processo.Append($" {sessão.DisplayName} ");
                    tag_processo.Append($" ID:{sessão.GetProcessID}");
                    arraydeprocessos[contagem++] = tag_processo;
                }
            }

            comboBox1.Items.AddRange(arraydeprocessos);
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
            volumemestre = trackBar1.Value / 100;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (parar)
            {
                Parada();
            }
        }
    }
}
