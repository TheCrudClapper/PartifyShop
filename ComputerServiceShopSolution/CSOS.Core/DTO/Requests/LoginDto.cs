﻿namespace CSOS.Core.DTO
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool isPersistent { get; set; }
    }
}
