using System.Data;
using Dapper;
using Orchid.DataAccess;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;

namespace OrchidCapitalCoreAPI.Repository
{
    public class DashboardRepository: IDashboardRepository
    {
        private readonly IDapperRepository _dapperRepository;
        private readonly IConfiguration _configuration;

        public DashboardRepository(IConfiguration configuration, IDapperRepository dapperRepository)
        {
            _configuration = configuration;
            _dapperRepository = dapperRepository;
            _dapperRepository.ConnectionString = SwitchConnectionString.PchaseConnectionstring;
            _dapperRepository.DB_NAME = configuration["DB_NAME"]!;
        }

        public async Task<CommonResponse> GetToolsCountsList(string userName, string userRole)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var query = "APortal_Dashboard";
                var param = new DynamicParameters();
                param.Add("@Action", "count");
                param.Add("@UserName", userName);
                param.Add("@UserRole", userRole);
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