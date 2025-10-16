namespace BusinessLayerAPI.Models.Request
{
    public class CreateBankAccountRequest
    {
        public string AccountNumber { get; set; } = "";
        public int Balance { get; set; }         
        public string Handle { get; set; } = ""; 
        public bool Active { get; set; } = true;
    }
}
