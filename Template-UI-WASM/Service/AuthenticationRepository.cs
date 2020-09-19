using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Template_UI_WASM.Contracts;
using Template_UI_WASM.Models;
using Template_UI_WASM.Providers;
using Template_UI_WASM.Static;

namespace Template_UI_WASM.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationRepository(
            HttpClient httpClient, 
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

            var response = await _httpClient.PostAsJsonAsync(Endpoints.LoginEndpoint, user);

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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> Registration(RegistrationModel user)
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoints.RegisterEndpoint, user);
            return response.IsSuccessStatusCode;
        }
    }
}
