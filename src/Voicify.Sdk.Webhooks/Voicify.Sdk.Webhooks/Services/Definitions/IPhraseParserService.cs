using System;
using System.Collections.Generic;
using System.Text;
using Voicify.Sdk.Webhooks.Models;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IPhraseParserService
    {
        /// <summary>
        /// Return parsed slots from an utterance phrase
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        IEnumerable<PhraseSlot> ExtractSlotsFromPhrase(string phrase);

        /// <summary>
        /// Return phrase parts delimited by slots, breaking up the phrase into slot and non slot parts
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        IEnumerable<string> SplitPhraseIntoParts(string phrase);
    }
}
