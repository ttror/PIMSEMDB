using System.Collections.ObjectModel;

namespace teste4343.Models
{


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
    public class Chamado
    {
        // Propriedades básicas
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public string Responsavel { get; set; }
        public string Departamento { get; set; }
        public string Categoria { get; set; }

        // Propriedades de data
        public DateTime DataCriacao { get; set; }
        public DateTime? DataFechamento { get; set; }
        public ObservableCollection<Anexo> Anexos { get; set; } = new ObservableCollection<Anexo>();
        public string DataAtualizacao { get; set; }

        // Propriedades formatadas para exibição
        public string DataCriacaoFormatada { get; set; }
        public string DataFechamentoFormatada { get; set; }

        // Comentários adicionais
        public string Comentarios { get; set; }

        // Construtor padrão
        public Chamado()
        {
            // Inicializar com valores padrão
            DataCriacao = DateTime.Now;
            DataAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Status = "Aberto";
            Prioridade = "Média";
            Comentarios = "";

            // Inicializar formatos de data
            AtualizarFormatosDatas();
        }

        // Método auxiliar para atualizar os formatos de data
        public void AtualizarFormatosDatas()
        {
            DataCriacaoFormatada = DataCriacao.ToString("dd/MM/yyyy");

            if (DataFechamento.HasValue)
            {
                DataFechamentoFormatada = DataFechamento.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                DataFechamentoFormatada = "";
            }
        }

        // Propriedade calculada para exibir o número de anexos
        public string InformacaoAnexos => Anexos.Count > 0 ? $"{Anexos.Count} anexo(s)" : "Nenhum anexo";
    }
}
