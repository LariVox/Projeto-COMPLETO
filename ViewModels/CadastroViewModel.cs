using System. ComponentModel.DataAnnotations;

namespace ProjetoCompleto.ViewModels
{
    public class CadastroViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmarSenha { get; set; }

        [Phone(ErrorMessage = "Telefone inválido")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }
    }
}