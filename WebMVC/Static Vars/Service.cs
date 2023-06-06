using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebMVC.Controllers;
using WebMVC.Models;

namespace WebMVC.Static_Vars
{
    public static class Service
    {
        private static readonly HttpClient _httpClient = StatClient.WebAPIClient;
        private static string _token;
        private static int userId;

        public static string Token()
        {
            return _token;
        }

        public static string SetToken(string tok)
        {
            return _token = tok;
        }

        public static int GetUserId()
        {
            return userId;
        }

        private static int SetUserId(int usr)
        {
            userId = usr;
            return userId;
        }

        public static void GetCookieFromSession(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("cookie", out string cookieValue))
            {
                string[] parts = cookieValue.Split('|');
                if (parts.Length > 1 && int.TryParse(parts[1], out int usr))
                {
                    SetUserId(usr);
                }
                else
                {
                    SetUserId(0);
                }
            }
            else
            {
                SetUserId(0);
            }
        }

        public static void DeleteCookieFromSession(HttpContext context)
        {
            context.Response.Cookies.Delete("cookie");
            SetUserId(0);
        }
      
        public static async Task<IEnumerable<PersonModel_MVC>> GetUsersFromAPI()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080/api/Person");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<PersonModel_MVC>>(jsonResult);
                return users;
            }
            else
            {
                return Enumerable.Empty<PersonModel_MVC>();
            }
        }

    }
}