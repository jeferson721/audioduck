
using NAudio.CoreAudioApi;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AudioDuck
{
    public partial class Form1 : Form
    {
        
        bool rodando = false;
        float volumemestre = 0;
        string meupid = "";
        bool parar = false;
        private CancellationTokenSource cancelador = new();
        private const int atualizaçãoporsegundo = 300;

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
                cancelador?.Cancel();
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

        private async Task MetodoDeTrabalho(CancellationToken tokendecancelamento)
        {         
            while (!tokendecancelamento.IsCancellationRequested)
            {                        
     
                bool audioemalgumlugar = false;
                AudioSessionControl? Minhasession = null;

                if (!ProcessoAindaAberto()) { parar = true; break; }

                if (Minhasession != null)
                {
                    Debug.WriteLine($" VOLUME=|{Minhasession.SimpleAudioVolume.Volume}| ");


                    if (audioemalgumlugar)
                    {
                        Debug.WriteLine($" BAIXO |{volumemestre:F5}|");
                        Minhasession.SimpleAudioVolume.Volume = volumemestre;
                    }
                    else
                    {
                        Debug.WriteLine($" ALTO ");
                        Minhasession.SimpleAudioVolume.Volume = 1;
                    }
                }
                Debug.WriteLine($"--------------------------------------------------------------");
                await Task.Delay(atualizaçãoporsegundo, tokendecancelamento);
            }
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
                    try
                    {
                        cancelador = new CancellationTokenSource();
                        Task.Run(async () => await MetodoDeTrabalho(cancelador.Token));
                        rodando = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao lançar o processo: {ex.Message}");
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
            // Obtém o dispositivo de áudio padrão (saída de som) usado para multimídia
            var enumerador = new MMDeviceEnumerator();
            var dispositivo = enumerador.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            // Pega a lista de todas as sessões de áudio ativas nesse dispositivo
            var sessões = dispositivo.AudioSessionManager.Sessions;

            // Primeira passada: conta quantas sessões NÃO são do sistema
            // (exclui coisas como sons do Windows, explorer, etc)
            int tamanhodalista = 0;
            for (int i = 0; i < sessões.Count; i++)
            {
                var sessão = sessões[i];
                if ((!sessão.DisplayName.Contains("@%SystemRoot%", StringComparison.OrdinalIgnoreCase))) { tamanhodalista++; }
            }

            object[] arraydeprocessos = new object[tamanhodalista];
            int contagem = 0;

            // Segunda passada: coleta informações das sessões de aplicativos
            for (int i = 0; i < sessões.Count; i++)
            {
                var sessão = sessões[i];
                StringBuilder tag_processo = new();

                // Ignora novamente as sessões do sistema
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
