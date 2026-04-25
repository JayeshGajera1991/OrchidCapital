using System.Data;
using Dapper;
using Orchid.DataAccess;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;
namespace OrchidCapitalCoreAPI.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IConfiguration _configuration;
        public AdminRepository(IConfiguration configuration, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dapperRepository.ConnectionString = SwitchConnectionString.PchaseConnectionstring;
            _dapperRepository.DB_NAME = configuration["DB_NAME"]!;
        }
        public async Task<CommonResponse> GetMasterList(GetMasterListRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = request.SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "SELECT");
                param.Add("@Filter", request.Filter);
                param.Add("@IsActive", request.IsActive);
                param.Add("@PageNumber", request.PageNumber);
                param.Add("@PageSize", request.PageSize);
                param.Add("@SortColumn", request.SortColumn);
                param.Add("@SortDirection", request.SortDirection);
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
        public async Task<CommonResponse> EditMasterDetails(int id,string SPName)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = SPName;
                var param = new DynamicParameters();
                param.Add("@Action", "EDIT");
                param.Add("@Id", id);
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

        public async Task<CommonResponse> DeleteMasterDetails(DeleteMasterDetailsRequest request)
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
        public async Task<CommonResponse> ChangeStatusInMasterDetails(ChangeStatusInMasterDetailsRequest request)
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
        public async Task<CommonResponse> UpdatePageDetails(UpdatePageDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_PageMst;
                var param = new DynamicParameters();
                if(request.Id == 0)
                    param.Add("@Action", "INSERT");
                else        
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@PageName", request.PageName);
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

        public async Task<CommonResponse> UpdateConfigSettingsDetails(UpdateConfigSettingsDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_ConfigSettingsMst;
                var param = new DynamicParameters();
                if(request.Id == 0)
                    param.Add("@Action", "INSERT");
                else        
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@Key", request.Key);
                param.Add("@Value", request.Value);
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

        public async Task<CommonResponse> UpdateImageConfigDetails(UpdateImageConfigDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_ImageConfigMst;
                var param = new DynamicParameters();
                if(request.Id == 0)
                    param.Add("@Action", "INSERT");
                else        
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@Headline", request.Headline);
                param.Add("@Description", request.Description);
                param.Add("@Role", request.Role);
                param.Add("@Type", request.Type);
                param.Add("@ImagePath", request.ImagePath);
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

        public async Task<CommonResponse> UpdateTeamDetails(UpdateTeamDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_OrchidCapitalTeamMst;
                var param = new DynamicParameters();
                if(request.Id == 0)
                    param.Add("@Action", "INSERT");
                else        
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@FullName", request.FullName);
                param.Add("@Headline", request.Headline);
                param.Add("@Description", request.Description1);
                param.Add("@Role", request.Role);
                param.Add("@Qualification", request.Qualification);
                param.Add("@ImagePath", request.Description2);
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

        public async Task<CommonResponse> UpdateRoleDetails(UpdateRoleDetailsRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_RoleMst;
                var param = new DynamicParameters();
                if(request.Id == 0)
                    param.Add("@Action", "INSERT");
                else        
                    param.Add("@Action", "UPDATE");
                param.Add("@Id", request.Id);
                param.Add("@Role", request.Role);
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

        public async Task<CommonResponse> UpdateUserPageRightsMapping(UpdateUserPageRightsMappingRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = ProxyAPI.APortal_UserPageRightsMappingMst;
                var param = new DynamicParameters();
                param.Add("@UserName", request.UserName);
                param.Add("@XmlData", request.XmlData);
                param.Add("@CreatedBy", request.CreatedBy);
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