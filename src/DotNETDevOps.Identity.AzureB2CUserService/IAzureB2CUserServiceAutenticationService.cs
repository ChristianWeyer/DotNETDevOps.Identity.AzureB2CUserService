using System.Net.Http;
using System.Threading.Tasks;

namespace DotNETDevOps.Identity.AzureB2CUserService
{
    public interface IAzureB2CUserServiceAutenticationService
    {
        Task<string> GetClientSecretAsync();

        Task AuthenticateAsync(HttpRequestMessage request);
    }

}
