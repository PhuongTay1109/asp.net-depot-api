using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/sign")]
    public class SignController : ControllerBase
    {
        private readonly string _secret = "your_secret_key";
        private readonly string _agent = "your_agent_code";

        [HttpGet]
        public IActionResult GenerateSign()
        {
            var time = DateTime.ParseExact("2024-07-29T11:25:52", "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            return Ok(ComputeSignature(time, _agent, _secret));
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
    }
}