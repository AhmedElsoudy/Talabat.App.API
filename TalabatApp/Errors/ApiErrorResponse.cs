
namespace TalabatApp.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetMessageForStatusCode(statuscode);

        }

        private string? GetMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                404 => "Not Found",
                500 => "Server Error",
                400 => "Bad Request",
                401 => "UnAuthorized",
                _ => null
            };
        }
    }
}
