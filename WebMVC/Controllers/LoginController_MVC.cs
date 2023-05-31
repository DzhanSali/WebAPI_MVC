/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController_MVC : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Auth()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var currentUser = await GetCurrentUser(token);
            return Ok(currentUser);
        }

        private async Task<PersonModel_MVC> GetCurrentUser(string token)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            *//*if (identity != null)
            {
                var userClaims = identity.Claims;
                // Parsing because they come as strings from the API
                int userId;
                short userAge;
                char userGender;

                int.TryParse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value, out userId);
                short.TryParse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.DateOfBirth)?.Value, out userAge);
                char.TryParse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Gender)?.Value, out userGender);

                return new PersonModel_MVC
                {
                    Id = userId,
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Age = userAge,
                    Address = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.StreetAddress)?.Value,
                    Gender = userGender,
                    PhoneNumber = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.HomePhone)?.Value,

                };
                
            }*//*

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:5091/api/Login/Auth");
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var currentUser = JsonConvert.DeserializeObject<PersonModel_MVC>(jsonResult);
                return currentUser;
            }

            return null;
        }
    }
}
*/