# DotNETDevOps.Identity.AzureB2CUserService

[![Build status](https://dev.azure.com/dotnet-devops/dotnetdevops/_apis/build/status/DotNETDevOps.Identity.AzureB2CUserService)](https://dev.azure.com/dotnet-devops/dotnetdevops/_build/latest?definitionId=0)

The following library adds support to use Azure B2C as a userservice to IdentityServer.

This means that the backend store for credentials is protected by using Azure AD and all its security. But you are still in controll of your indentity server and can add UI/Extensions as you see fit.

## Implementation Details

The oid claim from Azure AD users is used as the subject id (sub claim) of users according to OpenID specifications required by identityserver. the oid claim is the unique object id per user object in azure ad.

