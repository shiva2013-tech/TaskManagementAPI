using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using NLog;
using TaskManagementAPI.Data;
using TaskManagementAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
// Add services to the container.



builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseInMemoryDatabase(databaseName: "TaskManagementDb"));
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowLocalhost4200",
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:4200")
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
        {
            context.Request.Path = "/index.html"; await next();
        }
    });
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseCors("CorsPolicy");
//app.UseCors("AllowLocalhost4200");
app.UseAuthorization();

app.MapControllers();

app.Run();
