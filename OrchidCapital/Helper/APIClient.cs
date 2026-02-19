using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;

namespace OrchidCapital.Helper
{
    public class APIClient
    {
        private HttpClient _client;
        private string _baseUrl = "";
        private string _token = "";
        private string _userName = "";
        private string _userType = "";

        public TimeSpan Timeout { protected get; set; }

        public APIClient(string baseUrl, string token, string userName = "", string userType = "")
        {
            if (_client == null)
                _client = new HttpClient();
            _token = token;
            _userName = userName;
            _userType = userType;
            if (!string.IsNullOrEmpty(baseUrl)) _client.BaseAddress = new Uri(baseUrl);
        }

        public async Task<DataResponse<T>> GetAsync<T>(string url) where T : class
        {
            return await SendAsync<T>(url);
        }

        public async Task<DataResponse<T>> PostAsync<T>(string url, object objreq, bool IsIncreaseTimeOut = false) where T : class
        {
            return await SendAsync<T>(url, objreq, IsIncreaseTimeOut);
        }

        private async Task<DataResponse<T>> SendAsync<T>(string url, object objreq = null, bool IsIncreaseTimeOut = false) where T : class
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                if (requestMessage.Headers.Authorization is null)
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                requestMessage.Headers.Add("X-UserName", _userName);
                requestMessage.Headers.Add("X-UserType", _userType);
                if (IsIncreaseTimeOut == true)
                {
                    _client.Timeout = TimeSpan.FromMinutes(45); // Request will wait for 45 minutes
                }
                if (objreq != null)
                {
                    string requestobj = JsonConvert.SerializeObject(objreq);
                    StringContent jsonContent = new StringContent(requestobj, Encoding.UTF8, "application/json");

                    requestMessage.Method = HttpMethod.Post;
                    requestMessage.Content = jsonContent;
                }
                ServicePointManager.ServerCertificateValidationCallback = new
             RemoteCertificateValidationCallback
             (
                delegate { return true; }
             );
                HttpResponseMessage response = await _client.SendAsync(requestMessage);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<DataResponse<T>>(apiResponse);
                return result;
            }
            catch (HttpRequestException ex)
            {
                //_telemetryClient.TrackException(ex);
                throw;
            }
            catch (JsonSerializationException ex)
            {
                //_telemetryClient.TrackException(ex);
                throw;
            }
        }
    }
}
