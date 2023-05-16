using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IEnumerable<PersonModel_MVC> GetUsersFromAPI()
        {
            HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("Person").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<IEnumerable<PersonModel_MVC>>().Result;
            }
            else
            {
                return Enumerable.Empty<PersonModel_MVC>();
            }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {

            if (!this.ModelState.IsValid)
                return View(model);

            IEnumerable<PersonModel_MVC> personList = GetUsersFromAPI();
            bool userExists = personList.Any(p => p.Name == model.Name && p.Password == model.Password);

            if (userExists)
            {
                return RedirectToAction("Index", "PersonController_MVC");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
        }

        public IActionResult Registration()
        {

            return View();
            
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
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = StatClient.WebAPIClient.BaseAddress;

                    // Send the POST request to the API
                    var response = client.PostAsync("Person", content).Result;

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
    }
}