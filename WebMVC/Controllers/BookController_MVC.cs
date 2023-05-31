using Microsoft.AspNetCore.Mvc;
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
        private HomeController _controller;
        static int id = HomeController.statUserId;
        private static readonly HttpClient _httpClient = StatClient.WebAPIClient;

        public IActionResult Index()
        {
            // edit here to get only the books associated with the user

            IEnumerable<BookModel_MVC> bookList;
            HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("Books").Result;
            bookList = response.Content.ReadAsAsync<IEnumerable<BookModel_MVC>>().Result;            

            return View(bookList);
        }

        [HttpPost]
        public IActionResult Index(BookModel_MVC bookModel)
        {
            using (HttpClient client = new HttpClient())
            {
                var apiUrl = "http://localhost:5091/api/Book";

                bookModel.PersonId = id;
                var jsonBook = JsonConvert.SerializeObject(bookModel);
                var content = new StringContent(jsonBook, Encoding.UTF8, "application/json");

                content.Headers.Add("PersonId", bookModel.PersonId.ToString());

                var response = client.PostAsync(apiUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
        }



        public IActionResult CreateBook(int userId)
        {
            BookModel_MVC book = new BookModel_MVC();
            book.PersonId = userId; 
            return View(book);
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
                book.PersonId = id;

                var json = JsonConvert.SerializeObject(book);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync("http://localhost:5091/api/Books", content).Result;
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
            HttpResponseMessage response = _httpClient.GetAsync("http://localhost:5091/api/Books/" + bookid).Result;
            
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
        [IgnoreAntiforgeryToken]
        public IActionResult EditBook(BookModel_MVC book)
        {
            book.PersonId = id;
            var json = JsonConvert.SerializeObject(book);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            
            var response = _httpClient.PutAsync("http://localhost:5091/api/Books/" + book.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CreateBook");
            }
            else
            {
                ModelState.AddModelError("", "Failed to register user. Please try again.");
                return RedirectToAction("Error");
            }
        }
    }
}
