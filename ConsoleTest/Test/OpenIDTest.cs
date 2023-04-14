using LitJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleTest.Test
{
    public class OpenIDTest
    {
        public OpenIDTest()
        {
            while (true)
            {
                Run().ContinueWith(t =>
                {
                    Console.WriteLine(t.Exception);
                }).Wait();

                Console.WriteLine("Continue? Enter");
                Console.ReadLine();
            }
        }

        public class OpenIdEndPoint
        {
            public string issuer;
            public string jwks_uri;
            public string authorization_endpoint;
            public string token_endpoint;
            public string userinfo_endpoint;
            public string end_session_endpoint;
            public string check_session_iframe;
            public string revocation_endpoint;
            public string introspection_endpoint;
            public string device_authorization_endpoint;
            public bool frontchannel_logout_supported;
            public bool frontchannel_logout_session_supported;
            public bool backchannel_logout_supported;
            public bool backchannel_logout_session_supported;
            public string[] scopes_supported;
            public string[] claims_supported;
            public string[] grant_types_supported;
            public string[] response_types_supported;
            public string[] response_modes_supported;
            public string[] token_endpoint_auth_methods_supported;
            public string[] id_token_signing_alg_values_supported;
            public string[] subject_types_supported;
            public string[] code_challenge_methods_supported;
            public bool request_parameter_supported;
            public string[] request_object_signing_alg_values_supported;
            public bool authorization_response_iss_parameter_supported;
        }

        public class OpenIdToken
        {
            public string id_token;
            public string access_token;
            public string refresh_token;
            public string token_type;
            public string scope;
            public int expires_in;
        }

        public class AppInfo
        {
            public string appId = "ABCDEFG";
            public string appSecret = "abc";
            public string redirect_uri = "https://localhost:17174/Identity/Account/Authenticated";
            public string logout_uri = "https://localhost:17174/Identity/Account/Authenticated";
            public string scope = "offline_access openid profile phone email address abc roles scopes";
            public string response_type = "code";
            public string state = "abc";
            public string monce = "123";
        }

        private async Task Run()
        {
            AppInfo app = new AppInfo();
            Uri baseUri = new Uri("https://localhost:17174");
            Uri destination = new Uri(baseUri, "/api/Auth");

            if (false)
            {
                //app.appId = "Project1";
                //app.appSecret = null;
                //app.state = null;
                app.redirect_uri = "https://localhost:44423/Identity/Account/Authenticated";
                //app.scope = "Project1API openid profile";
                baseUri = new Uri("https://localhost:44423/");
                destination = new Uri(baseUri, "/weatherforecast");
            }

            var client = GetClient();
            var json = await client.GetStringAsync(new Uri(baseUri, "/.well-known/openid-configuration"));
            var endPoints = JsonConvert.DeserializeObject<OpenIdEndPoint>(json);

            Dictionary<string, string> param;
            Uri uri;
            OpenIdToken token = null;

            string id = "mrsions@gmail.com";
            string pw = "tkdcjf89";
            //id = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(id))
            {
                //pw = Console.ReadLine();
                param = new Dictionary<string, string>();
                param["client_id"] = app.appId;
                param["client_secret"] = app.appSecret;
                param["grant_type"] = "password";
                param["scope"] = app.scope;
                param["redirect_uri"] = app.redirect_uri;
                param["username"] = id;
                param["password"] = pw;
                RemoveEmpty(param);
                //uri = MakeUri(endPoints.token_endpoint, param);
                var response = await client.PostAsync(new Uri(endPoints.token_endpoint), new FormUrlEncodedContent(param));
                json = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<OpenIdToken>(json);
            }
            else
            {
                //---------------------------------------------------------------
                param = new Dictionary<string, string>();
                param["client_id"] = app.appId;
                param["scope"] = app.scope;
                param["response_type"] = app.response_type;
                param["redirect_uri"] = app.redirect_uri;
                param["state"] = app.state;
                param["monce"] = app.monce;
                RemoveEmpty(param);
                uri = MakeUri(endPoints.authorization_endpoint+"/callback", param);
                Console.WriteLine("URI: " + uri.AbsoluteUri);
                //OpenUrl(uri.AbsoluteUri);
                //Process.Start(uri.AbsoluteUri);

                //---------------------------------------------------------------
                Console.WriteLine("로그인완료 후 url을 입력 해 주세요.");
                json = "https://localhost:7174/Identity/Account/Authenticated?code=7C7CC269CBE487E1C08AE2791A18A337C0C525F23FC2A8E29398586C31540E0F&scope=offline_access%20openid%20profile&state=abc&session_state=cY-GlN7qGvixyp4JIRhjtnm89vKsSyFkzkLgRwleGq8.9EA392C45B6BF0A0DEBC55F33079A2E8&iss=https%3A%2F%2Flocalhost%3A7174";
                json = Console.ReadLine();
                uri = new Uri(json);
                var uriQueries = HttpUtility.ParseQueryString(uri.Query);
                var code = uriQueries["code"];
                var state = uriQueries["state"];
                var scope = uriQueries["scope"];
                var session_state = uriQueries["session_state"];
                var iss = uriQueries["iss"];

                //---------------------------------------------------------------
                param = new Dictionary<string, string>();
                param["client_id"] = app.appId;
                param["client_secret"] = app.appSecret;
                param["grant_type"] = "authorization_code";
                param["scope"] = app.scope;
                param["redirect_uri"] = app.redirect_uri;
                param["code"] = code;
                RemoveEmpty(param);
                //uri = MakeUri(endPoints.token_endpoint, param);
                var response = await client.PostAsync(new Uri(endPoints.token_endpoint), new FormUrlEncodedContent(param));
                json = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<OpenIdToken>(json);
            }

            //---------------------------------------------------------------

            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine();

            // set bearer
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

            // userinfo
            json = await client.GetStringAsync(endPoints.userinfo_endpoint);
            var user = JsonConvert.DeserializeObject(json);
            Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));

            // check session
            json = await client.GetStringAsync(endPoints.check_session_iframe);

            // get cookie
            var rs = await client.GetAsync(new Uri(baseUri, "/api/Auth/AccessToken2Cookie"));
            if(!rs.IsSuccessStatusCode)
            {
                Console.WriteLine("ERRROR!!!!!!!!!!!!!!!");
                return;
            }

            //---------------------------------------------------------------
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            //string v = await client.GetStringAsync(destination);
            client.DefaultRequestHeaders.Authorization = null;
            string v = await client.GetStringAsync(destination);
            string v2 = await client.GetStringAsync("https://localhost:7000/WeatherForecast");
        }

        private void RemoveEmpty(Dictionary<string, string> param)
        {
            var list = new List<string>();
            foreach (var pair in param)
            {
                if (string.IsNullOrWhiteSpace(pair.Key) ||
                    string.IsNullOrWhiteSpace(pair.Value))
                {
                    list.Add(pair.Key);
                }
            }
            foreach (var key in list)
            {
                param.Remove(key);
            }
        }

        private Uri MakeUri(string url, Dictionary<string, string> param)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(url);

            if (param != null && param.Count > 0)
            {
                bool first = true;
                foreach (var pair in param)
                {
                    if (string.IsNullOrWhiteSpace(pair.Key)
                        || string.IsNullOrWhiteSpace(pair.Value))
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                        sb.Append("?");
                    }
                    else
                    {
                        sb.Append("&");
                    }

                    sb.Append(Uri.EscapeUriString(pair.Key)).Append("=").Append(Uri.EscapeUriString(pair.Value));
                }
            }
            return new Uri(sb.ToString(), UriKind.Absolute);
        }

        private static HttpClient GetClient()
        {
            var _clientHandler = new HttpClientHandler();
            _clientHandler.UseCookies = true;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            }
            catch { }
            try
            {
                _clientHandler.ServerCertificateCustomValidationCallback = (a, b, c, d) => true;
            }
            catch { }

            return new HttpClient(_clientHandler);
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
