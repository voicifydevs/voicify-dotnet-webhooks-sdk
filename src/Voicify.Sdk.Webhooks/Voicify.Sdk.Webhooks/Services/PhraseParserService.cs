using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Voicify.Sdk.Webhooks.Models;
using Voicify.Sdk.Webhooks.Services.Definitions;

namespace Voicify.Sdk.Webhooks.Services
{
    public class PhraseParserService : IPhraseParserService
    {
        private const string slotPattern = @"({[^}]*})";
        public IEnumerable<PhraseSlot> ExtractSlotsFromPhrase(string phrase)
        {
            var slotMatches = Regex.Matches(phrase, slotPattern);
            var extractedPhrases = new List<PhraseSlot>();
            foreach (Match match in slotMatches)
            {
                extractedPhrases.Add(new PhraseSlot(match.Value));
            }
            return extractedPhrases;
        }

        public IEnumerable<string> SplitPhraseIntoParts(string phrase)
        {
            return Regex.Split(phrase, slotPattern);
        }
    }
}
