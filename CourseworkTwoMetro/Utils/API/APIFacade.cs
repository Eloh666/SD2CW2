using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.JSONUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CourseworkTwoMetro.Utils.API
{
    public class ApiFacade
    {
        private static HttpClient _client;
        private static string _jwt;

        public static void InitialiseApi()
        {
                _client = new HttpClient {BaseAddress = new Uri("https://coursework2api.herokuapp.com/") };
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<String> GetData(string path)
        {
            HttpResponseMessage response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public static void PostData(object data, string path)
        {
            string json = JsonConvert.SerializeObject(data);
            Console.WriteLine(json);
        }

        public static async Task<ObservableCollection<Customer>> GetCustomers()
        {
            var json = await GetData("/customer");
            return JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
        }

        public static async Task<ObservableCollection<Booking>> GetBookings()
        {
            var json = await GetData("/booking");
            return JsonConvert.DeserializeObject<ObservableCollection<Booking>>(json);
        }

        public static async Task<bool> Login(User user)
        {
            var json = LowcaseJsonKeysSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync("/auth", content);
                response.EnsureSuccessStatusCode();

                JObject jwt = JObject.Parse(await response.Content.ReadAsStringAsync());
                _jwt = (string)jwt["access_token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
                Console.WriteLine(_jwt);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}