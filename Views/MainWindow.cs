using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using teste4343.Models;
using System.Windows.Input;
using teste4343.Views;


namespace teste4343.Views
{

    public partial class MainWindow : Window
    {
        // Coleção observável para os chamados
        private ObservableCollection<Chamado> _chamados;

        public object ChamadosItem { get; private set; }

        public MainWindow()
        {

            InitializeComponent();

            // Inicializar a coleção de chamados
            _chamados = new ObservableCollection<Chamado>();
            ChamadosGrid.ItemsSource = _chamados;

            // Configurar as colunas do DataGrid se não estiverem definidas no XAML
            ConfigurarColunasChamadosGrid();

            // Inicializar contadores
            AtualizarContadores();
        }



        // Método para configurar as colunas do DataGrid
        private void ConfigurarColunasChamadosGrid()
        {
            // Verificar se as colunas já estão definidas no XAML
            if (ChamadosGrid.Columns.Count == 0)
            {
                // Adicionar coluna ID
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "ID",
                    Binding = new System.Windows.Data.Binding("Id"),
                    Width = 50
                });

                // Adicionar coluna Título
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Título",
                    Binding = new System.Windows.Data.Binding("Titulo"),
                    Width = 200
                });

                // Adicionar coluna Status
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Status",
                    Binding = new System.Windows.Data.Binding("Status"),
                    Width = 120
                });

                // Adicionar coluna Prioridade
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Prioridade",
                    Binding = new System.Windows.Data.Binding("Prioridade"),
                    Width = 100
                });

                // Adicionar coluna Responsável
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Responsável",
                    Binding = new System.Windows.Data.Binding("Responsavel"),
                    Width = 150
                });

                // Adicionar coluna Data de Criação
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Data de Criação",
                    Binding = new System.Windows.Data.Binding("DataCriacaoFormatada"),
                    Width = 150
                });

                // Adicionar coluna Departamento
                ChamadosGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Departamento",
                    Binding = new System.Windows.Data.Binding("Departamento"),
                    Width = 150
                });
            }
        }
        public static class UsuarioLogado
        {
            // Propriedades do usuário logado
            public static string Nome { get; set; }
            public static string TipoUsuario { get; set; }
            public static bool IsAdmin { get; set; }
            public static DateTime LoginTime { get; set; }

            // Método para verificar se o usuário tem permissão para editar chamados
            public static bool PodeEditarChamados()
            {
                return IsAdmin;
            }

            // Método para verificar se o usuário tem permissão para excluir chamados
            public static bool PodeExcluirChamados()
            {
                return IsAdmin;
            }

            // Método para verificar se o usuário tem permissão para criar novos chamados
            public static bool PodeCriarChamados()
            {
                return true; // Todos podem criar chamados
            }

            // Método para configurar o usuário logado
            public static void Configurar(string nome, string tipo)
            {
                Nome = nome;
                TipoUsuario = tipo;
                IsAdmin = (tipo.ToLower() == "admin");
                LoginTime = DateTime.Now;
            }

            // Método para limpar os dados do usuário (logout)
            public static void Limpar()
            {
                Nome = null;
                TipoUsuario = null;
                IsAdmin = false;
            }
        }

            private void BtnConfiguracoes_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var configWindow = new ConfiguracoesWindow();
                    configWindow.Owner = this;
                    configWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao abrir configurações: {ex.Message}",
                                  "Erro",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            private void RelatoriosItem_Click(object sender, MouseButtonEventArgs e)
        {
            // Carregar o UserControl na área principal
            var relatoriosView = new UserControl(); // Substitua pelo nome correto do UserControl
            MainContent.Content = relatoriosView;
        }

        // Método para tratar o clique no botão "Novo Chamado"
        private void BtnNovoChamado_Click(object sender, RoutedEventArgs e)
        {
            // Cria uma instância do UserControl NovoChamadoDialog
            var novoChamadoDialog = new NovoChamadoDialog();

            // Encapsula o UserControl em uma janela
            var dialogWindow = new Window
            {
                Content = novoChamadoDialog,
                Width = 600,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Title = "Novo Chamado",
                Owner = this // Define o proprietário da janela como a janela principal
            };

            // Exibe a janela como modal
            bool? resultado = dialogWindow.ShowDialog();

            // Verifica o resultado (true = Salvar, false = Cancelar)
            if (resultado == true)
            {
                var chamadoCriado = novoChamadoDialog.NovoChamado; // Obtém o chamado criado

                // Adiciona o chamado à lista
                if (chamadoCriado != null)
                {
                    _chamados.Add(chamadoCriado);

                    // Atualiza os contadores
                    AtualizarContadores();

                    MessageBox.Show($"Chamado '{chamadoCriado.Titulo}' salvo com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Método para editar um chamado
        // Método para editar um chamado
 
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Exibe uma mensagem de confirmação
            MessageBoxResult result = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Confirmação de Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Limpa os dados do usuário logado
                UsuarioLogado.Limpar();

                // Abre a janela de login
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Fecha a janela principal
                this.Close();
            }
        }

        private ContentControl GetMainContent()
        {
            return MainContent;
        }



        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var relatoriosWindow = new RelatoriosChamadosWindow();

                // Se você quiser que a janela principal fique bloqueada até fechar a de relatórios
                relatoriosWindow.ShowDialog();

                // OU se quiser que a janela de relatórios seja independente
                // relatoriosWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir janela de relatórios: {ex.Message}",
                               "Erro",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }




        private void BtnEditarChamado_Click(object sender, RoutedEventArgs e)
        {
            // Verificar se há um chamado selecionado
            if (ChamadosGrid.SelectedItem is Chamado chamadoSelecionado)
            {
                // Criar e configurar o diálogo de edição
                var editarDialog = new EditarChamadoDialog(chamadoSelecionado);
                var dialogWindow = new Window
                {
                    Title = "Editar Chamado",
                    Width = 600,
                    Height = 700,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this,
                    Content = editarDialog
                };

                // Mostrar o diálogo
                var resultado = dialogWindow.ShowDialog();

                // Se o usuário confirmou as alterações
                if (resultado == true)
                {
                    // Obter o chamado editado do diálogo
                    var chamadoEditado = editarDialog.ChamadoEditado;

                    // Encontrar o índice do chamado original
                    int index = _chamados.IndexOf(chamadoSelecionado);
                    if (index >= 0)
                    {
                        // Substituir o chamado original pelo editado
                        _chamados[index] = chamadoEditado;

                        // Atualizar a interface
                        ChamadosGrid.Items.Refresh();

                        // Atualizar contadores
                        AtualizarContadores();

                        MessageBox.Show("Chamado atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um chamado para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Método para excluir um chamado
        private void BtnExcluirChamado_Click(object sender, RoutedEventArgs e)
        {
            // Obter o chamado selecionado
            Chamado? chamadoSelecionado = ChamadosGrid.SelectedItem as Chamado;

            // Verificar se há um chamado selecionado
            if (chamadoSelecionado == null)
            {
                MessageBox.Show("Por favor, selecione um chamado para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirmar exclusão com o usuário
            MessageBoxResult resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir o chamado '{chamadoSelecionado.Titulo}'?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Se o usuário confirmar, exclui o chamado
            if (resultado == MessageBoxResult.Yes)
            {
                _chamados.Remove(chamadoSelecionado);

                // Atualiza os contadores após exclusão
                AtualizarContadores();

                MessageBox.Show("Chamado excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Método para visualizar detalhes de um chamado
        private void BtnVisualizarChamado_Click(object sender, RoutedEventArgs e)
        {
            // Obter o chamado selecionado
            Chamado? chamadoSelecionado = ChamadosGrid.SelectedItem as Chamado;

            // Verificar se há um chamado selecionado
            if (chamadoSelecionado == null)
            {
                MessageBox.Show("Por favor, selecione um chamado para visualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Criar uma janela para mostrar os detalhes do chamado
            var detalhesWindow = new Window
            {
                Title = $"Detalhes do Chamado #{chamadoSelecionado.Id}",
                Width = 600,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            // Criar um StackPanel para organizar os detalhes
            var stackPanel = new StackPanel { Margin = new Thickness(20) };

            // Adicionar os detalhes ao StackPanel
            stackPanel.Children.Add(new TextBlock { Text = $"ID: {chamadoSelecionado.Id}", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Título: {chamadoSelecionado.Titulo}", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Status: {chamadoSelecionado.Status}", Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Prioridade: {chamadoSelecionado.Prioridade}", Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Responsável: {chamadoSelecionado.Responsavel}", Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Departamento: {chamadoSelecionado.Departamento}", Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Categoria: {chamadoSelecionado.Categoria}", Margin = new Thickness(0, 5, 0, 5) });
            stackPanel.Children.Add(new TextBlock { Text = $"Data de Criação: {chamadoSelecionado.DataCriacaoFormatada}", Margin = new Thickness(0, 5, 0, 5) });

            if (chamadoSelecionado.DataFechamento.HasValue)
            {
                stackPanel.Children.Add(new TextBlock { Text = $"Data de Fechamento: {chamadoSelecionado.DataFechamentoFormatada}", Margin = new Thickness(0, 5, 0, 5) });
            }

            stackPanel.Children.Add(new TextBlock { Text = $"Data de Atualização: {chamadoSelecionado.DataAtualizacao}", Margin = new Thickness(0, 5, 0, 5) });

            // Adicionar descrição com título
            stackPanel.Children.Add(new TextBlock { Text = "Descrição:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 10, 0, 5) });
            stackPanel.Children.Add(new TextBox { Text = chamadoSelecionado.Descricao, IsReadOnly = true, TextWrapping = TextWrapping.Wrap, Height = 100, Margin = new Thickness(0, 0, 0, 10) });

            // Adicionar comentários com título
            stackPanel.Children.Add(new TextBlock { Text = "Comentários:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 10, 0, 5) });
            stackPanel.Children.Add(new TextBox { Text = chamadoSelecionado.Comentarios, IsReadOnly = true, TextWrapping = TextWrapping.Wrap, Height = 100, Margin = new Thickness(0, 0, 0, 10) });

            // Adicionar botão para fechar
            var fecharButton = new Button { Content = "Fechar", Width = 100, Margin = new Thickness(0, 10, 0, 0) };
            fecharButton.Click += (s, args) => detalhesWindow.Close();
            stackPanel.Children.Add(fecharButton);

            // Definir o conteúdo da janela
            detalhesWindow.Content = new ScrollViewer { Content = stackPanel };

            // Exibir a janela
            detalhesWindow.ShowDialog();
        }

        // Método para atualizar os contadores nos cards
        private void AtualizarContadores()
        {
            // Atualiza o total de chamados
            TotalChamadosText.Text = _chamados.Count.ToString();

            // Conta chamados abertos
            int abertos = _chamados.Count(c => c.Status == "Aberto");
            ChamadosAbertosText.Text = abertos.ToString();

            // Conta chamados em andamento
            int emAndamento = _chamados.Count(c => c.Status == "Em Andamento");
            EmAndamentoText.Text = emAndamento.ToString();
        }
    }
}
