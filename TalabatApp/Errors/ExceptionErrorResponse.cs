namespace TalabatApp.Errors
{
    public class ExceptionErrorResponse : ApiErrorResponse
    {
        public string? Details { get; set; }

        public ExceptionErrorResponse(int statuscode, string? message = null, string? details = null): base(statuscode, message)
        {
            Details = details; 
        }

    }
}
