using ChatApp.Repository.Interface;
using ChatApp.Repository;
using ChatApp.Service.Interface;
using ChatApp.Service;
using ChatApp.Service.Implementations;
using ChatApp.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers(); // Register controllers


//builder.Services.AddScoped<IChatRepository, ChatRepository>();
//builder.Services.AddScoped<IAgentRepository, AgentRepository>();
//builder.Services.AddScoped<IChatService, ChatService>();
//builder.Services.AddScoped<IAgentService, AgentService>();

//Repositories
builder.Services.AddSingleton<IAgentRepository, AgentRepository>();
builder.Services.AddSingleton<IChatRepository, ChatRepository>();
builder.Services.AddSingleton<IShiftRepository, ShiftRepository>();
builder.Services.AddSingleton<ITeamRepository, TeamRepository>();

//Services
builder.Services.AddSingleton<IAgentService, AgentService>();
builder.Services.AddSingleton<IShiftService, ShiftService>();
builder.Services.AddSingleton<IChatService, ChatService>();

// Register ChatAssignmentService as a Hosted Service
builder.Services.AddHostedService<ChatAssignmentService>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
