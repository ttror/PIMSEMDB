using System.Windows;

namespace teste4343.Views
{
    /// <summary>
    /// Interação lógica para LoginWindow.xaml
    /// </summary>
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

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Foca no campo de usuário ao iniciar
            TxtUsuario.Focus();

            // Carrega as configurações salvas (se existirem)
            CarregarConfiguracoesSalvas();
        }

        /// <summary>
        /// Carrega as configurações salvas do usuário
        /// </summary>
        private void CarregarConfiguracoesSalvas()
        {
            // Verifica se há configurações salvas
            if (Properties.Settings.Default.LembrarUsuario)
            {
                TxtUsuario.Text = Properties.Settings.Default.NomeUsuario;
                ChkLembrar.IsChecked = true;
            }
        }

        /// <summary>
        /// Salva as configurações do usuário
        /// </summary>
        private void SalvarConfiguracoes()
        {
            Properties.Settings.Default.LembrarUsuario = ChkLembrar.IsChecked ?? false;
            Properties.Settings.Default.NomeUsuario = ChkLembrar.IsChecked == true ? TxtUsuario.Text : "";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Evento do botão Entrar
        /// </summary>
        private async void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            await RealizarLogin();
        }

        /// <summary>
        /// Evento do botão Cancelar
        /// </summary>
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Método para realizar o login
        /// </summary>
        private async Task RealizarLogin()
        {
            // Esconde mensagem de erro anterior
            TxtMensagemErro.Visibility = Visibility.Collapsed;

            // Validação básica
            if (string.IsNullOrWhiteSpace(TxtUsuario.Text))
            {
                ExibirMensagemErro("Por favor, informe o usuário.");
                TxtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtSenha.Password))
            {
                ExibirMensagemErro("Por favor, informe a senha.");
                TxtSenha.Focus();
                return;
            }

            try
            {
                // Mostra o indicador de progresso
                ProgressOverlay.Visibility = Visibility.Visible;

                // Simula uma operação de autenticação
                await Task.Delay(1000);

                // Verifica as credenciais (em um sistema real, isso seria feito contra um banco de dados ou serviço)
                bool loginValido = VerificarCredenciais(TxtUsuario.Text, TxtSenha.Password);

                if (loginValido)
                {
                    // Salva as configurações do usuário
                    SalvarConfiguracoes();

                    // Configura o usuário logado
                    string tipoUsuario = TxtUsuario.Text.ToLower() == "admin" ? "admin" : "user";
                    UsuarioLogado.Configurar(TxtUsuario.Text, tipoUsuario);

                    // Abre a janela principal
                    var mainWindow = new MainWindow();
                    mainWindow.Show();

                    // Fecha a janela de login
                    this.Close();
                }
                else
                {
                    ExibirMensagemErro("Usuário ou senha incorretos.");
                    TxtSenha.Clear();
                    TxtSenha.Focus();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro($"Erro ao realizar login: {ex.Message}");
            }
            finally
            {
                // Esconde o indicador de progresso
                ProgressOverlay.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Verifica as credenciais do usuário
        /// </summary>
        private bool VerificarCredenciais(string usuario, string senha)
        {
            return (usuario.ToLower() == "admin" && senha == "admin") ||
                   (usuario.ToLower() == "user" && senha == "user");
        }

        /// <summary>
        /// Exibe uma mensagem de erro
        /// </summary>
        private void ExibirMensagemErro(string mensagem)
        {
            TxtMensagemErro.Text = mensagem;
            TxtMensagemErro.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Evento de tecla pressionada no campo de senha
        /// </summary>
        private async void TxtSenha_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                await RealizarLogin();
            }
        }
    }
}
