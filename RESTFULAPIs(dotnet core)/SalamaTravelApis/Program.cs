using SalamaTravelApis.Controllers;
using SalamaTravelDTA;

var builder = WebApplication.CreateBuilder(args);

// Load settings
Settings.LoadConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
              "https://salama-travel.netlify.app"
            )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Use middleware (Order is important)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔁 لازم تيجي قبل UseAuthorization
app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

// Map endpoints
app.UseEndpoints(endpoints =>
{
   _= endpoints.MapControllers();
    _=endpoints.MapHub<VisasPricesHub>("/VisaHub");
});

app.Run();
