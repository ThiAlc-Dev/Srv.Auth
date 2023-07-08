namespace Srv.Auth.Domain.Responses.CommandResponses
{
    public class ForgotPasswordResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
