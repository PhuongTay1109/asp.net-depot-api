using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using api.DTOs;
using TimeZoneConverter;

namespace api.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret = "your_secret_key";
        private readonly string _agent = "your_agent_code";

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            if (context.Request.Path.StartsWithSegments("/api/depot"))
            {
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset stream position

                var request = JsonConvert.DeserializeObject<ApiRequest>(requestBody);
                var validationErrors = ValidateRequest(request);

                if (validationErrors.Any())
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var errorResponse = JsonConvert.SerializeObject(validationErrors);
                    await context.Response.WriteAsync(errorResponse);
                    return;
                }
            }

            await _next(context);
        }

        private List<Error> ValidateRequest(ApiRequest request)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(request.Time))
            {
                errors.Add(new Error { ErrorCode = -2072, Message = "Thiếu Time bắt buộc" });
            }
            else
            {
                if (!DateTimeOffset.TryParse(request.Time, out var dateTimeOffset))
                {
                    errors.Add(new Error { ErrorCode = -2067, Message = "Giá trị 'time' không hợp lệ" });
                }
                else
                {
                    var timeZoneInfo = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
                    var expectedOffset = timeZoneInfo.GetUtcOffset(dateTimeOffset.DateTime);

                    // Check if the offset of the provided DateTimeOffset matches the expected offset
                    if (dateTimeOffset.Offset != expectedOffset)
                    {
                        errors.Add(new Error { ErrorCode = -2067, Message = "Giá trị 'time' không hợp lệ (giờ asia)" });
                    }
                    else
                    {
                        // Convert DateTimeOffset to UTC
                        var requestTimeUtc = dateTimeOffset.UtcDateTime;

                        // Get current UTC time
                        var currentUtcTime = DateTime.UtcNow;

                        // Check if the time is valid and within the allowed offset
                        var timeDifference = Math.Abs((currentUtcTime - requestTimeUtc).TotalMinutes);
                        if (timeDifference > 5)
                        {
                            errors.Add(new Error { ErrorCode = -2071, Message = "Giá trị 'time' lệch quá 5 phút" });
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(request.Data))
            {
                errors.Add(new Error { ErrorCode = -2072, Message = "Thiếu Data bắt buộc" });
            }

            if (string.IsNullOrEmpty(request.Sign))
            {
                errors.Add(new Error { ErrorCode = -2072, Message = "Thiếu Sign bắt buộc" });
            }

            if (!errors.Any())
            {
                // Convert Time from request to DateTimeOffset
                var requestTime = DateTimeOffset.ParseExact(request.Time, "yyyy-MM-ddTHH:mm:ss.fffzzz", null, System.Globalization.DateTimeStyles.AssumeUniversal);

                // Calculate the expected signature using the provided time
                var computedSign = ComputeSignature(requestTime.DateTime, _agent, _secret);
                if (computedSign != request.Sign)
                {
                    errors.Add(new Error { ErrorCode = -2074, Message = "Giá trị 'signature' không đúng" });
                }
            }

            return errors;
        }



        private string ComputeSignature(DateTime time, string agent, string secret)
        {
            using (var md5 = MD5.Create())
            {
                string timeAgent = time.ToString("yyyy-MM-ddTHH:mm:ss") + agent;
                byte[] hash1 = md5.ComputeHash(Encoding.UTF8.GetBytes(timeAgent));
                string hash1String = BitConverter.ToString(hash1).Replace("-", "").ToLower();

                string finalString = secret + hash1String;
                byte[] hash2 = md5.ComputeHash(Encoding.UTF8.GetBytes(finalString));
                return BitConverter.ToString(hash2).Replace("-", "").ToLower();
            }
        }

        private class Error
        {
            public int ErrorCode { get; set; }
            public string Message { get; set; }
        }
    }
}
