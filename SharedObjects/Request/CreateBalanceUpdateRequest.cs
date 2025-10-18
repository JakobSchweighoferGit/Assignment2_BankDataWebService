namespace BusinessLayerAPI.Models.Request
{
    public class CreateBalanceUpdateRequest
    {
        public int AccountID { get; set; }

        public int IncrAmount { get; set; }
    }
}
