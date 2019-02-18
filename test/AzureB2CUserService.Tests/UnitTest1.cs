using DotNETDevOps.Identity.AzureB2CUserService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AzureB2CUserService.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var sc = new ServiceCollection();
            sc.AddLogging();
            sc.AddHttpClient();
            sc.AddSingleton(Options.Create<AzureB2CUserServiceConfiguration>(new AzureB2CUserServiceConfiguration
            {
               
            }));
            sc.AddSingleton<AzureB2CUserService<AzureB2CUser>>();
            sc.AddSingleton<IAzureB2CUserServiceAutenticationService, AzureB2CUserServiceAutenticationService>();
            
            var sp = sc.BuildServiceProvider();
            
            var userservice = sp.GetRequiredService<AzureB2CUserService<AzureB2CUser>>();

            // await userservice.AddToGroupAsync("ae385c8c-e0ad-4f76-ad03-63aa6fd1c131", "abf2050e-6708-4c2c-b1f0-b439845cba66");
            var a = await userservice.SetPasswordAsync("abf2050e-6708-4c2c-b1f0-b439845cba66", "qwerty12");
        }
    }
}
