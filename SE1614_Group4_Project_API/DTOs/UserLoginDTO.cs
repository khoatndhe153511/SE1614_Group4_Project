using SE1614_Group4_Project_API.Utils;
using System.ComponentModel.DataAnnotations;

namespace SE1614_Group4_Project_API.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = Constants.ERR001)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.ERR001)]
        public string Password { get; set; }
    }
}