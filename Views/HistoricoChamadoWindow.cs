using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace teste4343.Views
{
    public partial class HistoricoChamadoWindow : Window, INotifyPropertyChanged
    {
        private string _protocolo;
        private ObservableCollection<EventoHistoricoModel> _eventosHistorico;

        public string Protocolo
        {
            get => _protocolo;
            set { _protocolo = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EventoHistoricoModel> EventosHistorico
        {
            get => _eventosHistorico;
            set { _eventosHistorico = value; OnPropertyChanged(); }
        }

        public HistoricoChamadoWindow(string protocolo)
        {
            InitializeComponent();
            DataContext = this;
            Protocolo = protocolo;
            CarregarHistorico();
        }

        private void CarregarHistorico()
        {
            EventosHistorico = new ObservableCollection<EventoHistoricoModel>();

            // TODO: Carregar histórico real do banco de dados
            // Exemplo de dados
            EventosHistorico.Add(new EventoHistoricoModel
            {
                Data = DateTime.Now.AddDays(-2),
                Usuario = "Sistema",
                TipoEvento = "Abertura do Chamado",
                Descricao = "Chamado criado",
                Alteracoes = new ObservableCollection<AlteracaoModel>()
            });

            EventosHistorico.Add(new EventoHistoricoModel
            {
                Data = DateTime.Now.AddDays(-1),
                Usuario = "João Silva",
                TipoEvento = "Atualização de Status",
                Descricao = "Status alterado",
                Alteracoes = new ObservableCollection<AlteracaoModel>
                {
                    new AlteracaoModel
                    {
                        Campo = "Status",
                        ValorAntigo = "Aberto",
                        ValorNovo = "Em Andamento"
                    }
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EventoHistoricoModel
    {
        public DateTime Data { get; set; }
        public string Usuario { get; set; }
        public string TipoEvento { get; set; }
        public string Descricao { get; set; }
        public ObservableCollection<AlteracaoModel> Alteracoes { get; set; }
        public bool TemAlteracoes => Alteracoes?.Count > 0;
    }

    public class AlteracaoModel
    {
        public string Campo { get; set; }
        public string ValorAntigo { get; set; }
        public string ValorNovo { get; set; }
    }
}
