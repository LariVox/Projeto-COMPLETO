using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoCompleto.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(255)]
        public string SenhaHash { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime?  DataUltimoAcesso { get; set; }

        public bool Ativo { get; set; } = true;

        [StringLength(50)]
        public string Perfil { get; set; } = "Usuario"; // Usuario, Admin, etc.
    }
}