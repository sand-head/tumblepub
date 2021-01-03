using System.ComponentModel.DataAnnotations;

namespace Bloq.Shared.Models.Api
{
    public record LoginModel(
        [Required] string Email,
        [Required] string Password,
        bool RememberMe);

    public record RegisterModel(
        [Required] string Username,
        [Required][DataType(DataType.EmailAddress)] string Email,
        [Required][DataType(DataType.Password)] string Password,
        [DataType(DataType.Password)][property: Compare("Password", ErrorMessage = "The confirmation password does not match.")] string ConfirmPassword);
}
