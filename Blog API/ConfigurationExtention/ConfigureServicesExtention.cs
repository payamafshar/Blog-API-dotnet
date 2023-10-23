using Blog_API.ApplicationDbContext;
using Blog_API.Identity;
using Blog_API.JwtServices;
using Blog_API.Mapping;
using Blog_API.Modules.Blog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;



namespace Blog_API.ConfigurationExtention
{
    public static class ConfigureServicesExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                //Authorization Policy [Authoriza] Globaly
                //ar policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //tions.Filters.Add(new AuthorizeFilter(policy));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //Services
            services.AddTransient<IJwtService, JwtService>();
            services.AddScoped<IBlogService, BlogService>();

            //DbContext
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Default"));
            });
            //Addin Http Client 
            services.AddHttpClient();

            //Addin Auto Mapper to mappin Response
            services.AddAutoMapper(typeof(AutoMapperProfile));
            //cors localhost:3000
            /*
            services.AddCors(options =>
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
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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
            services.AddAuthentication(options =>
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
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
            services.AddAuthorization(options => { });
            return services;
        }
    }
}
