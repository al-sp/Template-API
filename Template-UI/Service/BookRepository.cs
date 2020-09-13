using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Template_UI.Contracts;
using Template_UI.Models;

namespace Template_UI.Service
{
    public class BookRepository : BaseRepository<BookModel>, IBookRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorage;
        public BookRepository(
            IHttpClientFactory httpClient, 
            ILocalStorageService localStorage
            ) : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
    }
}
