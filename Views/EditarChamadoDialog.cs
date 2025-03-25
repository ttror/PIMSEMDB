using System.Windows;
using System.Windows.Controls;
using teste4343.Models;

namespace teste4343.Views
{
    public partial class EditarChamadoDialog : UserControl
    {
        // Propriedade para armazenar o chamado editado
        public Chamado ChamadoEditado { get; private set; }

        // Chamado original para referência
        private readonly Chamado _chamadoOriginal;

        public EditarChamadoDialog(Chamado chamadoParaEditar)
        {
            InitializeComponent();

            // Verificar se o chamado é válido
            if (chamadoParaEditar == null)
            {
                MessageBox.Show("Erro: Chamado inválido para edição.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Armazenar referência ao chamado original
            _chamadoOriginal = chamadoParaEditar;

            // Inicializar o objeto que armazenará as alterações
            ChamadoEditado = new Chamado
            {
                Id = chamadoParaEditar.Id
            };

            // Preencher campos com os valores do chamado
            PreencherCampos(chamadoParaEditar);
        }

        // Método para preencher os campos do formulário com os dados do chamado
        private void PreencherCampos(Chamado chamado)
        {
            // Preencher campos básicos
            IdTextBox.Text = chamado.Id.ToString();
            TituloTextBox.Text = chamado.Titulo;
            DescricaoTextBox.Text = chamado.Descricao;
            ResponsavelTextBox.Text = chamado.Responsavel;

            // Datas
            DataCriacaoPicker.SelectedDate = chamado.DataCriacao;

            // A data de atualização será sempre a data atual
            DataAtualizacaoPicker.SelectedDate = DateTime.Now;
            DataAtualizacaoPicker.IsEnabled = false; // Desabilitar edição da data de atualização

            // Comentários
            ComentariosTextBox.Text = chamado.Comentarios;

            // Selecionar o status correto
            SelecionarStatus(chamado.Status);

            // Selecionar a prioridade correta
            SelecionarPrioridade(chamado.Prioridade);

            // Adicionar campos específicos para o departamento e categoria se existirem no XAML
            AdicionarCamposEspecificos(chamado);
        }

        // Método para selecionar o status correto no ComboBox
        private void SelecionarStatus(string status)
        {
            switch (status)
            {
                case "Aberto":
                    StatusComboBox.SelectedIndex = 0;
                    break;
                case "Em Andamento":
                    StatusComboBox.SelectedIndex = 1;
                    break;
                case "Fechado":
                    StatusComboBox.SelectedIndex = 2;
                    break;
                default:
                    StatusComboBox.SelectedIndex = 0; // Padrão para Aberto
                    break;
            }
        }

        // Método para selecionar a prioridade correta no ComboBox
        private void SelecionarPrioridade(string prioridade)
        {
            switch (prioridade)
            {
                case "Baixa":
                    PrioridadeComboBox.SelectedIndex = 0;
                    break;
                case "Média":
                    PrioridadeComboBox.SelectedIndex = 1;
                    break;
                case "Alta":
                    PrioridadeComboBox.SelectedIndex = 2;
                    break;
                case "Crítica":
                    PrioridadeComboBox.SelectedIndex = 3;
                    break;
                default:
                    PrioridadeComboBox.SelectedIndex = 0; // Padrão para Baixa
                    break;
            }
        }

        // Método para adicionar campos específicos que podem não estar no XAML fornecido
        private void AdicionarCamposEspecificos(Chamado chamado)
        {
            // Verificar se existem TextBox para departamento e categoria no XAML
            // Se não existirem, podemos adicioná-los programaticamente

            try
            {
                // Tentar adicionar departamento e categoria se não estiverem no XAML
                var departamentoTextBox = this.FindName("DepartamentoTextBox") as TextBox;
                if (departamentoTextBox != null)
                {
                    departamentoTextBox.Text = chamado.Departamento;
                }
                else
                {
                    // Criar um TextBox para o departamento
                    var newDepartamentoTextBox = new TextBox
                    {
                        Name = "DepartamentoTextBox",
                        Text = chamado.Departamento,
                        Margin = new Thickness(0, 8, 0, 16)
                    };

                    // Adicionar ao formulário (supondo que o ScrollViewer tenha um StackPanel como filho)
                    var stackPanel = ((ScrollViewer)this.FindName("ScrollViewer")).Content as StackPanel;
                    if (stackPanel != null)
                    {
                        stackPanel.Children.Insert(stackPanel.Children.Count - 1, newDepartamentoTextBox);
                    }
                }

                // Fazer o mesmo para a categoria
                var categoriaTextBox = this.FindName("CategoriaTextBox") as TextBox;
                if (categoriaTextBox != null)
                {
                    categoriaTextBox.Text = chamado.Categoria;
                }
                else
                {
                    // Criar um TextBox para a categoria
                    var newCategoriaTextBox = new TextBox
                    {
                        Name = "CategoriaTextBox",
                        Text = chamado.Categoria,
                        Margin = new Thickness(0, 8, 0, 16)
                    };

                    // Adicionar ao formulário
                    var stackPanel = ((ScrollViewer)this.FindName("ScrollViewer")).Content as StackPanel;
                    if (stackPanel != null)
                    {
                        stackPanel.Children.Insert(stackPanel.Children.Count - 1, newCategoriaTextBox);
                    }
                }

                // Adicionar campo para data de fechamento se o status for "Fechado"
                if (chamado.Status == "Fechado" || StatusComboBox.SelectedIndex == 2)
                {
                    var dataFechamentoPicker = this.FindName("DataFechamentoPicker") as DatePicker;
                    if (dataFechamentoPicker != null)
                    {
                        dataFechamentoPicker.SelectedDate = chamado.DataFechamento ?? DateTime.Now;
                    }
                    else
                    {
                        // Criar um DatePicker para a data de fechamento
                        var newDataFechamentoPicker = new DatePicker
                        {
                            Name = "DataFechamentoPicker",
                            SelectedDate = chamado.DataFechamento ?? DateTime.Now,
                            Margin = new Thickness(0, 8, 0, 16)
                        };

                        // Adicionar ao formulário
                        var stackPanel = ((ScrollViewer)this.FindName("ScrollViewer")).Content as StackPanel;
                        if (stackPanel != null)
                        {
                            stackPanel.Children.Insert(stackPanel.Children.Count - 1, newDataFechamentoPicker);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Tratar exceções silenciosamente para não interromper o carregamento
                Console.WriteLine($"Erro ao adicionar campos específicos: {ex.Message}");
            }
        }

        // Evento de clique no botão Salvar
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar campos obrigatórios
                if (string.IsNullOrWhiteSpace(TituloTextBox.Text))
                {
                    MessageBox.Show("O título do chamado é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Atualizar propriedades do chamado
                ChamadoEditado.Titulo = TituloTextBox.Text;
                ChamadoEditado.Descricao = DescricaoTextBox.Text;
                ChamadoEditado.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? _chamadoOriginal.Status;
                ChamadoEditado.Prioridade = (PrioridadeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? _chamadoOriginal.Prioridade;
                ChamadoEditado.Responsavel = ResponsavelTextBox.Text;
                ChamadoEditado.DataCriacao = DataCriacaoPicker.SelectedDate ?? _chamadoOriginal.DataCriacao;
                ChamadoEditado.DataAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                ChamadoEditado.Comentarios = ComentariosTextBox.Text;

                // Verificar campos específicos
                var departamentoTextBox = this.FindName("DepartamentoTextBox") as TextBox;
                if (departamentoTextBox != null)
                {
                    ChamadoEditado.Departamento = departamentoTextBox.Text;
                }
                else
                {
                    ChamadoEditado.Departamento = _chamadoOriginal.Departamento;
                }

                var categoriaTextBox = this.FindName("CategoriaTextBox") as TextBox;
                if (categoriaTextBox != null)
                {
                    ChamadoEditado.Categoria = categoriaTextBox.Text;
                }
                else
                {
                    ChamadoEditado.Categoria = _chamadoOriginal.Categoria;
                }

                // Tratar data de fechamento
                if (ChamadoEditado.Status == "Fechado")
                {
                    var dataFechamentoPicker = this.FindName("DataFechamentoPicker") as DatePicker;
                    if (dataFechamentoPicker != null)
                    {
                        ChamadoEditado.DataFechamento = dataFechamentoPicker.SelectedDate ?? DateTime.Now;
                    }
                    else
                    {
                        ChamadoEditado.DataFechamento = DateTime.Now;
                    }
                }
                else
                {
                    ChamadoEditado.DataFechamento = null;
                }

                // Calcular propriedades derivadas
                ChamadoEditado.DataCriacaoFormatada = ChamadoEditado.DataCriacao.ToString("dd/MM/yyyy");
                if (ChamadoEditado.DataFechamento.HasValue)
                {
                    ChamadoEditado.DataFechamentoFormatada = ChamadoEditado.DataFechamento.Value.ToString("dd/MM/yyyy");
                }

                // Fechar o diálogo com resultado positivo
                Window.GetWindow(this).DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o chamado: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento de clique no botão Cancelar
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Fechar o diálogo com resultado negativo
            Window.GetWindow(this).DialogResult = false;
        }

        // Evento para atualizar a visibilidade do campo de data de fechamento quando o status muda
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                bool isFechado = StatusComboBox.SelectedIndex == 2; // Índice 2 = "Fechado"

                // Procurar pelo DatePicker de data de fechamento
                var dataFechamentoPicker = this.FindName("DataFechamentoPicker") as DatePicker;

                if (isFechado)
                {
                    // Se o status for "Fechado", mostrar o campo de data de fechamento
                    if (dataFechamentoPicker == null)
                    {
                        // Criar o campo se não existir
                        dataFechamentoPicker = new DatePicker
                        {
                            Name = "DataFechamentoPicker",
                            SelectedDate = DateTime.Now,
                            Margin = new Thickness(0, 8, 0, 16)
                        };

                        // Adicionar ao formulário
                        var stackPanel = ((ScrollViewer)this.FindName("ScrollViewer")).Content as StackPanel;
                        if (stackPanel != null)
                        {
                            // Adicionar antes dos comentários
                            int insertIndex = stackPanel.Children.IndexOf(ComentariosTextBox);
                            if (insertIndex >= 0)
                            {
                                stackPanel.Children.Insert(insertIndex, dataFechamentoPicker);
                            }
                            else
                            {
                                stackPanel.Children.Add(dataFechamentoPicker);
                            }
                        }
                    }
                    else
                    {
                        // Mostrar o campo se já existir
                        dataFechamentoPicker.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    // Se o status não for "Fechado", esconder o campo de data de fechamento
                    if (dataFechamentoPicker != null)
                    {
                        dataFechamentoPicker.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar campo de data de fechamento: {ex.Message}");
            }
        }
    }
}
