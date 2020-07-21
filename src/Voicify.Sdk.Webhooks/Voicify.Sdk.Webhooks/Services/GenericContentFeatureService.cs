using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Webhooks.Requests;
using Voicify.Sdk.Core.Models.Webhooks.Responses;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public abstract class GenericContentFeatureService<TRequest, TContext> : IContentFeatureService<TRequest,TContext>
        where TRequest : GeneralWebhookFulfillmentRequest
    {
        protected IResponseBuilder _responseBuilder;
        protected ICollection<IContentScenario<TContext>> _contentScenarios;

        public GenericContentFeatureService(
            IResponseBuilder responseBuilder,
            params IContentScenario<TContext>[] contentScenarios)
        {
            _responseBuilder = responseBuilder;
            _contentScenarios = contentScenarios;
        }

        public abstract Task<Result<TContext>> GetContextFromInput(string authorization, TRequest request);

        public virtual async Task<Result<WebhookResponse<ContentFulfillmentWebhookData>>> HandleAsync(string authorization, TRequest request)
        {
            try
            {
                var contextResult = await GetContextFromInput(authorization, request);
                if (contextResult.ResultType != ResultType.Ok) return new InvalidResult<WebhookResponse<ContentFulfillmentWebhookData>>("Failed to produce context from input");
                
                var context = contextResult.Data;

                if (_contentScenarios == null || _contentScenarios.Count() == 0)
                {
                    var error = "No content scenarios were registered for this feature";
                    Console.WriteLine(error);
                    return new InvalidResult<WebhookResponse<ContentFulfillmentWebhookData>>(error);
                }

                //Find a suitable content renderer
                var canProvideContentTasks = _contentScenarios.Select(s => ReturnScenarioIfCanProvideContent(s, s.CanProvideContent(context)));
                var viableContentRenderers = (await Task.WhenAll(canProvideContentTasks))?.Where(s => s != null).ToArray();

                if (!viableContentRenderers.Any())
                {
                    var error = "No viable content scenarios found";
                    Console.WriteLine(error);
                    return new InvalidResult<WebhookResponse<ContentFulfillmentWebhookData>>(error);
                }

                var random = new Random();
                var scenario = viableContentRenderers[random.Next(viableContentRenderers.Length)];

                var contentResults = await scenario.GetContent(context);
                var followUpContent = await scenario.GetFollowUpContent(context);

                if (contentResults?.Any() == true)
                    _responseBuilder.WithContent(contentResults[random.Next(contentResults.Length)]);

               //Don't instantiate a follow up override if there's no follow up content or follow up items
                if (followUpContent?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithContent(followUpContent[random.Next(followUpContent.Length)]));

                if (scenario.EventFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithEventFollowUps(scenario.EventFollowUps));

                if (scenario.ExitFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithExitMessageFollowUps(scenario.ExitFollowUps));

                if (scenario.FallbackFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithFallbackFollowUps(scenario.FallbackFollowUps));

                if (scenario.HelpFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithHelpMessageFollowUps(scenario.HelpFollowUps));

                if (scenario.LatestMessageFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithLatestMessageFollowUps(scenario.LatestMessageFollowUps));

                if (scenario.NumberRangeFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithNumberRangeFollowUps(scenario.NumberRangeFollowUps));

                if (scenario.QuestionAnswerFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithQuestionFollowUps(scenario.QuestionAnswerFollowUps));

                if (scenario.RecipeFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithRecipeFollowUps(scenario.RecipeFollowUps));

                if (scenario.SimpleChoiceFollowUps?.Any() == true)
                    _responseBuilder.WithFollowUp(f => f.WithSimpleChoiceFollowUps(scenario.SimpleChoiceFollowUps));

                _responseBuilder
                 .WithSessionAttributes("context", JsonConvert.SerializeObject(context))
                 .WithSessionAttributes("scenario", scenario.Name);

                return new SuccessResult<WebhookResponse<ContentFulfillmentWebhookData>>(_responseBuilder.BuildResponse());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new InvalidResult<WebhookResponse<ContentFulfillmentWebhookData>>("An error occurred");
            }
            //Get the context from the feature service

        }

        private async Task<IContentScenario<TContext>> ReturnScenarioIfCanProvideContent(IContentScenario<TContext> s, Task<bool> task)
        {
            var canProvideContent = await task;
            if (canProvideContent)
                return s;
            else return null;
        }
    }
}
