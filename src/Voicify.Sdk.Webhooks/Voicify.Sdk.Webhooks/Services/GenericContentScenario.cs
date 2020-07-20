using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services { 
    public abstract class GenericContentScenario<TContext> : IContentScenario<TContext>
    {
        public virtual string[] QuestionAnswerFollowUps { get; set; } = new string[0];
        public virtual string[] SimpleChoiceFollowUps { get; set; } = new string[0];
        public virtual string[] EventFollowUps { get; set; } = new string[0];
        public virtual string[] RecipeFollowUps { get; set; } = new string[0];
        public virtual string[] ExitFollowUps { get; set; } = new string[0];
        public virtual string[] FallbackFollowUps { get; set; } = new string[0];
        public virtual string[] HelpFollowUps { get; set; } = new string[0];
        public virtual string[] LatestMessageFollowUps { get; set; } = new string[0];
        public virtual string[] NumberRangeFollowUps { get; set; } = new string[0];
        public abstract string Name { get; }
        public abstract Task<bool> CanProvideContent(TContext context);
        public abstract Task<string[]> GetContent(TContext context);
        public abstract Task<string[]> GetFollowUpContent(TContext context);
    }
}
