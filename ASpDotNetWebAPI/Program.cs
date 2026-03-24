using ASpDotNetWebAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Required for Identity (.NET 8)
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

// ✅ Identity DB (Auth)
builder.Services.AddDbContext<AuthDBContext>(options =>
{
    options.UseInMemoryDatabase("AuthDB");
});

// ✅ Your main DB
builder.Services.AddDbContext<ApplicationDbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ✅ Identity Setup
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<AuthDBContext>()
    .AddApiEndpoints()
    .AddSignInManager(); // 🔥 IMPORTANT

// ✅ FIX: Correct authentication scheme
builder.Services.AddAuthentication(IdentityConstants.BearerScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    { 
        Title ="Auth Demo",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JMT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type= ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []


        }

    });

});

var app = builder.Build();

// ✅ Middleware order
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Identity endpoints (register/login)
app.MapIdentityApi<IdentityUser>();

app.MapControllers();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();