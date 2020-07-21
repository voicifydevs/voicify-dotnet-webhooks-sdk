using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Services
{
    public static class ContentExtensions
    {
        public static string StripNonEssential(this string content)
        {
            IEnumerable<string> nonEssentialPhrases = new List<string>() {
                "search for",
                "jobs",
                "positions",
                "are there any tweets about",
                "are there any tweets from",
                "did anyone tweet about",
                "is there any info about",
                "where is your",
                "where are your",
                "where are you",
                "what's the latest from",
                "what's the latest with",
                "what's the latest on",
                "what's new with",
                "what's new on",
                "what's new from",
                "what's the daily message from",
                "what are you",
                "who is",
                "tell me about",
                "are there any",
                "positions available",
                "jobs available",
                "jobs",
                "job",
                "about",
                "what are your",
                "do you have any",
                "open positions",
                "what do you have",
                "what do you have for open positions",
                "what open positions do you have in",
                "do you have any open positions",
                "do you have any positions"
            };
            string FormattedQuestion = content;

            foreach (string phrase in nonEssentialPhrases)
            {
                FormattedQuestion = FormattedQuestion.ToLower().Replace(phrase.ToLower(), "");
            }

            return FormattedQuestion;
        }
    }
}
