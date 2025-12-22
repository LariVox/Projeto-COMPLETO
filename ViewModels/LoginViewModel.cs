using System.ComponentModel.DataAnnotations;

namespace ProjetoCompleto.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType. Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}