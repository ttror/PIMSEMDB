using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace teste4343.Views
{
    public partial class ConfiguracoesWindow : Window, INotifyPropertyChanged
    {
        private string _temaSelecionado;
        private string _idiomaSelecionado;
        private bool _notificacoesDesktop;
        private bool _notificacoesEmail;
        private string _emailNotificacoes;
        private bool _backupAutomatico;
        private string _frequenciaBackupSelecionada;
        private string _diretorioBackup;
        private ObservableCollection<string> _temas;
        private ObservableCollection<string> _idiomas;
        private ObservableCollection<string> _frequenciasBackup;
        private ObservableCollection<DepartamentoModel> _departamentos;

        public string TemaSelecionado
        {
            get => _temaSelecionado;
            set { _temaSelecionado = value; OnPropertyChanged(); }
        }

        public string IdiomaSelecionado
        {
            get => _idiomaSelecionado;
            set { _idiomaSelecionado = value; OnPropertyChanged(); }
        }

        public bool NotificacoesDesktop
        {
            get => _notificacoesDesktop;
            set { _notificacoesDesktop = value; OnPropertyChanged(); }
        }

        public bool NotificacoesEmail
        {
            get => _notificacoesEmail;
            set { _notificacoesEmail = value; OnPropertyChanged(); }
        }

        public string EmailNotificacoes
        {
            get => _emailNotificacoes;
            set { _emailNotificacoes = value; OnPropertyChanged(); }
        }

        public bool BackupAutomatico
        {
            get => _backupAutomatico;
            set { _backupAutomatico = value; OnPropertyChanged(); }
        }

        public string FrequenciaBackupSelecionada
        {
            get => _frequenciaBackupSelecionada;
            set { _frequenciaBackupSelecionada = value; OnPropertyChanged(); }
        }

        public string DiretorioBackup
        {
            get => _diretorioBackup;
            set { _diretorioBackup = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Temas
        {
            get => _temas;
            set { _temas = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Idiomas
        {
            get => _idiomas;
            set { _idiomas = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> FrequenciasBackup
        {
            get => _frequenciasBackup;
            set { _frequenciasBackup = value; OnPropertyChanged(); }
        }

        public ObservableCollection<DepartamentoModel> Departamentos
        {
            get => _departamentos;
            set { _departamentos = value; OnPropertyChanged(); }
        }

        public ICommand SalvarCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }
        public ICommand RestaurarPadroesCommand { get; private set; }

        public ConfiguracoesWindow()
        {
            InitializeComponent();
            DataContext = this;
            InicializarComandos();
            CarregarDados();
        }

        private void InicializarComandos()
        {
            SalvarCommand = new RelayCommand(Salvar);
            CancelarCommand = new RelayCommand(Cancelar);
            RestaurarPadroesCommand = new RelayCommand(RestaurarPadroes);
        }

        private void CarregarDados()
        {
            Temas = new ObservableCollection<string>
            {
                "Claro",
                "Escuro",
                "Alto Contraste"
            };

            Idiomas = new ObservableCollection<string>
            {
                "Português (Brasil)",
                "English",
                "Español"
            };

            FrequenciasBackup = new ObservableCollection<string>
            {
                "Diário",
                "Semanal",
                "Mensal"
            };

            Departamentos = new ObservableCollection<DepartamentoModel>
            {
                new DepartamentoModel { Nome = "TI", Responsavel = "João Silva", Email = "ti@empresa.com" },
                new DepartamentoModel { Nome = "RH", Responsavel = "Maria Santos", Email = "rh@empresa.com" }
            };

            // Carregar configurações salvas
            CarregarConfiguracoesSalvas();
        }

        private void CarregarConfiguracoesSalvas()
        {
            // TODO: Implementar carregamento das configurações do banco de dados
            TemaSelecionado = "Claro";
            IdiomaSelecionado = "Português (Brasil)";
            NotificacoesDesktop = true;
            NotificacoesEmail = false;
            EmailNotificacoes = "";
            BackupAutomatico = true;
            FrequenciaBackupSelecionada = "Diário";
            DiretorioBackup = @"C:\Backup";
        }

        private void Salvar()
        {
            try
            {
                // TODO: Implementar salvamento das configurações
                MessageBox.Show("Configurações salvas com sucesso!", "Sucesso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar configurações: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar()
        {
            if (MessageBox.Show("Deseja realmente cancelar? Todas as alterações serão perdidas.",
                "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }

        private void RestaurarPadroes()
        {
            if (MessageBox.Show("Deseja realmente restaurar as configurações padrão?",
                "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CarregarConfiguracoesPadrao();
            }
        }

        private void CarregarConfiguracoesPadrao()
        {
            TemaSelecionado = "Claro";
            IdiomaSelecionado = "Português (Brasil)";
            NotificacoesDesktop = true;
            NotificacoesEmail = false;
            EmailNotificacoes = "";
            BackupAutomatico = true;
            FrequenciaBackupSelecionada = "Diário";
            DiretorioBackup = @"C:\Backup";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DepartamentoModel : INotifyPropertyChanged
    {
        private string _nome;
        private string _responsavel;
        private string _email;

        public string Nome
        {
            get => _nome;
            set { _nome = value; OnPropertyChanged(); }
        }

        public string Responsavel
        {
            get => _responsavel;
            set { _responsavel = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
