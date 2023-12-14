using API_Book_Shop;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         // укзывает, будет ли валидироваться издатель при валидации токена
         ValidateIssuer = true,
         // строка, представляющая издателя
         ValidIssuer = AuthOptions.ISSUER,

         // будет ли валидироваться потребитель токена
         ValidateAudience = true,
         // установка потребителя токена
         ValidAudience = AuthOptions.AUDIENCE,
         // будет ли валидироваться время существования
         ValidateLifetime = true,

         // установка ключа безопасности
         IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
         // валидация ключа безопасности
         ValidateIssuerSigningKey = true,
     };
 });

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<API_Book_Shop.Models.Book_ShopContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("con")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




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
