using Newtonsoft.Json.Linq;

namespace DotNETDevOps.Identity.AzureB2CUserService
{
    public class AzureB2CResult
    {
        public JObject Object { get; set; }
        public OdataError Error { get; set; }
        public bool IsError => Error != null;
    }

}
