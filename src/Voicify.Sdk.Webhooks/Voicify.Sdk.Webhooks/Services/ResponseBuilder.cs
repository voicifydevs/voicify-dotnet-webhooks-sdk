using System;
using System.Collections.Generic;
using System.Linq;
using Voicify.Sdk.Core.Models.Content;
using Voicify.Sdk.Core.Models.Model;
using Voicify.Sdk.Core.Models.Webhooks.Responses;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public class ResponseBuilder : IResponseBuilder
    {
        public WebhookResponse<ContentFulfillmentWebhookData> _response;
        private readonly IFollowUpBuilder _followUpBuilder;

        public ResponseBuilder(IFollowUpBuilder followUpBuilder)
        {
            _followUpBuilder = followUpBuilder;
            _response = new WebhookResponse<ContentFulfillmentWebhookData>
            {
                Data = EmptyResponse()
            };
        }

        public ResponseBuilder()
        {
            _followUpBuilder = new FollowUpBuilder();
            _response = new WebhookResponse<ContentFulfillmentWebhookData>
            {
                Data = EmptyResponse()
            };
        }

        public IResponseBuilder WithGeneralResponseContent(GeneralResponseModel response)
        {
            if (!string.IsNullOrWhiteSpace(response.Content))
                this.WithContent(response.Content);
            if (!string.IsNullOrWhiteSpace(response.DisplayTextOverride))
                this.WithDisplayText(response.DisplayTextOverride);
            if (!string.IsNullOrWhiteSpace(response.DisplayTitleOverride))
                this.WithDisplayTitle(response.DisplayTitleOverride);
            if (response.Audio is object)
                this.WithAudio(response.Audio.Url);
            if (response.Video is object)
                this.WithVideo(response.Video.Url);
            if (response.SmallImage is object)
                this.WithSmallImage(response.SmallImage.Url);
            if (response.LargeImage is object)
                this.WithLargeImage(response.LargeImage.Url);
            if (response.BackgroundImage is object)
                this.WithBackgroundImage(response.BackgroundImage.Url);
            if (response.Reprompt is object)
                this.WithReprompt(response.Reprompt);
            if (response.MediaResponseContainer is object)
                this.WithMediaResponse(response.MediaResponseContainer?.Responses?.FirstOrDefault());
            if (response.FollowUp is object)
            {
                this.WithFollowUp(f => {
                    var followUp = response.FollowUp;
                    f.WithContent(followUp.Content);
                    foreach (var contentItem in followUp.ChildContentContainer?.ContentItems ?? Enumerable.Empty<GenericContentModel>())
                    {
                        f.WithContentItemFollowUp(contentItem.Id, contentItem.FeatureTypeId);
                    }
                    
                });
            }
            return this;
        }

        public IResponseBuilder WithFollowUp(Action<IFollowUpBuilder> buildFollowUp)
        {
            buildFollowUp(_followUpBuilder);
            _response.Data.FollowUp = _followUpBuilder.GetFollowUp();
            return this;
        }

        public IResponseBuilder WithContent(string content)
        {
            CheckContent();
            _response.Data.Content += content;
            return this;
        }

        public IResponseBuilder WithMediaResponse(MediaResponseModel model)
        {
            _response.Data.MediaResponse = model;
            return this;
        }

        public IResponseBuilder WithReprompt(RepromptModel model)
        {
            _response.Data.Reprompt = model;
            return this;
        }

        public IResponseBuilder WithSmallImage(string url)
        {
            _response.Data.SmallImage = new MediaItemModel
            {
                Url = url
            };
            return this;
        }

        public IResponseBuilder WithLargeImage(string url)
        {
            _response.Data.LargeImage = new MediaItemModel
            {
                Url = url
            };
            return this;
        }

        public IResponseBuilder WithBackgroundImage(string url)
        {
            _response.Data.BackgroundImage = new MediaItemModel
            {
                Url = url
            };
            return this;
        }

        public IResponseBuilder WithVideo(string url)
        {
            _response.Data.Video = new MediaItemModel
            {
                Url = url
            };
            return this;
        }

        public IResponseBuilder WithDisplayTitle(string title)
        {
            _response.Data.DisplayTitleOverride = title;
            return this;
        }

        public IResponseBuilder WithDisplayText(string text)
        {
            _response.Data.DisplayTextOverride = text;
            return this;
        }

        public IResponseBuilder WithAudio(string url)
        {
            _response.Data.Audio = new MediaItemModel
            {
                Url = url,
                FileExtension = "mp3"
            };
            return this;
        }

        public IResponseBuilder WithReplacement(string key, string value)
        {
            CheckContent();
            _response.Data.Content = _response.Data.Content.Replace(key, value);
            return this;
        }

        public IResponseBuilder WithReplacements(Dictionary<string, string> dictionary)
        {
            CheckContent();
            foreach (var d in dictionary)
                _response.Data.Content = _response.Data.Content.Replace(d.Key, d.Value);
            return this;
        }

        public IResponseBuilder WithSessionAttributes(string key, object value)
        {
            CheckSessionAttributes();
            _response.Data.AdditionalSessionAttributes[key] = value;
            return this;
        }

        private void CheckSessionAttributes()
        {
            if (_response.Data.AdditionalSessionAttributes is null)
                _response.Data.AdditionalSessionAttributes = new Dictionary<string, object>();
        }

        public WebhookResponse<ContentFulfillmentWebhookData> BuildResponse()
        {
            return _response;
        }

        public IResponseBuilder WithAdditionalSessionAttributes()
        {
            _response.Data.AdditionalSessionAttributes = new Dictionary<string, object>();
            return this;
        }

        private void CheckContent()
        {
            if (_response.Data.Content is null)
                _response.Data.Content = "";
        }

        private ContentFulfillmentWebhookData EmptyResponse()
        {
            return new ContentFulfillmentWebhookData();
        }

        public IResponseBuilder FlushFollowUp()
        {
            _followUpBuilder.Flush();
            _response.Data.FollowUp = null;
            return this;
        }
    }
}
