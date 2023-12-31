using Abp.AspNetCore.SignalR.Hubs;
using Blog_API.ChatHubs;
using Blog_API.ConfigurationExtention;
using Blog_API.EexceptionMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
//Addin Services Extention Method
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();



app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.EnablePersistAuthorization());
    IdentityModelEventSource.ShowPII = true;
}


   
    app.MapHub<MessaginHub>("/chat");

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
