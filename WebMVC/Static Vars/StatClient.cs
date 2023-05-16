using System.Net.Http.Headers;

namespace WebMVC.Static_Vars
{
    public static class StatClient
    {

        public static HttpClient WebAPIClient = new HttpClient();

        static StatClient()
        {
            WebAPIClient.BaseAddress = new Uri("http://localhost:5091/api/");
            WebAPIClient.DefaultRequestHeaders.Clear();
            WebAPIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
    }
}
