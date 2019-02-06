using System;

namespace DotNETDevOps.Identity.AzureB2CUserService
{
    public class PatchAzureB2CUser
    {
        public string displayName { get; set; }
        public PasswordProfile passwordProfile { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string facsimileTelephoneNumber { get; set; }
        public SignInNames[] signInNames { get; set; }
        public string[] otherMails { get; set; }
        

    }

}
