using Microsoft.AspNetCore.Components.Authorization;

namespace PizzaWebAppUI.Helper;

public static class AuthenticationStateProviderHelper
{
    public static async Task<CustomerModel> GetCustomerFromAuth(this AuthenticationStateProvider provider, ICustomerData customerData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
        return await customerData.GetCustomerFromAuthentication(objectId);
    }
}
