namespace DEML20240904.Endpoints
{
    public static class BodegaEndpoint
    {
        private static List<Bodega> bodegas = new();

        public static void AddBodegaEndpoints(this WebApplication app)
        {
            app.MapPost("/bodega/crear", (Bodega bodega) =>
            {
                bodegas.Add(bodega);
                return Results.Ok();
            }).RequireAuthorization();

            app.MapPut("/bodega/modificar/{id}", (int id, Bodega bodega) =>
            {
                var bodegaExistente = bodegas.FirstOrDefault(b => b.Id == id);
                if (bodegaExistente == null)
                    return Results.NotFound();

                bodegaExistente.Nombre = bodega.Nombre;
                return Results.Ok();
            }).RequireAuthorization();

            app.MapGet("/bodega/obtener-por-id/{id}", (int id) =>
            {
                var bodega = bodegas.FirstOrDefault(b => b.Id == id);
                if (bodega == null)
                    return Results.NotFound();

                return Results.Ok(bodega);
            }).RequireAuthorization();
        }
    }

    public class Bodega
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
