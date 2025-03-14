﻿using System.ComponentModel.DataAnnotations;

namespace SE1614_Group4_Project_API.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}