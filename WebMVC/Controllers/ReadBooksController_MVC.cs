using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class ReadBooksController_MVC : Controller
    {
        public IActionResult Index()
        {

            IEnumerable<ReadBooksModel_MVC> readbooksList;
            HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("ReadBooks").Result;
            readbooksList = response.Content.ReadAsAsync<IEnumerable<ReadBooksModel_MVC>>().Result;

            return View(readbooksList);
        }
    }
    
}
