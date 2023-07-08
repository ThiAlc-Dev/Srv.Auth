﻿namespace Srv.Auth.Repository.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string AccessTokenExpirationInMinutes { get; set; }
        public string RefreshTokenExpirationInDays { get; set; }
    }
}
