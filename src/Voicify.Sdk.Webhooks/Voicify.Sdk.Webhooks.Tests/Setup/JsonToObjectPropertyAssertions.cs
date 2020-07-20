using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Data.Definitions
{
    /// <summary>
    /// Assertion that allows for casting a string assertion
    /// to an object via json.net deserialization then reference
    /// a property check
    /// </summary>
    public class JsonToObjectPropertyAssertions : StringAssertions
    {
        public JsonToObjectPropertyAssertions(string value) : base(value)
        {
            Subject = value;
        }

        protected override string Identifier => "json-string";

        public AndConstraint<T> ToObjectAndHave<T>(Func<T, bool> condition, string because = "", params object[] becauseArgs) where T : class
        {
            try
            {
                var jsonObject = JsonConvert.DeserializeObject<T>(Subject);

                Execute.Assertion.BecauseOf(because, becauseArgs)
                    .ForCondition(condition(jsonObject))
                    .FailWith("Expected condition to be true, but evaluation returned false");

                return new AndConstraint<T>(jsonObject);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Execute.Assertion.FailWith($"Error deserializing to json.");
                return null;
            }
        }
        public AndConstraint<dynamic> ToDynamicAndHave(Func<dynamic, bool> condition, string because = "", params object[] becauseArgs)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(Subject);

            Execute.Assertion.BecauseOf(because, becauseArgs)
                .ForCondition(condition(jsonObject))
                .FailWith("Expected condition to be true, but evaluation returned false");

            return new AndConstraint<dynamic>(jsonObject);
        }
    }
}
