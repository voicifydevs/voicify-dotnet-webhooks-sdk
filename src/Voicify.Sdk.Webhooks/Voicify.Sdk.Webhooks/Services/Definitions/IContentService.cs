using ServiceResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Webhooks.Responses;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IContentFeatureService<TRequest,TContext>
    {
        Task<Result<TContext>> GetContextFromInput(string authorization, TRequest request);
        Task<Result<WebhookResponse<ContentFulfillmentWebhookData>>> HandleAsync(string authorization, TRequest request);
    }
}
