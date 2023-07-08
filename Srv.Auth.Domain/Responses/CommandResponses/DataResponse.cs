namespace Srv.Auth.Domain.Responses.CommandResponses
{
    public class DataResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; } = default!;
    }
}
