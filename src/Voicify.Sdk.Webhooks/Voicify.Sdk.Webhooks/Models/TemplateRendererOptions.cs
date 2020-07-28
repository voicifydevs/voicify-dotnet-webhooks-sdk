using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Models
{
    public class TemplateRendererOptions
    {
        /// <summary>
        /// Setting this property to true will leave any tokens intact that have a corresponding prop in the JObject that has an empty value
        /// </summary>
        public bool DoNotFillEmptyValues { get; set; }
        /// <summary>
        /// Setting this property to true will leave any tokens intact that have a corresponding prop in the JObject that has a null value
        /// </summary>
        public bool DoNotFillNullValues { get; set; }
    }
}
