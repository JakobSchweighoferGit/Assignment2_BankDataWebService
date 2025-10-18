namespace RequestsResponses
{
    public class EditBankAccountRequest
    {
        public int AccountID { get; set; }
        public string AccountNumber { get; set; } = "";
        public int Balance { get; set; }
        public string Handle { get; set; } = "";
        public bool Active { get; set; }
    }
}
