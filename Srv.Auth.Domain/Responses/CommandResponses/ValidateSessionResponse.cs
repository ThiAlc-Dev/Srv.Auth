namespace Srv.Auth.Domain.Responses.CommandResponses
{
    public class ValidateSessionResponse
    {
        public bool Success { get; set; }
        public string? RefreshToken { get; set; }
        public string? DataExpiracao { get; set; }
    }
}
