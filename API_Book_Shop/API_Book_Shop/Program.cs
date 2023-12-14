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
         // ��������, ����� �� �������������� �������� ��� ��������� ������
         ValidateIssuer = true,
         // ������, �������������� ��������
         ValidIssuer = AuthOptions.ISSUER,

         // ����� �� �������������� ����������� ������
         ValidateAudience = true,
         // ��������� ����������� ������
         ValidAudience = AuthOptions.AUDIENCE,
         // ����� �� �������������� ����� �������������
         ValidateLifetime = true,

         // ��������� ����� ������������
         IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
         // ��������� ����� ������������
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
