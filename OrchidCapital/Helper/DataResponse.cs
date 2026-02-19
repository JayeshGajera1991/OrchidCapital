using Newtonsoft.Json;

namespace OrchidCapital.Helper
{
    public class DataResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public dynamic Response { get; set; }
        public dynamic data { get; set; }
        public dynamic detail { get; set; }
    }

    public class DataResponse<T> : DataResponse where T : class
    {
        public bool IsSuccessStatusCode
        {
            get
            {
                return StatusCode == 1;
            }
        }
        public List<T> Result
        {
            get
            {
                if (Response is null) return new List<T>();
                try
                {
                    return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(Response));
                }
                catch (JsonSerializationException)
                {
                    return new List<T>();
                }
            }
        }
        public T SingleResult
        {
            get
            {
                if (Response is null) return null;
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Response));
            }
        }
    }
}
