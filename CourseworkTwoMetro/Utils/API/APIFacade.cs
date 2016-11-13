﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Utils.JSONUtils;
using Newtonsoft.Json.Linq;

namespace CourseworkTwoMetro.Utils.API
{
    public class ApiFacade
    {
        private static HttpClient _client;
        private static string _jwt;

        public static async Task InitialiseApi()
        {
            if (_client == null)
            {
                _client = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:5000/")};
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }


        static async Task GetCustomers()
        {
            HttpResponseMessage response = await _client.GetAsync("/customer");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
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