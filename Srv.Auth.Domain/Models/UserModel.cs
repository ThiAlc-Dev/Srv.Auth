namespace Srv.Auth.Domain.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CodigoAcesso { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
