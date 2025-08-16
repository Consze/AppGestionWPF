var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseCors("AllowAnyOrigin");
app.UseStaticFiles();

app.MapGet("/", () => "Hola mundo!");

app.MapPost("/api/subir-imagen", async (IFormFile imagen) => {
    var nombreArchivo = imagen.FileName;
    var tamanoArchivo = imagen.Length;

    return Results.Ok(new { Mensaje = "Archivo recibido con éxito", Nombre = nombreArchivo, Tamano = tamanoArchivo });
});

app.Run();