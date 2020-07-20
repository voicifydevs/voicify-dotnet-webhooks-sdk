using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Voicify.Sdk.Webhooks.Models;

namespace Voicify.Sdk.Webhooks.Services
{
    public static class TemplateRenderer
    {
        private const string _variablePattern = @"({[^}]*})";

        public static string Render(string template, JObject jObj, TemplateRendererOptions options = null)
        {
            try
            {
                if (options is null)
                    options = new TemplateRendererOptions();

                //extract variables from template
                var templateMatches = Regex.Matches(template, _variablePattern);
                var templateVariables = templateMatches.Cast<Match>().Select(m => m.Value).ToArray();

                //Create mapping of input variable to 
                var fieldValuesForTemplateVariables = new Dictionary<string, string>();

                foreach (var v in templateVariables)
                {
                    var vName = v.Substring(1, v.Length - 2);
                    var fv = jObj.SelectToken(vName)?.Value<string>();
                    if (fv is object)
                        fieldValuesForTemplateVariables.Add(v, fv);
                }

                foreach (var fv in fieldValuesForTemplateVariables)
                {
                    if (fv.Value is null && options.DoNotFillNullValues)
                        continue;
                    if (fv.Value == string.Empty && options.DoNotFillEmptyValues)
                        continue;

                    template = template.Replace(fv.Key, fv.Value);
                }

                return template;
            }
            catch (Exception ex)
            {
                return $"Failed with reason: {ex.Message}";
            }
        }
    }
}
