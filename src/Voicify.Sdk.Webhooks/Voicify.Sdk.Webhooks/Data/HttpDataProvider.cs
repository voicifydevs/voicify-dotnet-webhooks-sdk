using Newtonsoft.Json;
using ServiceResult;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Data.Definitions;

namespace Voicify.Sdk.Webhooks.Data
{
    public class HttpDataProvider : IHttpDataProvider
    {

        protected readonly HttpClient _client;

        public HttpDataProvider(HttpClient client)
        {
            _client = client;
        }


        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostJsonAsync<T>(string url, string json)
        {
            await SetTokenAsync();
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);

        }

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostAsync<T>(string url)
        {
            Console.WriteLine($"POST 1 json async...{url}");
            await SetTokenAsync();
            var result = await _client.PostAsync(url, null);
            return await HandleResponseAsync<T>(result);

        }
        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PostAnonymousJsonAsync<T>(string url, string json)
        {
            Console.WriteLine($"POST ANON async...{url}");
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);

        }

        /// <summary>
        /// Makes an HTTP POST request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task PostJsonAsync(string url, string json)
        {
            Console.WriteLine($"POST json async...{url}");
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

        }

        /// <summary>
        /// Makes an HTTP GET Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> GetJsonAsync<T>(string url)
        {
            Console.WriteLine($"getting json async...{url}");
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.GetAsync(url);
            return await HandleResponseAsync<T>(result);

        }

        public virtual async Task<Result<T>> GetAnonymousJsonAsync<T>(string url)
        {
            Console.WriteLine($"getting ANON json async...{url}");
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.GetAsync(url);
            string b = await result.Content.ReadAsStringAsync();
            Console.WriteLine(b);
            return await HandleResponseAsync<T>(result);

        }

        /// <summary>
        /// Makes an HTTP PUT request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> PutJsonAsync<T>(string url, string json)
        {
            Console.WriteLine($"PUT json async...{url}");
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            return await HandleResponseAsync<T>(result);

        }

        public virtual async Task<Result<T>> PostFileAsync<T>(string url, string name, Stream fileData)
        {
            Console.WriteLine($"PUT json async...{url}");
            await SetTokenAsync();
            using (var content =
             new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                content.Add(new StreamContent(fileData), name);
                var result = await _client.PostAsync(url, content);
                return await HandleResponseAsync<T>(result);
            }

        }

        /// <summary>
        /// Makes an HTTP PUT request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual async Task PutJsonAsync(string url, string json)
        {
            Console.WriteLine($"PUT json async...{url}");
            await SetTokenAsync();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await _client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

        }

        /// <summary>
        /// Makes an HTTP DELETE request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual async Task<Result<T>> DeleteAsync<T>(string url)
        {
            Console.WriteLine($"DELETE async...{url}");
            await SetTokenAsync();
            var result = await _client.DeleteAsync(url);
            return await HandleResponseAsync<T>(result);

        }

        /// <summary> 
        /// Handles http response errors
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual Result<T> HandleError<T>(HttpResponseMessage response, string content)
        {

            // if unauthorized, pass that data back to sign the user out. This should eventually be handled by the base view model request handling
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new InvalidResult<T>($"Access denied: {response.StatusCode}");
            }

            // error, but no content? wing it.
            if (string.IsNullOrEmpty(content))
            {
                return new UnexpectedResult<T>();
            }

            // otherwise, start mapping errors.
            try
            {
                // try to use the full error
                var errorData = JsonConvert.DeserializeObject<IEnumerable<string>>(content);
                return new InvalidResult<T>(errorData.FirstOrDefault());

            }
            catch
            {
                return new UnexpectedResult<T>(content);
            }
        }

        /// <summary>
        /// Handles the response from an HTTP request and parses the data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>        
        /// <returns></returns>
        protected virtual async Task<Result<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            // get the content of the response and handle mapping the json data to the given type.
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                try
                {
                    var data = JsonConvert.DeserializeObject<T>(content);
                    return new SuccessResult<T>(data);

                }
                catch (Exception ex)
                {
                    return new UnexpectedResult<T>();
                }
            }
            return HandleError<T>(response, content);

        }
        protected virtual Task SetTokenAsync()
        {
            // NOTE: we can eventually handle authorization against services here
            return Task.CompletedTask;
        }
    }

}
