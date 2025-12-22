using ProjetoCompleto.Models;
using ProjetoCompleto.ViewModels;
using System.Threading.Tasks;

namespace ProjetoCompleto.Services
{
    public interface IAuthService
    {
        Task<(bool Sucesso, string Mensagem, Usuario Usuario)> CadastrarUsuario(CadastroViewModel model);
        Task<(bool Sucesso, string Mensagem, Usuario Usuario)> Login(LoginViewModel model);
        Task<bool> EmailExiste(string email);
        string HashSenha(string senha);
        bool VerificarSenha(string senha, string senhaHash);
    }
}