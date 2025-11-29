var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
// Configure logging
builder.Logging.ClearProviders();       // optional, clears default providers
builder.Logging.AddConsole();           // must add console logging
builder.Logging.SetMinimumLevel(LogLevel.Information);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();

}

app.MapGrpcService<gRPC.Demo.Services.TelementaryTrackingService>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
