using System;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Threading.Tasks;
using CourseworkTwoMetro.Models;
using Newtonsoft.Json;


namespace CourseworkOneMetro.ViewModels.Utils
{
    public class APIFacade
    {
        private static APIFacade _instance;
        private static HttpClient _client;

        private APIFacade() { }

        static async Task InitAPI()
        {
            _client.BaseAddress = new Uri("http://127.0.0.1:5000/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        static async Task GetDataTestAsync(string path)
        {
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }

        static async Task Login()
        {

            var tempUser = new User();
            tempUser.Username = "";
            tempUser.Password = "";

            var json = JsonConvert.SerializeObject(tempUser);
            
            HttpResponseMessage response = await _client.PutAsJsonAsync("/auth", json);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            var JWT = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JWT);
        }


        public static APIFacade Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new APIFacade();
                }
                return _instance;
            }
        }



    }
}