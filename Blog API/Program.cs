using Blog_API.ConfigurationExtention;
using Blog_API.EexceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);
//Addin Services Extention Method
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();



app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
