using System.ComponentModel.DataAnnotations;

namespace Catalogo_Blazor.Shared.Models
{
    public class User
    {
        [Required(ErrorMessage = "Informe o email")]
        [EmailAddress(ErrorMessage = "Formato do email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha")]
        public string Password { get; set; }
    }
}
