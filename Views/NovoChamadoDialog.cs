using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using teste4343.Models;

namespace teste4343.Views
{
    public class Anexo
    {
        public string NomeArquivo { get; set; }
        public string CaminhoCompleto { get; set; }
        public string Tamanho { get; set; }
        public DateTime DataAnexo { get; set; }

        public Anexo(string caminhoArquivo)
        {
            CaminhoCompleto = caminhoArquivo;
            NomeArquivo = Path.GetFileName(caminhoArquivo);

            var fileInfo = new FileInfo(caminhoArquivo);
            Tamanho = FormatarTamanhoArquivo(fileInfo.Length);
            DataAnexo = DateTime.Now;
        }

        private string FormatarTamanhoArquivo(long bytes)
        {
            string[] sufixos = { "B", "KB", "MB", "GB", "TB" };
            int contador = 0;
            double tamanho = bytes;

            while (tamanho >= 1024 && contador < sufixos.Length - 1)
            {
                tamanho /= 1024;
                contador++;
            }

            return $"{tamanho:0.##} {sufixos[contador]}";
        }
    }

    public partial class NovoChamadoDialog : UserControl
    {
        public Chamado NovoChamado { get; private set; }
        private ObservableCollection<Anexo> _listaAnexos = new ObservableCollection<Anexo>();

        public NovoChamadoDialog()
        {
            InitializeComponent();

            // Inicializar os ComboBox com valores padrão
            if (CbPrioridade != null && CbPrioridade.Items.Count > 0)
                CbPrioridade.SelectedIndex = 1; // Seleciona "Média" por padrão

            if (CbCategoria != null && CbCategoria.Items.Count > 0)
                CbCategoria.SelectedIndex = 0; // Seleciona "Hardware" por padrão

            // Configurar a lista de anexos para o ItemsControl
            if (ListaAnexos != null)
                ListaAnexos.ItemsSource = _listaAnexos;
        }

        // Evento do botão "SALVAR"
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(TxtTitulo.Text))
            {
                MessageBox.Show("Por favor, preencha o título do chamado.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Criar o objeto Chamado com os dados do formulário
            NovoChamado = new Chamado
            {
                Id = GerarNovoId(),
                Titulo = TxtTitulo.Text,
                Descricao = TxtDescricao.Text,
                Prioridade = CbPrioridade.SelectedItem != null ?
                    ((ComboBoxItem)CbPrioridade.SelectedItem).Content.ToString() : "Média",
                Status = "Aberto", // Status padrão para novos chamados
                Categoria = CbCategoria.SelectedItem != null ?
                    ((ComboBoxItem)CbCategoria.SelectedItem).Content.ToString() : "Hardware",
                Responsavel = TxtResponsavel.Text,
                Departamento = TxtDepartamento.Text,
                DataCriacao = DateTime.Now
            };

            // Adicionar os anexos ao chamado
            foreach (var anexo in _listaAnexos)
            {
                // Convertendo o anexo da Views para o anexo do Models
                var anexoModel = GetAnexoModel(anexo);

                NovoChamado.Anexos.Add(anexoModel);
            }

            // Fecha a janela pai com resultado positivo
            Window.GetWindow(this).DialogResult = true;
        }

        private static Models.Anexo GetAnexoModel(Anexo anexo) => new()
        {
            Nome = anexo.NomeArquivo,
            Caminho = anexo.CaminhoCompleto,
            DataAnexo = anexo.DataAnexo
        };

        // Método para gerar um novo ID
        private int GerarNovoId()
        {
            return (int)(DateTime.Now.Ticks % 10000);
        }

        // Evento do botão "CANCELAR"
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Fecha a janela pai com resultado negativo
            Window.GetWindow(this).DialogResult = false;
        }

        // Método para anexar um arquivo
        public void BtnAnexar_Click(object sender, RoutedEventArgs e)
        {
            // Criar um diálogo de seleção de arquivo
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecionar Arquivo",
                Multiselect = false
            };

            // Mostrar o diálogo e verificar se o usuário selecionou um arquivo
            if (openFileDialog.ShowDialog() == true)
            {
                string caminhoArquivo = openFileDialog.FileName;

                // Criar um novo anexo
                var novoAnexo = new Anexo(caminhoArquivo);

                // Adicionar o anexo à coleção
                _listaAnexos.Add(novoAnexo);
            }
        }

        // Método para remover um anexo selecionado
        public void BtnRemoverAnexo_Click(object sender, RoutedEventArgs e)
        {
            // No XAML, o botão tem um Tag que contém o objeto Anexo
            if (sender is Button button && button.Tag is Anexo anexoParaRemover)
            {
                _listaAnexos.Remove(anexoParaRemover);
            }
        }
    }
}
