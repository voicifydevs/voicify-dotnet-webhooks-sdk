using System;
using System.Collections.Generic;
using System.Text;
using Voicify.Sdk.Core.Models.Content;
using Voicify.Sdk.Core.Models.Model;
using Voicify.Sdk.Core.Models.Webhooks.Responses;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IResponseBuilder
    {
        WebhookResponse<ContentFulfillmentWebhookData> BuildResponse();
        IResponseBuilder FlushFollowUp();
        IResponseBuilder WithFollowUp(Action<IFollowUpBuilder> buildFollowUp);
        IResponseBuilder WithContent(string content);
        IResponseBuilder WithGeneralResponseContent(GeneralResponseModel response);
        IResponseBuilder WithMediaResponse(MediaResponseModel model);
        IResponseBuilder WithReprompt(RepromptModel model);
        IResponseBuilder WithBackgroundImage(string url);
        IResponseBuilder WithSmallImage(string url);
        IResponseBuilder WithLargeImage(string url);
        IResponseBuilder WithVideo(string url);
        IResponseBuilder WithDisplayTitle(string title);
        IResponseBuilder WithDisplayText(string text);
        IResponseBuilder WithAudio(string url);
        IResponseBuilder WithReplacement(string key, string value);
        IResponseBuilder WithReplacements(Dictionary<string, string> dictionary);
        IResponseBuilder WithSessionAttributes(string key, object value);
    }
}
