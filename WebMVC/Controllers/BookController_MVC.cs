using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class BookController_MVC : Controller
    {
        public IActionResult Index()
        {

            IEnumerable<BookModel_MVC> bookList;
            HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("Book").Result;
            bookList = response.Content.ReadAsAsync<IEnumerable<BookModel_MVC>>().Result;

            return View(bookList);
        }
    }
}
