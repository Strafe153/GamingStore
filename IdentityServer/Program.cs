using IdentityServer.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddServices(config);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options =>
    {
        options.WithOrigins("https://localhost:5001", "http://localhost:5136");
    });
});

var app = builder.Build();

app.UseIdentityServer();

app.Run();
