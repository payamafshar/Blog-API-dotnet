using Blog_API.ApplicationDbContext;
using Blog_API.JwtServices;
using Blog_API.Mapping;
using Blog_API.Modules.Blog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Blog_API.Modules.Likes_Comments;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;


namespace Blog_API.ConfigurationExtention
{
    public static class ConfigureServicesExtention
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers(options =>
            {
                //Accesing to logger service for action Filter requirement
                //var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ModelStateValidatorForSpeceficController>>();
                //Adding Filter Globaly Way 1
                //options.Filters.Add(new ModelStateValidatorForSpeceficController(logger, "exampleKey", "exampleValue",2));
                //Adding Filter Globaly Way 2
                //options.Filters<ModelStateValidatorForSpeceficController>(5);

                //Authorization Policy [Authoriza] Globaly
                //ar policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //tions.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

      


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogApi", Version = "V1" });
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme,
                            },
                            Scheme = "OAuth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
          
            });
            //Services
            services.AddTransient<IJwtService, JwtService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ILikeAndCommentService, LikesAndCommentService>();
            services.AddHttpContextAccessor();


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
            //JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //IF user not authenticated with jwt redirect to cookie athenticationScheme
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;



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
                //options.Authority = configuration["AuthortyUrl"];
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            
               
            
            });
            services.AddAuthorization();
            services.AddSignalR();
            return services;
        }
    }
}
