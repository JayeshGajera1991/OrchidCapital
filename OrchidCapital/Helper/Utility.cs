using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Serialization;

namespace OrchidCapital.Helper
{
    public static class Utility
    {
        private static IConfiguration _configuration;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static string GetAppSettings(string apiUrl)
        {
            var value = string.Empty;
            if (_configuration != null)
            {
                value = _configuration[apiUrl]!;
            }
            return value;
        }

        public static string GetAssets(int Option, string FileName = "")
        {
            switch (Option)
            {
                case 0://With Version
                    //return string.Format("{0}{1}?v={2}", _configuration["CDNPath"]!, FileName, _configuration["CDN-VERSION"]!);
                    return string.Format("{0}{1}?v={2}", _configuration["CDNPath"]!, FileName, DateTime.Now.ToString("yyyyMMdd"));
                    break;
                case 1://Without Version
                    return string.Format("{0}{1}", _configuration["CDNPath"]!, FileName);
                    break;
                default://Base CDN Path
                    return string.Format("{0}", _configuration["CDNPath"]!);
                    break;
            }
        }

        public static string GetBase64Image(string path)
        {
            if (System.IO.File.Exists(path))
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(path);
                return $"data:image/png;base64,{Convert.ToBase64String(imageArray)}";
            }
            else
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(Utility.GetAssets(1, "img/") + "userProfile.webp");
                return $"data:image/png;base64,{Convert.ToBase64String(imageArray)}";
            }
        }

        public static string GetSplitLastValue(string val)
        {
            string value = string.Empty;
            if (val.Contains("/"))
            {
                value = val.Trim().Split("/").Last();
            }
            return value;
        }
        public static string GetSplitLastSecondValue(string val)
        {
            string value = string.Empty;
            if (val.Contains("/"))
            {
                value = val.Trim().Split("/").Reverse().Take(2).Last();
            }
            return value;
        }

        public static string TrimToBasePath(string fullUrl)
        {
            var uri = new Uri(fullUrl);
            var segments = uri.Segments;

            // Ensure there are enough segments to remove
            if (segments.Length <= 1)
                return uri.GetLeftPart(UriPartial.Authority) + "/";

            // Remove the last two segments (e.g., "Edocs/" and "Index")
            var newPath = string.Concat(segments.Take(segments.Length - 2));

            // Reconstruct the new URI
            var builder = new UriBuilder(uri)
            {
                Path = newPath
            };

            return builder.Uri.ToString();
        }

        #region Cookie Manage
        public static void SetCookie(string key, string value, int? expireTime, IHttpContextAccessor _contextAccessor)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddDays(expireTime.Value).AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(expireTime.Value).AddMilliseconds(10);
            }
            _contextAccessor.HttpContext.Response.Cookies.Append(key, Encryption.Encrypt(_configuration["USR-ENC-KEY"], value), option);
        }

        public static string GetCookie(string key, IHttpContextAccessor _contextAccessor)
        {
            string cartId = _contextAccessor.HttpContext.Request.Cookies[key];
            if (cartId != null)
            {
                return Encryption.Decrypt(_configuration["USR-ENC-KEY"], _contextAccessor.HttpContext.Request.Cookies[key]);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void RemoveCookie(string key, IHttpContextAccessor _contextAccessor)
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(key);
        }
        #endregion

        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

                using (StringReader stringReader = new System.IO.StringReader(xmlText))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch
            {
                throw;
            }
        }

        public static string FirstCharToUpper(string str)
        {
            return !string.IsNullOrEmpty(str) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()) : string.Empty;
        }

        public static string GetDateFormat()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }
    }
}
