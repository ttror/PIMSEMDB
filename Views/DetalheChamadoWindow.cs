using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace teste4343.Views
{
    public partial class NovoChamadoWindow : Window, INotifyPropertyChanged
    {
        private string _titulo;
        private string _departamentoSelecionado;
        private string _prioridadeSelecionada;
        private string _descricao;
        private ObservableCollection<string> _departamentos;
        private ObservableCollection<string> _prioridades;
        private ObservableCollection<AnexoModel> _anexos;

        public string Titulo
        {
            get => _titulo;
            set { _titulo = value; OnPropertyChanged(); }
        }

        public string DepartamentoSelecionado
        {
            get => _departamentoSelecionado;
            set { _departamentoSelecionado = value; OnPropertyChanged(); }
        }

        public string PrioridadeSelecionada
        {
            get => _prioridadeSelecionada;
            set { _prioridadeSelecionada = value; OnPropertyChanged(); }
        }

        public string Descricao
        {
            get => _descricao;
            set { _descricao = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Departamentos
        {
            get => _departamentos;
            set { _departamentos = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Prioridades
        {
            get => _prioridades;
            set { _prioridades = value; OnPropertyChanged(); }
        }

        public ObservableCollection<AnexoModel> Anexos
        {
            get => _anexos;
            set { _anexos = value; OnPropertyChanged(); }
        }

        public ICommand SalvarCommand { get; private set; }
        public ICommand CancelarCommand { get; private set; }
        public ICommand AdicionarAnexoCommand { get; private set; }
        public ICommand RemoverAnexoCommand { get; private set; }

        public NovoChamadoWindow()
        {
            InitializeComponent();
            DataContext = this;
            InicializarComandos();
            CarregarDados();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void InicializarComandos()
        {
            SalvarCommand = new RelayCommand(Salvar, PodeSalvar);
            CancelarCommand = new RelayCommand(Cancelar);
            AdicionarAnexoCommand = new RelayCommand(AdicionarAnexo);
            RemoverAnexoCommand = new RelayCommand<AnexoModel>(RemoverAnexo);
        }

        private void CarregarDados()
        {
            Departamentos = new ObservableCollection<string>
            {
                "TI",
                "RH",
                "Financeiro",
                "Comercial",
                "Suporte"
            };

            Prioridades = new ObservableCollection<string>
            {
                "Baixa",
                "Média",
                "Alta",
                "Crítica"
            };

            Anexos = new ObservableCollection<AnexoModel>();
        }

        private bool PodeSalvar()
        {
            return !string.IsNullOrWhiteSpace(Titulo) &&
                   !string.IsNullOrWhiteSpace(DepartamentoSelecionado) &&
                   !string.IsNullOrWhiteSpace(PrioridadeSelecionada) &&
                   !string.IsNullOrWhiteSpace(Descricao);
        }

        private void Salvar()
        {
            try
            {
                // TODO: Implementar lógica de salvamento no banco de dados
                var novoChamado = new ChamadoModel
                {
                    Titulo = Titulo,
                    Departamento = DepartamentoSelecionado,
                    Prioridade = PrioridadeSelecionada,
                    Descricao = Descricao,
                    DataAbertura = DateTime.Now,
                    Status = "Aberto"
                };

                // Simular salvamento
                MessageBox.Show("Chamado criado com sucesso!", "Sucesso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar chamado: {ex.Message}", "Erro",
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

        private void AdicionarAnexo()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "Todos os arquivos (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    Anexos.Add(new AnexoModel
                    {
                        Nome = System.IO.Path.GetFileName(filename),
                        Caminho = filename
                    });
                }
            }
        }

        private void RemoverAnexo(AnexoModel anexo)
        {
            if (anexo != null)
            {
                Anexos.Remove(anexo);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AnexoModel
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
    }

    public class ChamadoModel
    {
        public string Titulo { get; set; }
        public string Departamento { get; set; }
        public string Prioridade { get; set; }
        public string Descricao { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Status { get; set; }
    }
}
