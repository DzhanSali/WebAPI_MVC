using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class ReadBooksController_MVC : Controller
    {
        private static readonly HttpClient _httpClient = StatClient.WebAPIClient;
        private int _loginId = Service.GetUserId();
        private int pagination;

        public IActionResult Index(string search, int page = 1, int pageSize = 5)
        {
            if (_loginId > 0)
            {
                IEnumerable<ReadBooksModel_MVC> bookList;
                HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("ReadBooks").Result;
                bookList = response.Content.ReadAsAsync<IEnumerable<ReadBooksModel_MVC>>().Result;

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

        [HttpGet]
        public IActionResult EditReadBook(int bookId)
        {
            {
                HttpResponseMessage response = _httpClient.GetAsync("http://localhost:8080/api/ReadBooks/" + bookId).Result;

                if (response.IsSuccessStatusCode)
                {
                    var book = response.Content.ReadAsAsync<ReadBooksModel_MVC>().Result;
                    return View(book);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPut]
        public IActionResult EditReadBook(ReadBooksModel_MVC book)
        {
            book.PersonId = _loginId;
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("http://localhost:8080/api/ReadBooks/" + book.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to register user. Please try again.");
                return RedirectToAction("Error");
            }
        }


        public IActionResult Pagination(int num)
        {
            pagination = num;
            return RedirectToAction("Index");
        }

    }
}
