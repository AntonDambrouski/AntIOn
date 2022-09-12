using Chat.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500").AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();