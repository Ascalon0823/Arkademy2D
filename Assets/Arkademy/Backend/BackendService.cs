using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Application = UnityEngine.Device.Application;
using AuthenticationHeaderValue = System.Net.Http.Headers.AuthenticationHeaderValue;

namespace Arkademy.Backend
{
    public class BackendService
    {
        public static readonly string PROD = "prod";
        public static readonly string DEV = "dev";

        private static BackendService _instance;

        private static BackendService Service
        {
            get
            {
                if (_instance != null) return _instance;
                var env = Application.isEditor ? DEV : PROD;
                var envConf = Resources.LoadAll<BackendEnvironment>("")
                    .FirstOrDefault(x => x.envName == env);
                if (!envConf) throw new Exception("Backend environment not found");
                var token =
                    TryGetLocalToken(out var cached) ? cached : "";
                if (!string.IsNullOrEmpty(envConf.overrideToken)) token = envConf.overrideToken;
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(envConf.baseURL)
                };

                var service = new BackendService
                {
                    _client = httpClient
                };
                service.AddToken(token);
                _instance = service;
                return _instance;
            }
        }

        private void AddToken(string token)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
        }

        private static bool TryGetLocalToken(out string token)
        {
            token = "";
            if (!PlayerPrefs.HasKey("PlayerToken")) return false;
            token = PlayerPrefs.GetString("PlayerToken");
            Debug.Log($"Use cached token");
            return true;
        }

        private HttpClient _client;

        public static async Task Login(string username, string password)
        {
            var payload = new JObject();
            payload["username"] = username;
            payload["password"] = password;
            var result = await Service._client.PostAsync("login",
                new StringContent(
                    payload.ToString(),
                    Encoding.UTF8,
                    "application/json"
                ));
            if (!result.IsSuccessStatusCode)
            {
                Debug.LogError(await result.Content.ReadAsStringAsync());
                return;
            }

            var tokenJson = JObject.Parse(await result.Content.ReadAsStringAsync());
            if (!tokenJson.TryGetValue("token", out var tokenObj))
            {
                Debug.LogError("Token not found in result");
                return;
            }

            var token = tokenObj.ToString();
            Service.AddToken(tokenObj.ToString());
            PlayerPrefs.SetString("PlayerToken", token);
            Debug.Log("Login successfully");
        }

        public static async Task<User> GetUser()
        {
            var result = await Service._client.GetAsync("user");
            if (!result.IsSuccessStatusCode)
            {
                Debug.Log(JsonConvert.SerializeObject(result.RequestMessage));
                Debug.LogError(await result.Content.ReadAsStringAsync());
                return null;
            }

            return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
        }
    }
}