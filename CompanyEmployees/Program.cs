using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

#region Services
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();

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

app.MapControllers();
#endregion

app.Run();
