using Dog_Books_BackEnd.Startup;
using Dog_Books_BackEnd;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//IMPORTANT
//Would normally whitelist any IP's or origins here.
//Can also manage CORS and whitelisting IP's/origin URL's within the Azure Portal CORS tab
//For the purpose of running this as a local project I will "AllowAny" for all
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowAnyOrigin();
                          });
});

//I call my method from "ApplicationSettingsBuilder", and now my environment specific settings are available
//to call within my controllers
builder.Host.ConfigureAppSettings();

var settings = builder.Configuration.GetRequiredSection("Settings").Get<Settings>();

builder.Services.AddOptions();
builder.Services.Configure<Settings>(
    builder.Configuration.GetSection("Settings"));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
