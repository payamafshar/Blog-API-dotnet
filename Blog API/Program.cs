using Blog_API.ApplicationDbContext;
using Blog_API.Identity;
using Blog_API.Mapping;
using Blog_API.Modules.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddDbContext<BlogDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//cors localhost:3000
/*
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowOrigins").Get<string[]>());
        policyBuilder.WithHeaders("content-type", "origin", "accept");
    });
    options.AddPolicy("Client1", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowOrigins2").Get<string[]>());
        policyBuilder.WithHeaders("content-type", "origin", "accept");
        policyBuilder.WithMethods("POST");
    });


});
*/
//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    //can config the password
    options.Password.RequiredLength = 5;
})
    .AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, BlogDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, BlogDbContext, Guid>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
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
