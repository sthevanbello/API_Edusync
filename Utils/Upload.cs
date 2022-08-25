using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace ApiMaisEventos.Utils
{
    // Singleton -> Static
    public static class Upload
    {
        // Upload
        public static string UploadFile(IFormFile arquivo, string[] extensoesPermitidas, string diretorio)
        {
            try
            {
                // Onde será salvo o arquivo
                var pasta = Path.Combine("StaticFiles", diretorio);
                var caminho = Path.Combine(Directory.GetCurrentDirectory(), pasta);
                // Verifica se existe um arquivo para ser salvo
                if (arquivo.Length > 0)
                {
                    string nomeArquivo = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');
                    if (ValidarExtensao(extensoesPermitidas, nomeArquivo))
                    {
                        var extensao = RetornarExtensao(nomeArquivo);
                        var novoNome = $"{Guid.NewGuid()}.{extensao}";
                        var caminhoCompleto = Path.Combine(caminho, novoNome);
                        // Salvar o arquivo na aplicação
                        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                        {
                            arquivo.CopyTo(stream);
                        }
                        return novoNome;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }
        // Validar extensões

        // Validar extensão de arquivo
        public static bool ValidarExtensao(string[] extensoesPermitidas, string nomeArquivo)
        {
            var extensao = RetornarExtensao(nomeArquivo);
            foreach (var ext in extensoesPermitidas)
            {
                if (ext == extensao)
                {
                    return true;
                }
            }
            return false;
        }
        // Remover arquivo

        // Retornar Extensão
        public static string RetornarExtensao(string nomeArquivo)
        {
            // arquivo.jpeg arqui.vo.jpeg
            //      0   1       0   1   2
            // Por isso o dados.Length-1 - Para pegar sempre o último elemento do array após fazer o split
            string[] dados = nomeArquivo.Split('.');
            return dados[dados.Length - 1];
        }
    }
}
