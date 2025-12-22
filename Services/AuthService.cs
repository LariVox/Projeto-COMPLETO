using Microsoft.EntityFrameworkCore;
using ProjetoCompleto.Data;
using ProjetoCompleto.Models;
using ProjetoCompleto.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoCompleto.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Sucesso, string Mensagem, Usuario Usuario)> CadastrarUsuario(CadastroViewModel model)
        {
            try
            {
                // Verifica se o email já existe
                if (await EmailExiste(model.Email))
                {
                    return (false, "Este email já está cadastrado", null!);
                }

                // Cria o usuário
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email.ToLower(),
                    SenhaHash = HashSenha(model.Senha),
                    Telefone = model.Telefone,
                    DataCadastro = DateTime.Now,
                    Ativo = true,
                    Perfil = "Usuario"
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return (true, "Cadastro realizado com sucesso!", usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao cadastrar usuário: {ex.Message}", null!);
            }
        }

        public async Task<(bool Sucesso, string Mensagem, Usuario Usuario)> Login(LoginViewModel model)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());

                if (usuario == null)
                {
                    return (false, "Email ou senha inválidos", null!);
                }

                if (!usuario.Ativo)
                {
                    return (false, "Usuário inativo", null!);
                }

                if (!VerificarSenha(model.Senha, usuario.SenhaHash))
                {
                    return (false, "Email ou senha inválidos", null!);
                }

                // Atualiza data do último acesso
                usuario.DataUltimoAcesso = DateTime.Now;
                await _context.SaveChangesAsync();

                return (true, "Login realizado com sucesso!", usuario);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao fazer login: {ex.Message}", null!);
            }
        }

        public async Task<bool> EmailExiste(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerificarSenha(string senha, string senhaHash)
        {
            var hashSenhaInformada = HashSenha(senha);
            return hashSenhaInformada == senhaHash;
        }
    }
}