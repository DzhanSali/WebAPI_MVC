using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Static_Vars;

namespace WebMVC.Controllers
{
    public class PersonController_MVC : Controller
    {

        public ActionResult Index()
        {
            IEnumerable<PersonModel_MVC> personList;
            HttpResponseMessage response = StatClient.WebAPIClient.GetAsync("Person").Result;
            personList = response.Content.ReadAsAsync<IEnumerable<PersonModel_MVC>>().Result;

            return View(personList);
        }

    }
}
