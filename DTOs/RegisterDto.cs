﻿namespace Recipe_Sharing_Platform.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsChef { get; set; }
    }
}
