﻿namespace SE1614_Group4_Project_API.DTOs
{
    public class ChangePasswordModelDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}