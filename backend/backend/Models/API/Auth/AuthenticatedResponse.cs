﻿namespace backend.Models.API.Auth
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
    }
}
