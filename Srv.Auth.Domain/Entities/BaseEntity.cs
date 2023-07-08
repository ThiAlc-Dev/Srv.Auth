namespace Srv.Auth.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public DateTime? AtualizadoEm { get; set; }
        public bool Ativo { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.Now;
            Ativo = true;
        }
    }
}
