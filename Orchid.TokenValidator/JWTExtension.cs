using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchid.TokenValidator
{
    public static class JWTExtension
    {
        /// <summary>
        /// Create JWT Token Validation Mehanism
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="builder"></param>
        public static void AddJwtAuthentication(this IServiceCollection services, IWebHostEnvironment environment, IConfigurationBuilder builder)
        {
            // Hard Coded file should be ENVironment Specific
            IConfigurationRoot _config = builder.Build();
            _ = services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // validate the server
                        ValidateAudience = true, // Validate the recipient of token is authorized to receive
                        ValidateLifetime = true, // Check if token is not expired and the signing key of the issuer is valid 
                        ValidateIssuerSigningKey = true, // Validate signature of the token 

                        //Issuer and audience values are same as defined in generating Token
                        ValidIssuer = _config["JWT-ISSUER"], // stored in appsetting file
                        ValidAudience = _config["JWT-ISSUER"], // stored in appsetting file
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT-SECRET"]!)), // stored in appsetting file
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Audience = _config["JWT-AUDIENCE"];
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var claims = context.Principal.Claims;

                            var userClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            if (userClaim == null) return;

                            var claimValues = userClaim.Value.Split('|');
                            if (claimValues.Length != 2) return;

                            var tokenUsername = claimValues[0];
                            var tokenUserType = claimValues[1];

                            var httpContext = context.HttpContext;

                            string requestUsername = string.Empty;
                            string requestUserType = string.Empty;
                            string requestctxuser = string.Empty;

                            // param in Header
                            if (context.Request.Headers.ContainsKey("X-UserName") && !string.IsNullOrEmpty(context.Request.Headers["X-UserName"]))
                            {
                                requestUsername = context.Request.Headers["X-UserName"]!;
                            }
                            if (context.Request.Headers.ContainsKey("X-UserType") && !string.IsNullOrEmpty(context.Request.Headers["X-UserType"]))
                            {
                                requestUserType = context.Request.Headers["X-UserType"]!;
                            }

                            if (string.IsNullOrEmpty(requestUsername) || string.IsNullOrEmpty(requestUserType))
                            {
                                if (httpContext.Request.Method == HttpMethods.Get)
                                {
                                    if (context.Request.QueryString.HasValue) // param in querystring
                                    {
                                        requestUsername = context.Request.Query["username"].ToString();
                                        requestUserType = context.Request.Query["userType"].ToString();
                                        requestctxuser = context.Request.Query["ctxuser"].ToString();

                                        requestUsername = string.IsNullOrEmpty(requestUsername) ? requestctxuser : requestUsername;
                                    }
                                }
                                if (context.Request.ContentLength > 0) // param in body
                                {
                                    context.Request.EnableBuffering();

                                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                                    {
                                        var body = await reader.ReadToEndAsync();

                                        if (!string.IsNullOrEmpty(body))
                                        {
                                            if (body.ToLower().Contains("username") || body.ToLower().Contains("ctxuser"))
                                            {
                                                var userInfo = JsonConvert.DeserializeObject<UserInfo>(body);

                                                if (userInfo != null)
                                                {
                                                    requestUsername = string.IsNullOrEmpty(userInfo.UserName) ? userInfo.CtxUser : userInfo.UserName;
                                                    requestUserType = userInfo.UserType;
                                                }
                                            }
                                        }
                                    }
                                    context.Request.Body.Position = 0;
                                }
                            }

                            if (requestUsername.ToLower() != tokenUsername.ToLower() || requestUserType.ToLower() != tokenUserType.ToLower())
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Fail("Username or UserType does not match the token claims.");
                            }

                            return;
                        }
                    };
                });
        }
        public static string CreateToken(string username, string UserType, IConfiguration configuration, DateTime? Tokenexpires = null)
        {
            try
            {
                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                //set the time when it expires
                DateTime expires = Tokenexpires.HasValue ? Tokenexpires.Value : DateTime.UtcNow.AddDays(Convert.ToDouble(configuration.GetSection("ExpiryDays").Value));

                //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();

                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username+"|"+UserType)
            });

                string sec = configuration["JWT-SECRET"];
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
                var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


                //create the jwt
                var token =
                    (JwtSecurityToken)
                        tokenHandler.CreateJwtSecurityToken(issuer: configuration["JWT-ISSUER"], audience: configuration["JWT-AUDIENCE"],
                            subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
    public record UserInfo
    {
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string CtxUser { get; set; }
    }
}
