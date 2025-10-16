using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentationLayerAPI.Models.Request;
using PresentationLayerAPI.Models.Response;
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
            try
            {
                RestClient client = new RestClient("http://localhost:5295");
                RestRequest request = new RestRequest("/api/GetBankAccountInformationByAccNumber", Method.Get);
                request.AddJsonBody(data);
                RestResponse response = client.Execute(request);
                AccountDetailsResponse BankAccountInformation = JsonConvert.DeserializeObject<AccountDetailsResponse>(response.Content);

                return PartialView("~/Views/Admin/AdminEditBankAccountInformationView.cshtml", BankAccountInformation);

            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Login/LoginErrorView.cshtml");
            }
        }
    }
}
