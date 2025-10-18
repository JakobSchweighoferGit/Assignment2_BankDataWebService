namespace RequestsResponses
{
    public class EditUserRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string PicturePath { get; set; } = "";
        public string? Password { get; set; }
        public string Handle { get; set; }
    }
}
