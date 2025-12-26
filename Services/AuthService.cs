using Dapper;
using ProjetoCompleto.Data;
using ProjetoCompleto.Models;
using ProjetoCompleto.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoCompleto.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbConnectionFactory _dbFactory;

        public AuthService(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Usuario?> AutenticarAsync(string email, string senha)
        {
            using var connection = _dbFactory.CreateConnection();

            var sql = "SELECT ID, NOME, EMAIL, SENHA_HASH AS SenhaHash, TELEFONE, PERFIL, DATA_CADASTRO AS DataCadastro, ATIVO FROM USUARIOS WHERE EMAIL = @Email AND ATIVO = 1";
            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });

            if (usuario == null)
                return null;

            var senhaHash = GerarHashSenha(senha);

            if (usuario.SenhaHash != senhaHash)
                return null;

            return usuario;
        }

        public async Task<bool> CadastrarUsuarioAsync(CadastroViewModel model)
        {
            using var connection = _dbFactory.CreateConnection();

            // Verificar se email já existe
            var sqlVerifica = "SELECT COUNT(*) FROM USUARIOS WHERE EMAIL = @Email";
            var existe = await connection.ExecuteScalarAsync<int>(sqlVerifica, new { Email = model.Email });

            if (existe > 0)
                return false;

            // Inserir novo usuário
            var sql = @"INSERT INTO USUARIOS (NOME, EMAIL, SENHA_HASH, TELEFONE, PERFIL, DATA_CADASTRO, ATIVO) 
                        VALUES (@Nome, @Email, @SenhaHash, @Telefone, @Perfil, @DataCadastro, @Ativo)";

            await connection.ExecuteAsync(sql, new
            {
                Nome = model.Nome,
                Email = model.Email,
                SenhaHash = GerarHashSenha(model.Senha),
                Telefone = model.Telefone,
                Perfil = "Usuario",
                DataCadastro = DateTime.Now,
                Ativo = true
            });

            return true;
        }

        public async Task<Usuario?> ObterUsuarioPorEmailAsync(string email)
        {
            using var connection = _dbFactory.CreateConnection();

            var sql = "SELECT ID, NOME, EMAIL, SENHA_HASH AS SenhaHash, TELEFONE, PERFIL, DATA_CADASTRO AS DataCadastro, ATIVO FROM USUARIOS WHERE EMAIL = @Email";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
        }

        public async Task<Usuario?> ObterUsuarioPorIdAsync(int id)
        {
            using var connection = _dbFactory.CreateConnection();

            var sql = "SELECT ID, NOME, EMAIL, SENHA_HASH AS SenhaHash, TELEFONE, PERFIL, DATA_CADASTRO AS DataCadastro, ATIVO FROM USUARIOS WHERE ID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
        }

        private string GerarHashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}