using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class BookController_MVC : Controller
    {
        private static readonly HttpClient _httpClient = StatClient.WebAPIClient;
        private int _loginId = Service.GetUserId();
        // or alternatively
        //int? loginId = HttpContext.Session.GetInt32("UserId");

        public IActionResult Index(string search, int page = 1, int pageSize = 5)
        {
            if (_loginId > 0)
            {
                IEnumerable<BookModel_MVC> bookList;
                HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("Books").Result;
                bookList = response.Content.ReadAsAsync<IEnumerable<BookModel_MVC>>().Result;

                bookList = bookList.Where(book => book.PersonId == _loginId);

                if (!string.IsNullOrEmpty(search))
                {
                    bookList = bookList.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
                }

                int totalItems = bookList.Count();
                int totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);

                bookList = bookList.Skip((page - 1) * pageSize).Take(pageSize);

                ViewData["CurrentPage"] = page;
                ViewData["PageSize"] = pageSize;
                ViewData["TotalItems"] = totalItems;
                ViewData["TotalPages"] = totalPages;

                return View(bookList.ToList());
            }
            else
            {
                ErrorViewModel errorViewModel = new ErrorViewModel();
                errorViewModel.NotLogged = "You are not logged in!";
                return View("Error", errorViewModel);
            }
        }


        public IActionResult CreateBook()
        {
            BookModel_MVC book = new BookModel_MVC();
            book.PersonId = _loginId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(BookModel_MVC book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            else
            {
                book.PersonId = _loginId;

                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync("http://localhost:8080/api/Books", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to register user. Please try again.");
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        public IActionResult EditBook(int bookid)
        {
            HttpResponseMessage response = _httpClient.GetAsync("http://localhost:8080/api/Books/" + bookid).Result;

            if (response.IsSuccessStatusCode)
            {
                var book = response.Content.ReadAsAsync<BookModel_MVC>().Result;
                return View(book);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public IActionResult EditBook(BookModel_MVC book)
        {
            book.PersonId = _loginId;
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("http://localhost:8080/api/Books/" + book.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "BookController_MVC");
            }
            else
            {
                ModelState.AddModelError("", "Failed to register user. Please try again.");
                return RedirectToAction("Error");
            }
        }

    }
}

