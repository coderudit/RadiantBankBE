using RadiantBank.API;
using RadiantBank.Application;
using RadiantBank.Infrastructure;
using RadiantBank.Infrastructure.RadiantBankDB;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllHeaders",
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader(); 
            policy.AllowAnyMethod();
        });
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddAPIServices();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // Initialise and seed database
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<RadiantBankDbContextInitializer>();
    await initializer.SeedAsync();
}
app.UseCors("AllowAllHeaders");
app.MapControllers();

app.Run();