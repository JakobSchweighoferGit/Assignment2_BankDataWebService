using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using PresentationLayerAPI.Models.Response;
using RequestsResponses;
using RestSharp;

namespace PresentationLayerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBankAccountManagementController : Controller
    {
        
        [HttpPost("adminBankAccountEditInformation")]
        public IActionResult GetBankAccountformation([FromBody] GetBankAccountinformationRequest data)
        {
            Console.WriteLine("ID: " + data.AccountID.ToString());
            try
            {
                RestClient client = new RestClient("http://localhost:5295");
                RestRequest request = new RestRequest("/api/GetBankAccountInformationById", Method.Post);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                AccountDetailsResponse BankAccountInformation = JsonConvert.DeserializeObject<AccountDetailsResponse>(response.Content);

                if (BankAccountInformation == null) {
                    return PartialView("~/Views/Login/LoginErrorView.cshtml");
                }
                Console.WriteLine(BankAccountInformation.Balance.ToString());
                Console.WriteLine("RAW: " + response.Content);



                return PartialView("~/Views/Admin/AdminEditBankAccountInformationView.cshtml", BankAccountInformation);

            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Login/LoginErrorView.cshtml");
            }
        }

        [HttpPost("adminBankAccountUpdate")]
        public IActionResult UpdateBankAccount([FromBody] EditBankAccountRequest data)
        {
            try
            {
                var client = new RestClient("http://localhost:5295");
                var request = new RestRequest("/api/EditBankAccount", Method.Post);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                SucessResponse result = JsonConvert.DeserializeObject<SucessResponse>(response.Content);

                if (!result.Success)
                {
                    var msg = string.IsNullOrWhiteSpace(result.Message) ? "Update failed" : result.Message;
                    return BadRequest(new { success = false, message = msg });
                }

                return Ok(new { success = true, message = "Account updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Unexpected error" });
            }
        }
    }
}
