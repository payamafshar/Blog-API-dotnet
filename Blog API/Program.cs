using Blog_API.ApplicationDbContext;
using Blog_API.Identity;
using Blog_API.JwtService;
using Blog_API.Mapping;
using Blog_API.Modules.Blog;
using Blog_API.Modules.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    //Authorization Policy [Authoriza] Globaly
   //ar policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    //tions.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Services
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddScoped<IBlogService, BlogService>();

//DbContext
builder.Services.AddDbContext<BlogDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
//Addin Http Client 
builder.Services.AddHttpClient();

//Addin Auto Mapper to mappin Response
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
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<BlogDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, BlogDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, BlogDbContext, Guid>>();
//JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //IF user not authenticated with jwt redirect to cookie athenticationScheme
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    //Validation of token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options => { });
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
