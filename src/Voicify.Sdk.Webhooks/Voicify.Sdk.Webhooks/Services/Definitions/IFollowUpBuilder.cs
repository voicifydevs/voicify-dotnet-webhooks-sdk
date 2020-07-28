using System;
using System.Collections.Generic;
using System.Text;
using Voicify.Sdk.Core.Models.Model;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IFollowUpBuilder
    {
        FollowUpModel GetFollowUp();
        void Flush();
        IFollowUpBuilder LimitToChildren(bool limitToChildren = true);
        IFollowUpBuilder WithReplacement(string key, string value);
        IFollowUpBuilder WithReplacements(Dictionary<string, string> dictionary);
        IFollowUpBuilder WithContent(string content);
        IFollowUpBuilder WithEventFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithExitMessageFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithFallbackFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithHelpMessageFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithLatestMessageFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithNumberRangeFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithQuestionFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithRecipeFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithSimpleChoiceFollowUps(params string[] followUpIds);
        IFollowUpBuilder WithContentItemFollowUp(string contentItemId, string featureTypeId);
    }
}
