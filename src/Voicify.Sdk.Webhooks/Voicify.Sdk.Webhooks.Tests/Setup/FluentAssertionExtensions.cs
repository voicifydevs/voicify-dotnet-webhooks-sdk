using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Data.Definitions
{
    /// <summary>
    /// Extension methods for fluent assertion to add additional test logic
    /// </summary>
    public static class FluentAssertionExtensions
    {
        public static JsonToObjectPropertyAssertions ShouldCast(this string json)
        {
            return new JsonToObjectPropertyAssertions(json);
        }
    }
}