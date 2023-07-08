using Srv.Auth.Domain.Entities;

namespace Srv.Auth.Domain.Models
{
    public class UserCodeConfirmationModel
    {
        public int Id { get; set; }
        public string UsuarioCpfCnpj { get; set; } = string.Empty;
        public virtual User Usuario { get; set; } = new();
        public string Codigo { get; set; } = string.Empty;
        public DateTime Validade { get; set; }
        public bool Ativo { get; set; }
    }
}
