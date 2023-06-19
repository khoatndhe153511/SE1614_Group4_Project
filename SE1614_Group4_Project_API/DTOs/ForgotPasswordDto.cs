using System.ComponentModel.DataAnnotations;

namespace SE1614_Group4_Project_API.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}