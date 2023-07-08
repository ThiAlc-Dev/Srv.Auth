namespace Srv.Auth.Domain.Responses.CommandResponses
{
    public class CreateSessionrResponse
    {
        public bool authenticated { get; set; }
        public string created { get; set; } = default!;
        public string expiration { get; set; } = default!;
        public string acessToken { get; set; } = default!;
        public string refreshToken { get; set; } = default!;
        public string userName { get; set; } = default!;
        public string message { get; set; } = default!;
    }
}
