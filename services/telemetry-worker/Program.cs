using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect("redis:6379"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
