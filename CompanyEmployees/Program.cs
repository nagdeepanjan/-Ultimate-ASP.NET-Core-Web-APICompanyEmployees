using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;

var builder = WebApplication.CreateBuilder(args);

//Logging
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

#region Services
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();

builder.Services.AddControllers();
#endregion

var app = builder.Build();

#region Middleware
if (app.Environment.IsDevelopment()) 
    app.UseDeveloperExceptionPage(); 
else 
    app.UseHsts();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All 
}); 
app.UseCors("CorsPolicy");


app.UseAuthorization();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    Console.WriteLine("Start of app.Use\n");
    await next(context);
    Console.WriteLine("End of app.Use\n");
});

//app.Run(async (HttpContext context) =>
//{
//    Console.WriteLine("Inside app.Run\n");
//    //await context.Response.WriteAsync("Inside app.Run\n");
//});

app.MapControllers();
#endregion

app.Run();
