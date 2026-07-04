using Microsoft.AspNetCore.Mvc;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;
namespace OrchidCapitalCoreAPI.Controllers
{
    [Route("api/ORCHIDCAPITAL")]
    [ApiController]
    [ApiVersion(APIVersions.Version1)]

    public class FinanceController : Controller
    {
        private readonly IFinanceRepository _financeRepository;
        private readonly IConfiguration _configuration;
        public FinanceController(IFinanceRepository financeRepository, IConfiguration configuration)
        {
            _financeRepository = financeRepository;
            _configuration = configuration;
        }
        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetFinanceList")]
        public async Task<IActionResult> GetFinanceList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "")
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.GetFinanceList(UserName, UserRole, IsActive, PageNumber, PageSize, SortColumn, SortDirection, SPName, Filter);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetNewApplicationNumber")]
        public async Task<IActionResult> GetNewApplicationNumber(string UserName, string UserRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.GetNewApplicationNumber(UserName, UserRole);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/EditFinanceDetails")]
        public async Task<IActionResult> EditFinanceDetails(string UserName, string UserRole, int Id, string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.EditFinanceDetails(UserName, Id, SPName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/DeleteFinanceDetails")]
        public async Task<IActionResult> DeleteFinanceDetails([FromBody] DeleteMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.DeleteFinanceDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/ChangeStatusInFinanceDetails")]
        public async Task<IActionResult> ChangeStatusInFinanceDetails([FromBody] ChangeStatusInMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.ChangeStatusInFinanceDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateLoanApplicationDetails")]
        public async Task<IActionResult> UpdateLoanApplicationDetails([FromBody] UpdateLoanApplicationDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.UpdateLoanApplicationDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateLoanDocumentDetails")]
        public async Task<IActionResult> UpdateLoanDocumentDetails([FromBody] UpdareLoanApplicationDocumentDetails request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.UpdateLoanDocumentDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateLoanGuarantorDetails")]
        public async Task<IActionResult> UpdateLoanGuarantorDetails([FromBody] UpdateGuarantorRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.UpdateLoanGuarantorDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateLoanGuarantorDocumentDetails")]
        public async Task<IActionResult> UpdateLoanGuarantorDocumentDetails([FromBody] GuarantorDocumentViewModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.UpdateLoanGuarantorDocumentDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpPost("v{version:apiVersion}/UpdateFinalLoanApplicationDetails")]
        public async Task<IActionResult> UpdateFinalLoanApplicationDetails([FromBody] UpdateFinalLoanApplicationDetails request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.UpdateFinalLoanApplicationDetails(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }

        [MapToApiVersion(APIVersions.Version1)]
        [HttpGet("v{version:apiVersion}/GetCommonList")]
        public async Task<IActionResult> GetCommonList(string UserName, string UserRole, string Option)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _financeRepository.GetCommonList(UserName,UserRole, Option);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }
    }
}