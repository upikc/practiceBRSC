using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using parcticeAPP.ResponseModels;
using practiceAPP.UserModels;

namespace parcticeAPP
{
    public static class DataContex
    {
        static public bool ContainsNullOrWhiteSpace(string[] array)
        {
            if (array == null)
            {
                return true;
            }
            foreach (var str in array)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return true;
                }
            }
            return false;
        }
        static public int NumbBeforeSpace(string text)
        {
            return int.Parse(text.Split(' ', 2)[0]);
        }


        private static string adress = "https://localhost:7288/api/";
        public static (HttpStatusCode statusCode, LoginResponse? loginResponse) Register(RegisterModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{adress}Auth/register", data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);
                    return (response.StatusCode, loginResponse);
                }
                else
                    return (response.StatusCode, null);
            }
        }

        public static (HttpStatusCode statusCode , LoginResponse? loginResponse) Login(LoginModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{adress}Auth/login", data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);
                    return (response.StatusCode, loginResponse);
                }
                else
                    return (response.StatusCode, null);
            }

        }

        public static ExchangeUser[] GetUsersWithDetails(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.GetAsync($"{adress}users/GetUsersWithDetails").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var loginResponse = JsonSerializer.Deserialize<ExchangeUser[]>(responseContent);
                    return loginResponse;
                }
                else
                {
                    return null;
                }
            }
        }

        public static (HttpStatusCode statusCode, ExchangeUser? user) GetUserById(string token , int userId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.GetAsync($"{adress}users/GetUserById?id={userId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var user = JsonSerializer.Deserialize<ExchangeUser>(responseContent);
                    return (response.StatusCode , user);
                }
                else
                    return (response.StatusCode , null);
            }
        }

        public static HttpStatusCode UpdateUser(string token, int userId, UpdateUserModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.PutAsync($"{adress}users/UpdateUserData?id={userId}", data).Result;

                return (response.StatusCode);

            }
        }
        public static HttpStatusCode UpdateAllUserData(string token, int userId, UpdateUserModelWithPass model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.PutAsync($"{adress}users/UpdateUserDataWithPassword?id={userId}", data).Result;

                return (response.StatusCode);

            }
        }

        public static HttpStatusCode DeleteUser(string token, int userId)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = client.DeleteAsync($"{adress}users/DeleteUser?id={userId}").Result;

                return (response.StatusCode);

            }
        }

    }
}
