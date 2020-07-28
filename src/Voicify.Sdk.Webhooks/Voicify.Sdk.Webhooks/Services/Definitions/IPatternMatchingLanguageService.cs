using ServiceResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Model;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IPatternMatchingLanguageService
    {
        Task<Result<List<ProcessedLanguage>>> ProcessAll(string input, InteractionModel languageModel);
    }
}
