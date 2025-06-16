using MqttBrokerWebApi.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSignalR();

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .SetIsOriginAllowed(_ => true) // ← oder nur localhost:3000 erlauben
//            .AllowCredentials();
//    });
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseRouting();
//app.UseCors();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<NotificationHub>("/notificationHub");
//    endpoints.MapControllers();
//});

app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();
