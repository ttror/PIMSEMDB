using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace teste4343.Views
{
    public partial class ExportacaoWindow : Window, INotifyPropertyChanged
    {
        private bool _incluirProtocolo = true;
        private bool _incluirTitulo = true;
        private bool _incluirDepartamento = true;
        private bool _incluirPrioridade = true;
        private bool _incluirStatus = true;
        private bool _incluirDataAbertura = true;
        private bool _incluirTempoDecorrido = true;
        private bool _incluirCabecalho = true;
        private string _formatoDataSelecionado;
        private ObservableCollection<string> _formatosDatas;

        public bool IncluirProtocolo
        {
            get => _incluirProtocolo;
            set { _incluirProtocolo = value; OnPropertyChanged(); }
        }

        public bool IncluirTitulo
        {
            get => _incluirTitulo;
            set { _incluirTitulo = value; OnPropertyChanged(); }
        }

        public bool IncluirDepartamento
        {
            get => _incluirDepartamento;
            set { _incluirDepartamento = value; OnPropertyChanged(); }
        }

        public bool IncluirPrioridade
        {
            get => _incluirPrioridade;
            set { _incluirPrioridade = value; OnPropertyChanged(); }
        }

        public bool IncluirStatus
        {
            get => _incluirStatus;
            set { _incluirStatus = value; OnPropertyChanged(); }
        }

        public bool IncluirDataAbertura
        {
            get => _incluirDataAbertura;
            set { _incluirDataAbertura = value; OnPropertyChanged(); }
        }

        public bool IncluirTempoDecorrido
        {
            get => _incluirTempoDecorrido;
            set { _incluirTempoDecorrido = value; OnPropertyChanged(); }
        }

        public bool IncluirCabecalho
        {
            get => _incluirCabecalho;
            set { _incluirCabecalho = value; OnPropertyChanged(); }
        }

        public string FormatoDataSelecionado
        {
            get => _formatoDataSelecionado;
            set { _formatoDataSelecionado = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> FormatosDatas
        {
            get => _formatosDatas;
            set { _formatosDatas = value; OnPropertyChanged(); }
        }

        public ICommand ExportarCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }

        public ExportacaoWindow(TipoExportacao tipo)
        {
            InitializeComponent();
            DataContext = this;
            InicializarComandos();
            CarregarFormatosDatas();
            Title = $"Exportar para {tipo}";
        }

        private void InicializarComandos()
        {
            ExportarCommand = new RelayCommand(Exportar, PodeExportar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private void CarregarFormatosDatas()
        {
            FormatosDatas = new ObservableCollection<string>
            {
                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm",
                "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm"
            };
            FormatoDataSelecionado = FormatosDatas[0];
        }

        private bool PodeExportar()
        {
            return IncluirProtocolo || IncluirTitulo || IncluirDepartamento ||
                   IncluirPrioridade || IncluirStatus || IncluirDataAbertura ||
                   IncluirTempoDecorrido;
        }

        private void Exportar()
        {
            try
            {
                // TODO: Implementar lógica de exportação
                MessageBox.Show("Exportação concluída com sucesso!", "Sucesso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar()
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public enum TipoExportacao
        {
            Excel,
            PDF
        }
    }
}
