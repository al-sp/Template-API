using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Template_UI.Contracts;

namespace Template_UI.Service
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClient;
        public BaseRepository(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Create(string url, T obj)
        {
            if (obj == null)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(obj));

            var client = _httpClient.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
            {
                return false;
            }

            var requset = new HttpRequestMessage(HttpMethod.Delete, url + id);
            var client = _httpClient.CreateClient();
            HttpResponseMessage response = await client.SendAsync(requset);

            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<T> Get(string url, int id)
        {
            if (id < 1)
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _httpClient.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            } 
            else
            {
                return null;
            }
        }

        public async Task<IList<T>> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClient.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IList<T>>(content);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Update(string url, T obj)
        {
            if (obj == null)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(obj), 
                Encoding.UTF8, 
                "application/json"
                );

            var client = _httpClient.CreateClient();
            HttpResponseMessage responce = await client.SendAsync(request);

            if (responce.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
