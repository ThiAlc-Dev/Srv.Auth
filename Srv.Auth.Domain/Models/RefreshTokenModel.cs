using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srv.Auth.Domain.Models
{
    public class RefreshTokenModel
    {
        public string Username { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
