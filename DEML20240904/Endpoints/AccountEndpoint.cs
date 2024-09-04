using DEML20240904.Auth;

namespace DEML20240904.Endpoints
{
    public static class AccountEndpoint
    {
        public static void AddAccountEndpoints(this WebApplication app)
        {
            app.MapPost("/account/login", (string username, string password, IJwtAuthenticationService authService) =>
            {
                var token = authService.Authenticate(username, password);
                if (token == null)
                    return Results.Unauthorized();

                return Results.Ok(token);
            });

            app.MapPost("/account/logout", () =>
            {
                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}
