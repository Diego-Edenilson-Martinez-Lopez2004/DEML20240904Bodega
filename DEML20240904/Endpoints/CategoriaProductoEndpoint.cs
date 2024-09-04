namespace DEML20240904.Endpoints
{
    public static class CategoriaProductoEndpoint
    {
        private static List<string> categorias = new();

        public static void AddCategoriaProductoEndpoints(this WebApplication app)
        {
            app.MapGet("/categoria/obtener-todos", () =>
            {
                return Results.Ok(categorias);
            });

            app.MapPost("/categoria/registrar", (string categoria) =>
            {
                categorias.Add(categoria);
                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}
