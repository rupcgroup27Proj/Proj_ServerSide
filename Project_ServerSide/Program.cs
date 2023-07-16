using Microsoft.Extensions.FileProviders;
using Project_ServerSide.Models.Algorithm;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection(); //Problems wih ruppin's server - uncomment before publishing

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();


app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new CompositeFileProvider(
        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles")),
        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "pdfs")),
        new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "submissions"))
        ),
    RequestPath = new PathString("/Images")
});


app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

//Algorithm.Main(); //for updating the algorithm once a day

app.Run();


