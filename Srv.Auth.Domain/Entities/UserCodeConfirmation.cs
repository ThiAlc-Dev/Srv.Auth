namespace Srv.Auth.Domain.Entities
{
    public class UserCodeConfirmation
    {
        public int Id { get; set; }
        public string UsuarioCpfCnpj { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public DateTime Validade { get; set; }
    }
}
