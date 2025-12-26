using ProjetoCompleto.Models;
using ProjetoCompleto.ViewModels;

namespace ProjetoCompleto.Services
{
    public interface IAuthService
    {
        Task<Usuario? > AutenticarAsync(string email, string senha);
        Task<bool> CadastrarUsuarioAsync(CadastroViewModel model);
        Task<Usuario?> ObterUsuarioPorEmailAsync(string email);
        Task<Usuario?> ObterUsuarioPorIdAsync(int id);
    }
}