using System.Collections.Generic;
using Voicify.Sdk.Core.Models.Constants;
using Voicify.Sdk.Core.Models.Model;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public class FollowUpBuilder : IFollowUpBuilder
    {
        private FollowUpModel _followUp;
        public FollowUpBuilder()
        {
            _followUp = new FollowUpModel();
        }        

        public IFollowUpBuilder WithContent(string content)
        {
            CheckContent();
            _followUp.Content += content;
            return this;
        }
        public IFollowUpBuilder WithReplacement(string key, string value)
        {
            CheckContent();
            _followUp.Content = _followUp.Content.Replace(key, value);
            return this;
        }

        public IFollowUpBuilder WithReplacements(Dictionary<string, string> dictionary)
        {
            CheckContent();
            foreach (var d in dictionary)
                _followUp.Content = _followUp.Content.Replace(d.Key, d.Value);
            return this;
        }

        public FollowUpModel GetFollowUp()
        {
            return _followUp;
        }
        public IFollowUpBuilder WithContentItemFollowUp(string contentItemId, string featureTypeId)
        {
            CheckChildContentContainer();
            _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = contentItemId, FeatureTypeId = featureTypeId });

            return this;
        }
        public IFollowUpBuilder WithQuestionFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.QuestionAnswer });

            return this;
        }
        public IFollowUpBuilder WithEventFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.Events });

            return this;
        }
        public IFollowUpBuilder WithExitMessageFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.ExitMessages });

            return this;
        }
        public IFollowUpBuilder WithFallbackFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.Fallback });

            return this;
        }
        public IFollowUpBuilder WithHelpMessageFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.HelpMessages });

            return this;
        }
        public IFollowUpBuilder WithLatestMessageFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.LatestMessages });

            return this;
        }
        public IFollowUpBuilder WithNumberRangeFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.NumberRange });

            return this;
        }
        public IFollowUpBuilder WithRecipeFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.Recipes });

            return this;
        }

        public IFollowUpBuilder WithSimpleChoiceFollowUps(params string[] followUpIds)
        {
            CheckChildContentContainer();
            foreach (var id in followUpIds)
                _followUp.ChildContentContainer.ContentItems.Add(new GenericContentModel { Id = id, FeatureTypeId = FeatureTypeIds.SimpleChoice });

            return this;
        }
        public IFollowUpBuilder WithHint(FollowUpHintModel hint)
        {
            CheckHints();
            _followUp.FollowUpHints.Add(hint);
            return this;
        }

        public IFollowUpBuilder LimitToChildren(bool limitToChildren = true)
        {
            CheckChildContentContainer();
            _followUp.ChildContentContainer.IsLimitedToChildren = limitToChildren;
            return this;
        }
        private void CheckContent()
        {
            if (_followUp.Content is null)
                _followUp.Content = "";
        }

        private void CheckChildContentContainer()
        {
            if (_followUp.ChildContentContainer is null)
                _followUp.ChildContentContainer = new ChildContentContainerModel
                {
                    IsLimitedToChildren = false,
                    ContentItems = new List<GenericContentModel>(),
                };
        }

        private void CheckHints()
        {
            if (_followUp.FollowUpHints is null)
                _followUp.FollowUpHints = new List<FollowUpHintModel>();
        }

        public void Flush()
        {
            _followUp = new FollowUpModel();
        }
    }
}
