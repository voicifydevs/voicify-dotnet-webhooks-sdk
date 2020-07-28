using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IDataTraversalService
    {
        /// <summary>
        /// Traverses a JObject to find the string specified by navigation
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        string Traverse(JObject jObject, string navigation);

        /// <summary>
        /// Traverses a string treated as a jobject or jarray
        /// </summary>
        /// <param name="json"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        string TraverseJSONString(string json, string navigation);
        /// <summary>
        /// Traverses a HtmlNode to find the string specified by navigation
        /// </summary>
        /// <param name="node"></param>
        /// <param name="navigation"></param>
        /// <returns></returns>
        string Traverse(HtmlNode node, string navigation);
    }
}
