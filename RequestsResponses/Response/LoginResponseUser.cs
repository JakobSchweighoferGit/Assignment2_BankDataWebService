namespace PresentationLayerAPI.Models.Response
{
    public class LoginResponseUser
    {
        public string Handle { get; set; }
        public bool Admin { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
