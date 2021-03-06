using Blazored.LocalStorage;
using Catalogo_Blazor.Client.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalogo_Blazor.Client.Auth
{
    public class TokenAuthenticationProvider : AuthenticationStateProvider, IAuthorizeService
    {
        //private readonly IJSRuntime js;
        private readonly HttpClient http;
        private readonly ILocalStorageService localStorage;
        //public static readonly string tokenKey = "tokenKey";
        public TokenAuthenticationProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            http = httpClient;
        }

        private AuthenticationState notAuthentication =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>("tokenKey");

            if (string.IsNullOrEmpty(token))
            {
                return notAuthentication;
            }
            
            return CreateAuthenticationState(token);
        }

        public AuthenticationState CreateAuthenticationState(string token)
        {
            // colocar o token obtido do localstorage no header do request
            // na seção authorization assim poderemos está autenticando
            // cada requisição HTTP enviada ao servidor por este cliente
            http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token);

            // extrair as claims
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(string token)
        {
            await localStorage.SetItemAsync("tokenKey", token);
            //await js.SetInLocalStorage(tokenKey, token);
            var authState = CreateAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("tokenKey");
            //await js.RemoveItem(tokenKey);
            http.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(notAuthentication));
        }
    }
}
