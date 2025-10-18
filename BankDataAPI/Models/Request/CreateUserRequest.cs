namespace BusinessLayerAPI.Models.Request
{
    public class CreateUserRequest
    {
        public string Handle { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = ""; 
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? PicturePath { get; set; } = "";
        public bool Admin { get; set; }

        public CreateUserRequest(string handle, string firstName, string lastName, string email, string password, string address, string phone, string picturePath, bool admin)
        {
            Handle = handle;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Address = address;
            Phone = phone;
            PicturePath = picturePath;
            Admin = admin;
        }
    }
}
