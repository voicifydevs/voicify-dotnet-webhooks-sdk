using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Webhooks.Requests;
using Voicify.Sdk.Core.Models.Webhooks.Responses;
using Voicify.Sdk.Webhooks.Services;

namespace Voicify.Samples.Webhooks.Controllers
{
    [Route("api/[controller]")]
    public class BasicSamplesController : Controller
    {
        /// <summary>
        /// Replaces the content property / output speech with simple object mapping
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReplaceContent")]
        public IActionResult ReplaceContent([FromBody]GeneralWebhookFulfillmentRequest request)
        {
            return Ok(new GeneralFulfillmentResponse
            {
                Data = new ContentFulfillmentWebhookData
                {
                    Content = "This is now the output speech"
                }
            });
        }

        /// <summary
        /// Replaces the content property / output speech with a response builder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReplaceContentResponseBuilder")]
        public IActionResult ReplaceContentResponseBuilder([FromBody]GeneralWebhookFulfillmentRequest request)
        {
            var builder = new ResponseBuilder();
            builder.WithContent("This is now the output speech");
            return Ok(builder.BuildResponse());
        }
    }
}
