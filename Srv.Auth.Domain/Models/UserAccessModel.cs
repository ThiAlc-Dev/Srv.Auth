using Srv.Auth.Domain.Entities;

namespace Srv.Auth.Domain.Models
{
    public class UserAccessModel
    {
        public string Token { get; set; }
        public DateTime DtCriacaoToken { get; set; }
        public DateTime DtExpiracaoToken { get; set; }
        public string CpfCnpj { get; set; }
        public bool Ativo { get; set; }
    }
}
