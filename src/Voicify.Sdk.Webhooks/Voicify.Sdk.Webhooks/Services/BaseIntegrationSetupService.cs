using ServiceResult;
using System.Threading.Tasks;
using Voicify.Sdk.Core.Models.Integrations.Setup;

namespace Voicify.Sdk.Webhooks.Services
{
    public abstract class BaseIntegrationSetupService
    {
        public virtual async Task<Result<IntegrationSetupResponse>> Setup(IntegrationSetupRequest apiUser)
        {
            return new SuccessResult<IntegrationSetupResponse>(new IntegrationSetupResponse { AdditionalPropertiesConfiguration = null });
        }
    }
}
