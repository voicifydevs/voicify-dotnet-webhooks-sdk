using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Models
{
    /// <summary>
    /// Helper class for parsing slots that may contain examples
    /// </summary>
    public class PhraseSlot
    {
        public PhraseSlot(string fullText)
        {
            FullText = fullText;
        }
        public string FullText { get; set; }

        public string Value
        {
            get
            {
                return (FullText.Substring(1, FullText.Length - 2)).Split('|')[0].Trim();
            }
        }
        /// <summary>
        /// Calculated example, without delimeter brackets, whitespace trimmed, potentially null
        /// </summary>
        public string Example
        {
            get
            {
                return FullText.Contains("|") ? FullText.Substring(1, FullText.Length - 2).Split('|')[1].Trim() : null;
            }
        }
    }

}
