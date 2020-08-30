using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Template_UI.Contracts;
using Template_UI.Models;
using Template_UI.Providers;
using Template_UI.Static;

namespace Template_UI.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationRepository(
            IHttpClientFactory httpClient, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider
            )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            var requset = new HttpRequestMessage(HttpMethod.Post, Endpoints.LoginEndpoint);

            requset.Content = new StringContent(
                JsonConvert.SerializeObject(user),
                Encoding.UTF8,
                "application/json"
                );

            var client = _httpClient.CreateClient();

            HttpResponseMessage response = await client.SendAsync(requset);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var contnet = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenModel>(contnet);

            // Store token
            await _localStorage.SetItemAsync("authToken", token.Token);

            // Change auth state
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> Registration(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.RegisterEndpoint);

            request.Content = new StringContent(
               JsonConvert.SerializeObject(user),
               Encoding.UTF8,
               "application/json"
               );

            var client = _httpClient.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
