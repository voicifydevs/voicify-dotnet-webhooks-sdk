using ServiceResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Voicify.Sdk.Webhooks.Data.Definitions
{
    public interface IHttpDataProvider
    {

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<Result<T>> PostJsonAsync<T>(string url, string json);

        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Result<T>> PostAsync<T>(string url);


        /// <summary>
        /// Makes an HTTP POST request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<Result<T>> PostAnonymousJsonAsync<T>(string url, string json);

        /// <summary>
        /// Makes an HTTP POST request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task PostJsonAsync(string url, string json);

        /// <summary>
        /// Makes an HTTP GET Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Result<T>> GetJsonAsync<T>(string url);

        Task<Result<T>> GetAnonymousJsonAsync<T>(string url);

        /// <summary>
        /// Makes an HTTP PUT request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<Result<T>> PutJsonAsync<T>(string url, string json);

        /// <summary>
        /// Makes an HTTP PUT request with no return data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task PutJsonAsync(string url, string json);

        /// <summary>
        /// Makes an HTTP DELETE request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Result<T>> DeleteAsync<T>(string url);


        /// <summary>
        /// Uploads the file by a stream
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        Task<Result<T>> PostFileAsync<T>(string url, string name, Stream fileData);

    }
}
