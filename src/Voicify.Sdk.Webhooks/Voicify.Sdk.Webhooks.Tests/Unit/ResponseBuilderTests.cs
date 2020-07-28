using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Constants;
using Voicify.Sdk.Core.Models.Integrations.Setup;
using Voicify.Sdk.Webhooks.Services;
using Voicify.Sdk.Webhooks.Services.Definitions;
using Xunit;

namespace Voicify.Sdk.Webhooks.Data.Unit
{
    public class ResponseBuilderTests
    {
        private IResponseBuilder _responseBuilder;
        [Fact]
        public void SuccessfulResponseBuild()
        {
            _responseBuilder = new ResponseBuilder(new FollowUpBuilder());
            var response = _responseBuilder
                .WithContent("testContent{1},{2},{3}")
                .WithReplacement("{1}", "one")
                .WithReplacements(new Dictionary<string, string>() { { "{2}", "two" }, { "{3}", "three" } })
                .WithSessionAttributes("key", new IntegrationSetupRequest())
                .WithFollowUp(f => f.WithContent("followUpContent{1}")
                    .WithReplacement("{1}", "one")
                    .WithNumberRangeFollowUps("id1")
                    .WithSimpleChoiceFollowUps("id2"))
                .BuildResponse();
            response.Data.Content.Should().Be("testContentone,two,three");
            response.Data.FollowUp.Content.Should().Be("followUpContentone");
            response.Data.FollowUp.ChildContentContainer.ContentItems.Where(c => c.FeatureTypeId.Equals(FeatureTypeIds.NumberRange)).Count().Should().Be(1);
            response.Data.FollowUp.ChildContentContainer.ContentItems.Where(c => c.FeatureTypeId.Equals(FeatureTypeIds.NumberRange)).FirstOrDefault().Id.Should().Be("id1");
            response.Data.FollowUp.ChildContentContainer.ContentItems.Where(c => c.FeatureTypeId.Equals(FeatureTypeIds.SimpleChoice)).Count().Should().Be(1);
            response.Data.FollowUp.ChildContentContainer.ContentItems.Where(c => c.FeatureTypeId.Equals(FeatureTypeIds.SimpleChoice)).FirstOrDefault().Id.Should().Be("id2");

        }
    }
}
