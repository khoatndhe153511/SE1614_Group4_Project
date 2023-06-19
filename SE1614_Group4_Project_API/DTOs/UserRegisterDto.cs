using SE1614_Group4_Project_API.Utils;
using System.ComponentModel.DataAnnotations;

namespace SE1614_Group4_Project_API.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = Constants.ERR001)]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.ERR001)]
        public string UserName { get; set; }

        [Required(ErrorMessage = Constants.ERR001)]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = Constants.ERR001)]
        public string Password { get; set; }

        [Required(ErrorMessage = Constants.ERR001)]
        public string ConfirmPassword { get; set; }
    }
}