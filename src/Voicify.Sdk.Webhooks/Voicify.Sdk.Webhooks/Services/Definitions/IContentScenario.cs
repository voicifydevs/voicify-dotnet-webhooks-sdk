using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IContentScenario<TContext>
    {
        string Name { get; }
        string[] QuestionAnswerFollowUps { get; set; } 
        string[] SimpleChoiceFollowUps { get; set; }
        string[] EventFollowUps { get; set; }
        string[] RecipeFollowUps { get; set; }
        string[] ExitFollowUps { get; set; }
        string[] FallbackFollowUps { get; set; }
        string[] HelpFollowUps { get; set; }
        string[] LatestMessageFollowUps { get; set; }
        string[] NumberRangeFollowUps { get; set; }
        Task<bool> CanProvideContent(TContext context);
        Task<string[]> GetContent(TContext context);
        Task<string[]> GetFollowUpContent(TContext context);
    }
}
