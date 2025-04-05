using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Arkademy.Backend
{
    public class BackendService
    {
        public static readonly string PROD = "prod";
        public static readonly string DEV = "dev";

        private static BackendService _instance;

        public Uri BaseUrl;
        public string Token;

        public static bool Offline
        {
            get
            {
                if (!Application.isEditor)
                {
                    return !Application.absoluteURL.Contains("192.168");
                }
                return Env().offline;
            }
        }

        

        public async Task<T> Get<T>(string url)
        {
            var uri = new Uri(BaseUrl, new Uri(url,UriKind.Relative));
            var request = UnityWebRequest.Get(uri);
            request.SetRequestHeader("Authorization", Token);
            await request.SendWebRequest();
            var result = request.result;
            if (result != UnityWebRequest.Result.Success)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
        }

        public async Task<UnityWebRequest> Post(string url, string payload)
        {
            var request = UnityWebRequest.Post(new Uri(BaseUrl, new Uri(url,UriKind.Relative)), payload, "application/json");
            request.SetRequestHeader("Authorization", Token);
            await request.SendWebRequest();
            return request;
        }

        public async Task<UnityWebRequest> Patch(string url, string payload)
        {
            var request = UnityWebRequest.Post(new Uri(BaseUrl, new Uri(url,UriKind.Relative)), payload, "application/json");
            request.method = "PATCH";
            request.SetRequestHeader("Authorization", Token);
            await request.SendWebRequest();
            return request;
            
        }

        public static BackendEnvironment Env()
        {
            var env = Application.isEditor ? DEV : PROD;
            return Resources.LoadAll<BackendEnvironment>("")
                .FirstOrDefault(x => x.envName == env);
        }
        private static BackendService Service
        {
            get
            {
                if (_instance != null) return _instance;
                var envConf = Env();
                if (!envConf) throw new Exception("Backend environment not found");
                var token =
                    TryGetLocalToken(out var cached) ? cached : "";
                if (!string.IsNullOrEmpty(envConf.overrideToken)) token = envConf.overrideToken;
                var service = new BackendService
                {
                    BaseUrl = new Uri(envConf.baseURL),
                    Token = token
                };
                _instance = service;
                return _instance;
            }
        }

        private static bool TryGetLocalToken(out string token)
        {
            token = "";
            if (!PlayerPrefs.HasKey("PlayerToken")) return false;
            token = PlayerPrefs.GetString("PlayerToken");
            Debug.Log($"Use cached token");
            return true;
        }

        public static async Task<bool> Login(string username, string password)
        {
            var payload = new JObject();
            payload["username"] = username;
            payload["password"] = password;
            var result = await Service.Post("login", payload.ToString());
            // var result = await Service._client.PostAsync("login",
            //     new StringContent(
            //         payload.ToString(),
            //         Encoding.UTF8,
            //         "application/json"
            //     ));
            if (result.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(result.error);
                return false;
            }

            var tokenJson = JObject.Parse(result.downloadHandler.text);
            if (!tokenJson.TryGetValue("token", out var tokenObj))
            {
                Debug.LogError("Token not found in result");
                return false;
            }

            var token = tokenObj.ToString();
            Service.Token = token;
            PlayerPrefs.SetString("PlayerToken", token);
            Debug.Log("Login successfully");
            return true;
        }

        public static async Task<bool> Register(string username, string password)
        {
            var payload = new JObject();
            payload["username"] = username;
            payload["password"] = password;
            var result = await Service.Post("register", payload.ToString());
            if (result.result!=UnityWebRequest.Result.Success)
            {
                Debug.LogError(result.error);
                return false;
            }

            return true;
        }

        public static async Task<User> GetUser()
        {
            var result = await Service.Get<User>("user");
            return result;
        }

        public static async Task<PlayerRecord> UpdatePlayer(Data.PlayerRecord record)
        {
            var serverRecord = record.ToServerPlayerRecord();
            var payload = JsonConvert.SerializeObject(serverRecord, new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-ddTH:mm:ssK"
            });
            Debug.Log(payload);
            var result = await Service.Patch("player", payload);

            if (result.result!=UnityWebRequest.Result.Success)
            {
                Debug.LogError(result.error);
                return null;
            }
            return JsonConvert.DeserializeObject<PlayerRecord>(result.downloadHandler.text);
        }
    }
}