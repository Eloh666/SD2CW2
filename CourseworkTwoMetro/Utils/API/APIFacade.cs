using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.Utils.JSONUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CourseworkTwoMetro.Utils.API
{
    public class ApiFacade
    {
        //private const string Host = "https://coursework2api.herokuapp.com/";
        private const string Host = "http://127.0.0.1:5000";

        private static HttpClient _client;
        private static string _jwt;


        public static void InitialiseApi()
        {
            _client = new HttpClient { BaseAddress = new Uri(Host) };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

        public static async Task<string> GetData(string path)
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

        public static async Task<bool> DeleteBooking(Booking booking)
        {
            string json = MyJsonSerializer.Serialize(booking);
            return await DeleteItem(json, "/booking");
        }

        public static async Task<bool> DeleteCustomer(Customer customer)
        {
            string json = MyJsonSerializer.Serialize(customer);
            return await DeleteItem(json, "/customer");
        }

        public static async Task<bool> DeleteItem(string json, string path)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpRequestMessage request = new HttpRequestMessage { Content = content, Method = HttpMethod.Delete, RequestUri = new Uri(Host + path) };
                await _client.SendAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Booking> SaveBooking(Booking booking, bool shouldPutInsteadOfPost)
        {
            string json = MyJsonSerializer.Serialize(booking);
            var savedCustomerData = shouldPutInsteadOfPost ? await PutData(json, "/booking") : await PostData(json, "/booking"); ;
            try
            {
                return JsonConvert.DeserializeObject<Booking>(savedCustomerData);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<Customer> SaveCustomer(Customer customer, bool shouldPutInsteadOfPost)
        {
            string json = MyJsonSerializer.Serialize(customer);
            var savedCustomerData = shouldPutInsteadOfPost ? await PutData(json, "/customer") : await PostData(json, "/customer");;
            try
            {
                return JsonConvert.DeserializeObject<Customer>(savedCustomerData);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string> PostData(string json, string path)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync(path, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return "An error has occurred when trying to save the data";
            }
        }

        public static async Task<string> PutData(string json, string path)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(path, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> Login(User user)
        {
            var json = MyJsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync("/auth", content);
                response.EnsureSuccessStatusCode();

                JObject jwt = JObject.Parse(await response.Content.ReadAsStringAsync());
                _jwt = (string)jwt["access_token"];
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", _jwt);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}