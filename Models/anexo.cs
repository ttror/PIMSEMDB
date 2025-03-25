using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace teste4343.Models
{
    public class Anexo : INotifyPropertyChanged
    {
        private int _id;
        private int _chamadoId;
        private string _nome;
        private string _caminho;
        private string _tipo;
        private long _tamanho;
        private DateTime _dataUpload;
        private DateTime _dataAnexo;

        // Propriedades com notificação de mudança
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int ChamadoId
        {
            get => _chamadoId;
            set { _chamadoId = value; OnPropertyChanged(); }
        }

        public string Nome
        {
            get => _nome;
            set { _nome = value; OnPropertyChanged(); }
        }

        public string Caminho
        {
            get => _caminho;
            set
            {
                _caminho = value;
                AtualizarInformacoesArquivo();
                OnPropertyChanged();
            }
        }

        public string Tipo
        {
            get => _tipo;
            set { _tipo = value; OnPropertyChanged(); }
        }

        public long Tamanho
        {
            get => _tamanho;
            set
            {
                _tamanho = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TamanhoFormatado));
            }
        }

        public DateTime DataUpload
        {
            get => _dataUpload;
            set
            {
                _dataUpload = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DataUploadFormatada));
            }
        }

        public DateTime DataAnexo
        {
            get => _dataAnexo;
            set { _dataAnexo = value; OnPropertyChanged(); }
        }

        // Propriedades somente leitura formatadas
        public string DataUploadFormatada => DataUpload.ToString("dd/MM/yyyy HH:mm");
        public string TamanhoFormatado => FormatarTamanho(Tamanho);
        public string IconeArquivo => ObterIcone();
        public bool Existe => File.Exists(Caminho);
        public string ExtensaoArquivo => Path.GetExtension(Caminho)?.ToLower() ?? "";

        // Construtor padrão
        public Anexo()
        {
            DataUpload = DateTime.Now;
            DataAnexo = DateTime.Now;
        }

        // Construtor com caminho do arquivo
        public Anexo(string caminho)
        {
            Caminho = caminho;
            DataUpload = DateTime.Now;
            DataAnexo = DateTime.Now;
            AtualizarInformacoesArquivo();
        }

        // Método para atualizar informações do arquivo
        private void AtualizarInformacoesArquivo()
        {
            if (string.IsNullOrEmpty(Caminho)) return;

            Nome = Path.GetFileName(Caminho);
            Tipo = Path.GetExtension(Caminho)?.ToLower() ?? "";

            try
            {
                var fileInfo = new FileInfo(Caminho);
                if (fileInfo.Exists)
                {
                    Tamanho = fileInfo.Length;
                    DataAnexo = fileInfo.CreationTime;
                }
            }
            catch (Exception ex)
            {
                Tamanho = 0;
                // Você pode adicionar um log de erro aqui
                System.Diagnostics.Debug.WriteLine($"Erro ao obter informações do arquivo: {ex.Message}");
            }
        }

        // Método para formatar o tamanho do arquivo
        private string FormatarTamanho(long bytes)
        {
            string[] sufixos = { "B", "KB", "MB", "GB", "TB" };
            int contador = 0;
            decimal tamanho = bytes;

            while (tamanho >= 1024 && contador < sufixos.Length - 1)
            {
                tamanho /= 1024;
                contador++;
            }

            return string.Format("{0:0.##} {1}", tamanho, sufixos[contador]);
        }

        // Método para obter o ícone com base no tipo de arquivo
        private string ObterIcone()
        {
            if (string.IsNullOrEmpty(Tipo)) return "FileOutline";

            return Tipo.ToLower() switch
            {
                ".pdf" => "FileDocumentOutline",
                ".doc" or ".docx" => "FileWordOutline",
                ".xls" or ".xlsx" => "FileExcelOutline",
                ".ppt" or ".pptx" => "FilePowerpointOutline",
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => "FileImageOutline",
                ".zip" or ".rar" or ".7z" => "FileZipOutline",
                ".txt" => "FileTextOutline",
                _ => "FileOutline"
            };
        }

        // Método para verificar se o arquivo é uma imagem
        public bool IsImagem()
        {
            string[] extensoesImagem = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            return Array.Exists(extensoesImagem, x => x.Equals(Tipo, StringComparison.OrdinalIgnoreCase));
        }

        // Método para copiar o arquivo para um novo destino
        public bool CopiarPara(string destinoPath)
        {
            try
            {
                if (!File.Exists(Caminho)) return false;

                string novoDestino = Path.Combine(destinoPath, Nome);
                File.Copy(Caminho, novoDestino, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Método para excluir o arquivo
        public bool Excluir()
        {
            try
            {
                if (File.Exists(Caminho))
                {
                    File.Delete(Caminho);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Implementação do INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
