using Buttons.Web;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining<Program>();
});
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b
            .WithOrigins("http://127.0.0.1:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<IUserIdProvider, PlayerIdProvider>();
builder.Services.AddSingleton<GameSession>();

var app = builder.Build();

app.UseRouting();
app.UseCors();
app.MapHub<GameHub>("/game");

app.Run();