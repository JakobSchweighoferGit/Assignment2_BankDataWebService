namespace BusinessLayerAPI.Models.Response
{
    public class UserLoginDataResponse
    {
        public string Handle { get; set; }
        public bool Admin { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
