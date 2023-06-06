using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient _httpClient = StatClient.WebAPIClient;
        private readonly IConfiguration _config;
        private string _token;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private async Task<PersonModel_MVC> GetCurrentUser(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:8080/api/Login");
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var currentUser = JsonConvert.DeserializeObject<PersonModel_MVC>(jsonResult);
                return currentUser;
            }

            return null;
        }


        public async Task<IActionResult> Profile(int accountId)
        {
            var currentUser = await Service.GetUsersFromAPI();
            var user = currentUser.FirstOrDefault(p => p.Id == accountId);

            if (user != null)
            {
                Service.GetCookieFromSession(HttpContext);
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/api/Login");
            var personList = await Service.GetUsersFromAPI();

            var loginData = personList
                .Where(p => p.Name == model.Name && p.Password == model.Password)
                .Select(p => new { p.Name, p.Password })
                .FirstOrDefault();

            request.Content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _token = await response.Content.ReadAsStringAsync();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Error");
                }
            }

            bool userExists = personList.Any(p => p.Name == model.Name && p.Password == model.Password);
            if (userExists)
            {
                var currentUser = personList.FirstOrDefault(p => p.Name == model.Name && p.Password == model.Password);

                var cookieContent = $"{_token}|{currentUser.Id}";

                Response.Cookies.Append("cookie", cookieContent, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(60)
                });
                Service.GetCookieFromSession(HttpContext);
                // or alternatively
                // HttpContext.Session.SetInt32("UserId", currentUser.Id);

                return RedirectToAction("Profile", new { accountId = currentUser.Id });
            }
            else
            {
                return RedirectToAction("Error");                
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registration()
        {

            if (Service.GetUserId() != 0)
            {
                Service.DeleteCookieFromSession(HttpContext);
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Registration(RegistrationVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync("http://localhost:8080/api/Person", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to register user. Please try again.");
                    return View(model);
                }
            }
        }

        public IActionResult Logout()
        {
            Service.DeleteCookieFromSession(HttpContext);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                Path = "/"
            };
            Response.Cookies.Append("cookie", "", cookieOptions);
            Service.DeleteCookieFromSession(HttpContext);

            return RedirectToAction("index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GenerateToken(LoginVM user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Password", user.Password),
                new Claim("Name", user.Name)
            };

            var tokenOptions = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
