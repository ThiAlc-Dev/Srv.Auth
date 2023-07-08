namespace Srv.Auth.Domain.Responses.CommandResponses
{
    public class UserAccessResponse
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DtCriacaoToken { get; set; }
        public DateTime DtExpiracaoToken { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
