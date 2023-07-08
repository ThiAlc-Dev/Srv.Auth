namespace Srv.Auth.Domain.Entities
{
    public class UserAccess
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime DtCriacaoToken { get; set; }
        public DateTime DtExpiracaoToken { get; set; }
        public virtual User Usuario { get; set; }
        public string UsuarioCpfCnpj { get; set; }
        public bool Ativo { get; set; }
    }
}
