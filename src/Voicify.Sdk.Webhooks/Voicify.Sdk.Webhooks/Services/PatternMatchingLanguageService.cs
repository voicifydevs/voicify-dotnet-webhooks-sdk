using ServiceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Model;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public class PatternMatchingLanguageService : IPatternMatchingLanguageService
    {
        private readonly IPhraseParserService _phraseParserService;

        public PatternMatchingLanguageService(IPhraseParserService phraseParserService)
        {
            _phraseParserService = phraseParserService;
        }

        public Task<Result<List<ProcessedLanguage>>> ProcessAll(string input, InteractionModel languageModel)
        {
            try
            {
                var intentMatches = new List<ProcessedLanguage>();
                foreach (var intent in languageModel.Intents)
                {
                    foreach (var utterance in intent.Utterances)
                    {
                        var slots = MatchSlots(input, utterance);
                        if (slots != null)
                        {
                            intentMatches.Add(new ProcessedLanguage
                            {
                                Intent = intent.Name["voicify"],
                                Slots = slots,
                            });
                        }
                    }
                }

                return Task.FromResult<Result<List<ProcessedLanguage>>>(new SuccessResult<List<ProcessedLanguage>>(intentMatches));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Task.FromResult<Result<List<ProcessedLanguage>>>(new UnexpectedResult<List<ProcessedLanguage>>());
            }
        }
        private Dictionary<string, string> MatchSlots(string input, string utterance)
        {
            // ASSUMPTIONS: No back to back slots - does not require exact match - does not support slot types
            var utteranceParts = _phraseParserService.SplitPhraseIntoParts(utterance).ToList();
            utteranceParts = utteranceParts.Where(u => !string.IsNullOrEmpty(u)).ToList();
            var remainingString = input.ToLower();
            var slots = new Dictionary<string, string>();
            var previousWasSlot = false;
            var hasCarrier = false;
            for (var i = 0; i < utteranceParts.Count; i++)
            {
                var part = utteranceParts[i]?.ToLower();
                var isSlot = part.Contains("{") && part.Contains("}");
                if (isSlot)
                {
                    if (i == utteranceParts.Count - 1)
                    {
                        // ends on slot
                        slots.Add(part.Replace("{", string.Empty).Replace("}", string.Empty), remainingString);
                    }
                    previousWasSlot = true;
                    continue;
                }
                else if (!string.IsNullOrEmpty(part) && remainingString.Contains(part))
                {
                    hasCarrier = true;
                    var index = remainingString.IndexOf(part);
                    if (previousWasSlot)
                    {
                        slots.Add(utteranceParts[i - 1].Replace("{", string.Empty).Replace("}", string.Empty), remainingString.Substring(0, index));
                    }
                    remainingString = remainingString.Remove(index, part.Length);
                }
                else if (!hasCarrier)
                {
                    return null;
                }
            }

            return slots;
        }
    }
}
