using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace teste4343.Views
{
    public partial class RelatoriosChamadosWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<ChamadoRelatorio> _chamados;
        private readonly SnackbarMessageQueue _messageQueue;

        public ObservableCollection<ChamadoRelatorio> Chamados
        {
            get => _chamados;
            set
            {
                _chamados = value;
                OnPropertyChanged(nameof(Chamados));
            }
        }

        public RelatoriosChamadosWindow()
        {
            InitializeComponent();
            DataContext = this;
            _messageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            snackbar.MessageQueue = _messageQueue;

            CarregarDadosIniciais();
        }

        private async void BtnAplicarFiltro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAplicarFiltro.IsEnabled = false;
                await AplicarFiltros();
                AtualizarEstatisticas();
                MostrarMensagem("Filtros aplicados com sucesso!");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao aplicar filtros: {ex.Message}", true);
            }
            finally
            {
                btnAplicarFiltro.IsEnabled = true;
            }
        }

        private async void BtnLimparFiltro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnLimparFiltro.IsEnabled = false;
                LimparFiltros();
                await CarregarChamados();
                AtualizarEstatisticas();
                MostrarMensagem("Filtros limpos com sucesso!");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao limpar filtros: {ex.Message}", true);
            }
            finally
            {
                btnLimparFiltro.IsEnabled = true;
            }
        }

        private async void BtnExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ExportarParaExcel();
                MostrarMensagem("Dados exportados para Excel com sucesso!");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao exportar para Excel: {ex.Message}", true);
            }
        }

        private async void BtnExportarPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ExportarParaPDF();
                MostrarMensagem("Dados exportados para PDF com sucesso!");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao exportar para PDF: {ex.Message}", true);
            }
        }

        private void BtnVisualizarDetalhes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is ChamadoRelatorio chamado)
                {
                    // Implementar visualização de detalhes
                    MessageBox.Show($"Detalhes do chamado: {chamado.Protocolo}");
                }
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao visualizar detalhes: {ex.Message}", true);
            }
        }

        private void BtnVisualizarHistorico_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.DataContext is ChamadoRelatorio chamado)
                {
                    // Implementar visualização de histórico
                    MessageBox.Show($"Histórico do chamado: {chamado.Protocolo}");
                }
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao visualizar histórico: {ex.Message}", true);
            }
        }

        private async void BtnGerarRelatorio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await GerarRelatorioPersonalizado();
                MostrarMensagem("Relatório gerado com sucesso!");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao gerar relatório: {ex.Message}", true);
            }
        }

        private async Task AplicarFiltros()
        {
            var departamento = (cmbDepartamento.SelectedItem as ComboBoxItem)?.Content.ToString();
            var prioridade = (cmbPrioridade.SelectedItem as ComboBoxItem)?.Content.ToString();
            var status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            var dataInicial = dtpDataInicial.SelectedDate;

            await Task.Run(() =>
            {
                var chamadosFiltrados = Chamados.Where(c =>
                    (departamento == "Todos" || c.Departamento == departamento) &&
                    (prioridade == "Todas" || c.Prioridade == prioridade) &&
                    (status == "Todos" || c.Status == status) &&
                    (!dataInicial.HasValue || c.DataAbertura >= dataInicial.Value)
                ).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Chamados = new ObservableCollection<ChamadoRelatorio>(chamadosFiltrados);
                });
            });
        }

        private void LimparFiltros()
        {
            cmbDepartamento.SelectedIndex = 0;
            cmbPrioridade.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            dtpDataInicial.SelectedDate = null;
        }

        private void AtualizarEstatisticas()
        {
            // Atualiza os números nos cards
            txtTotalChamados.Text = Chamados.Count.ToString();
            txtChamadosAbertos.Text = Chamados.Count(c => c.Status == "Aberto").ToString();

            // Calcula tempo médio de resolução
            var tempoMedio = CalcularTempoMedioResolucao();
            txtTempoMedio.Text = $"{tempoMedio:F1}h";
        }

        private double CalcularTempoMedioResolucao()
        {
            // Implementar cálculo real do tempo médio
            return 4.2; // Valor exemplo
        }

        private async Task CarregarChamados()
        {
            // TODO: Implementar carregamento real dos dados
            var chamadosExemplo = new ObservableCollection<ChamadoRelatorio>
            {
                new ChamadoRelatorio
                {
                    Protocolo = "CH001",
                    Titulo = "Problema com impressora",
                    Departamento = "TI",
                    Prioridade = "Alta",
                    Status = "Aberto",
                    DataAbertura = DateTime.Now.AddDays(-2),
                    TempoDecorrido = "2 dias"
                }
                // Adicionar mais exemplos conforme necessário
            };

            Chamados = chamadosExemplo;
        }

        private async Task ExportarParaExcel()
        {
            // TODO: Implementar exportação para Excel
            await Task.Delay(1000); // Simulação
            throw new NotImplementedException();
        }

        private async Task ExportarParaPDF()
        {
            // TODO: Implementar exportação para PDF
            await Task.Delay(1000); // Simulação
            throw new NotImplementedException();
        }

        private async Task GerarRelatorioPersonalizado()
        {
            // TODO: Implementar geração de relatório personalizado
            await Task.Delay(1000); // Simulação
            throw new NotImplementedException();
        }

        private void CarregarDadosIniciais()
        {
            try
            {
                Chamados = new ObservableCollection<ChamadoRelatorio>();
                LimparFiltros();
                CarregarChamados().ConfigureAwait(false);
                AtualizarEstatisticas();
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro ao carregar dados iniciais: {ex.Message}", true);
            }
        }

        private void MostrarMensagem(string mensagem, bool isError = false)
        {
            _messageQueue.Enqueue(
                mensagem,
                null,
                null,
                null,
                false,
                true,
                isError ? TimeSpan.FromSeconds(6) : TimeSpan.FromSeconds(3)
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ChamadoRelatorio
    {
        public string Protocolo { get; set; }
        public string Titulo { get; set; }
        public string Departamento { get; set; }
        public string Prioridade { get; set; }
        public string Status { get; set; }
        public DateTime DataAbertura { get; set; }
        public string TempoDecorrido { get; set; }
    }
}
