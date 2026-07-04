using System.Data;
using Dapper;
using Orchid.DataAccess;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;
namespace OrchidCapitalCoreAPI.Repository
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IConfiguration _configuration;

        public FinanceRepository(IConfiguration configuration, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dapperRepository.ConnectionString = SwitchConnectionString.PchaseConnectionstring;
            _dapperRepository.DB_NAME = configuration["DB_NAME"]!;
        }

        public async Task<CommonResponse> GetFinanceList(string UserName, string UserRole, int IsActive, int PageNumber, int PageSize, string SortColumn, string SortDirection, string SPName, string Filter = "")
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "SELECT");
                param.Add("@CreatedBy", UserName);
                param.Add("@Filter", Filter);
                param.Add("@IsActive", IsActive);
                param.Add("@PageNumber", PageNumber);
                param.Add("@PageSize", PageSize);
                param.Add("@SortColumn", SortColumn);
                param.Add("@SortDirection", SortDirection);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> EditFinanceDetails(string userName, int id, string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "EDIT");
                param.Add("@Id", id);
                param.Add("@CreatedBy", userName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> DeleteFinanceDetails(DeleteMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = request.SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "DELETE");
                param.Add("@Id", request.Id);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> ChangeStatusInFinanceDetails(ChangeStatusInMasterDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = request.SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "STATUS");
                param.Add("@Id", request.Id);
                param.Add("@IsActive", request.IsActive);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateLoanApplicationDetails(UpdateLoanApplicationDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanApplication;
                var param = new DynamicParameters();
                if (request.LoanApplicationId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@LoanApplicationId", request.LoanApplicationId);
                param.Add("@LoanProductId", request.LoanProductId);
                param.Add("@ApplicationNo", request.ApplicationNo);
                param.Add("@CustomerId", request.CustomerId);
                param.Add("@BranchId", request.BranchId);
                param.Add("@RequestedAmount", request.RequestedAmount);
                param.Add("@RequestedTenure", request.RequestedTenure);
                param.Add("@RequestedInterestRate", request.RequestedInterestRate);
                param.Add("@LoanPurpose", request.LoanPurpose);
                param.Add("@ApplicationStatus", request.ApplicationStatus);
                param.Add("@ApplicationDate", request.ApplicationDate);
                param.Add("@Remarks", request.Remarks);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetCommonList(string userName, string userRole, string option)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_FinanceCommonList;
                var param = new DynamicParameters();
                param.Add("@Option", option);
                param.Add("@UserName", userName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateLoanDocumentDetails(UpdareLoanApplicationDocumentDetails request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanDocuments;
                var param = new DynamicParameters();
                if (request.DocumentId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@DocumentId", request.DocumentId);
                param.Add("@LoanApplicationId", request.LoanApplicationId);
                param.Add("@DocumentType", request.DocumentType);
                param.Add("@DocumentName", request.DocumentName);
                param.Add("@FilePath", request.FilePath);
                param.Add("@UploadedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateLoanGuarantorDetails(UpdateGuarantorRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanGuarantor;
                var param = new DynamicParameters();
                if (request.GuarantorId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@GuarantorId", request.GuarantorId);
                param.Add("@LoanApplicationId", request.LoanApplicationId);
                param.Add("@Name", request.Name);
                param.Add("@Mobile", request.Mobile);
                param.Add("@Address", request.Address);
                param.Add("@Occupation", request.Occupation);
                param.Add("@Relationship", request.Relationship);
                param.Add("@PANNumber", request.PANNumber);
                param.Add("@AadhaarNumber", request.AadhaarNumber);
                param.Add("@UpdatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }
        public async Task<CommonResponse> UpdateLoanGuarantorDocumentDetails(GuarantorDocumentViewModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanGuarantorDocument;
                var param = new DynamicParameters();
                if (request.GuarantorDocumentId == 0)
                    param.Add("@Action", "INSERT");
                else
                    param.Add("@Action", "UPDATE");
                param.Add("@GuarantorId", request.GuarantorId);
                param.Add("@GuarantorDocumentId", request.GuarantorDocumentId);
                param.Add("@DocumentName", request.DocumentName);
                param.Add("@FilePath", request.FilePath);
                param.Add("@CreatedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> UpdateFinalLoanApplicationDetails(UpdateFinalLoanApplicationDetails request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanApplication;
                var param = new DynamicParameters();
                param.Add("@Action", "STATUS");
                param.Add("@LoanApplicationId", request.LoanApplicationId);
                param.Add("@UploadedBy", request.UserName);
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

        public async Task<CommonResponse> GetNewApplicationNumber(string userName, string userRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_LoanApplication;
                var param = new DynamicParameters();
                param.Add("@Action", "CODE");
                var queryResponse = await _dapperRepository.GetAllAsync<dynamic>(query, param, commandTimeout: null, commandType: CommandType.StoredProcedure);
                if (queryResponse.Any())
                {
                    response.StatusCode = 1;
                    response.Response = queryResponse;
                    response.Message = "";
                }
                else
                {
                    response.StatusCode = 0;
                    response.Message = "Data Not Found.";
                }
            }

            catch (Exception ex)
            {
                response.StatusCode = 0;
                response.Message = ex.Message;
                response.Response = "Internal Server Error";
            }
            return response;
        }

    }
}