using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Template_UI_WASM.Contracts;
using Template_UI_WASM.Models;

namespace Template_UI_WASM.Service
{
    public class AuthorRepository : BaseRepository<AuthorModel>, IAuthorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthorRepository(
            HttpClient httpClient,
            ILocalStorageService localStorage
            ) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
    }
}
