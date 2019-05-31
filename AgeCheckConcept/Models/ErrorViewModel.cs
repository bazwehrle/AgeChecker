namespace AgeCheckConcept.Models
{
    public class ErrorViewModel
    {
        public string ErrorMessage { get; set; }

        public string ErrorDetail { get; set; }

        public ErrorViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ErrorViewModel(string errorMessage, string errorDetails)
        {
            ErrorMessage = errorMessage;
            ErrorDetail = errorDetails;
        }
    }
}