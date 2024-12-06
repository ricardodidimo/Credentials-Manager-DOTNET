using FluentResults;
using webapi.Endpoints.Category;
using webapi.Endpoints.Credentials;
using webapi.Endpoints.User;
using webapi.Endpoints.Vault;

namespace webapi.Helpers
{
    public static class ApiRoutesRegisterExtension
    {
        public static void RegisterApiRoutes(this WebApplication app)
        {
            app.MapPost("/register", RegisterUserEndpoint.Execute)
            .WithName("Register")
            .WithOpenApi();

            app.MapPost("/login", LoginUserEndpoint.Execute)
            .WithName("Login")
            .WithOpenApi();

            app.MapGet("/vaults", ListVaultsEndpoint.Execute)
            .WithName("ListVaults")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPost("/vaults", CreateVaultEndpoint.Execute)
            .WithName("CreateVaults")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPatch("/vaults", PatchVaultEndpoint.Execute)
            .WithName("UpdateVaults")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapDelete("/vaults", DeleteVaultEndpoint.Execute)
            .WithName("DeleteVault")
            .RequireAuthorization()
            .WithOpenApi();


            app.MapGet("/credentials", ListCredentialsEndpoint.Execute)
            .WithName("ListCredentials")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPost("/credentials", CreateCredentialsEndpoint.Execute)
            .WithName("CreateCredentials")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPatch("/credentials", PatchCredentialsEndpoint.Execute)
            .WithName("UpdateCredentials")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapDelete("/credentials", DeleteCredentialsEndpoint.Execute)
            .WithName("DeleteCredentials")
            .RequireAuthorization()
            .WithOpenApi();


            app.MapGet("/categories", ListCategoriesEndpoint.Execute)
            .WithName("ListCategories")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPost("/categories", CreateCategoryEndpoint.Execute)
            .WithName("CreateCategory")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapPatch("/categories", PatchCategoryEndpoint.Execute)
            .WithName("UpdateCategory")
            .RequireAuthorization()
            .WithOpenApi();

            app.MapDelete("/categories", DeleteCategoryEndpoint.Execute)
            .WithName("DeleteCategory")
            .RequireAuthorization()
            .WithOpenApi();
        }
    }
}
