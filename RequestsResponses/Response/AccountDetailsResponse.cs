namespace RequestsResponses
{
    public class AccountDetailsResponse
    {
        public int AccountID { get; set; }
        public string AccountNumber { get; set; } = "";
        public int Balance { get; set; }
        public bool Active { get; set; }
        public int UserID { get; set; }
        public string Handle { get; set; } = "";
    }
}
