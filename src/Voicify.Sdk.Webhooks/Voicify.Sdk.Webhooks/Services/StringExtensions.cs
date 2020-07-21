using System;
using System.Collections.Generic;
using System.Text;

namespace Voicify.Sdk.Webhooks.Services
{
    public static class StringExtensions
    {
        public static string[] Split(this string input, string split)
        {
            return input.Split(new string[] { split }, StringSplitOptions.None);
        }
    }
}
