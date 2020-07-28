using ServiceResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Integrations.Setup;

namespace Voicify.Sdk.Webhooks.Services.Definitions
{
    public interface IIntegrationSetupService<TConfig> 
        where TConfig : class
    {
        Task<Result<IntegrationSetupResponse>> Setup(TConfig request);
        Task<Result<bool>> Config(TConfig request);
    }
}
