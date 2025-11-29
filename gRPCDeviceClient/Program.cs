using gRPCDeviceClient;

var builder = Host.CreateApplicationBuilder(args);
Console.WriteLine("Enter Device Id:");
int deviceId = int.Parse(Console.ReadLine());
// configure worker with device id
builder.Services.AddHostedService<Worker>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Worker>>();
    return new Worker(logger, deviceId);
});

var host = builder.Build();
host.Run();
