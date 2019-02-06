using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DotNETDevOps.Identity.AzureB2CUserService
{
   
    /// <summary>
    /// https://msdn.microsoft.com/Library/Azure/Ad/Graph/api/entity-and-complex-type-reference#user-entity
    /// </summary>
    public class AzureB2CUser
    {
        public AzureB2CUser(string userName, string password, string displayName = null)
        {
            signInNames = new SignInNames[] { new SignInNames { type = "userName", value = userName } };
            this.displayName = displayName ?? userName;
            passwordProfile = new PasswordProfile { password = password };
        }
        public AzureB2CUser()
        {

        }

        public bool accountEnabled { get; set; } = true;
        public SignInNames[] signInNames { get; set; } = Array.Empty<SignInNames>();
        public string creationType { get; set; } = "LocalAccount";
        public string displayName { get; set; }
        public string mailNickname { get; set; }

        public PasswordProfile passwordProfile { get; set; }

        /// <summary>
        /// Specifies the password profile for the user. The profile contains the user's password. This property is required when a user is created.
        /// The password in the profile must satisfy minimum requirements as specified by the passwordPolicies property.By default, a strong password is required.For information about the constraints that must be satisfied for a strong password, see Password policy under Change your password in the Microsoft Office 365 help pages.The passwordProfile is a write only property.
        /// </summary>
        public string passwordPolicies { get; set; } = "DisablePasswordExpiration,DisableStrongPassword";
        public string city { get; set; }

        public string objectId { get; set; }

        public string objectType { get; set; }
        public string[] assignedLicenses { get; set; } = Array.Empty<string>();
        public string[] assignedPlans { get; set; } = Array.Empty<string>();
        public string companyName { get; set; }
        public string country { get; set; }
        public string department { get; set; }
        public string employeeId { get; set; }
        public string facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public string mail { get; set; }
        public string mailNickName { get; set; }
        public string[] otherMails { get; set; }

      
        public DateTimeOffset? deletionTimestamp { get; set; }

        // extra fields
        [JsonExtensionData]
        private IDictionary<string, JToken> _extraStuff;

        public string GetExtraDataString() => JToken.FromObject(_extraStuff).ToString();

        
        
    }

     
}
