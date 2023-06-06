namespace WebMVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string LoginError { get; set; }

        public string NotLogged { get; set; }
    }
}