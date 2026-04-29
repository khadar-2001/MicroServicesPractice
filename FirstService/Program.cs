using Microsoft.AspNetCore.RateLimiting;
using MiddlewareLib;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", r =>
    {
        r.PermitLimit = 5;
        r.Window = TimeSpan.FromSeconds(30);
        r.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        r.QueueLimit = 1;
    });

    /*
    rloptions.AddSlidingWindowLimiter("SlidePolicy", options => {
        options.PermitLimit = 10;
        options.Window=TimeSpan.FromSeconds(30);
        options.SegmentsPerWindow = 5;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    }); 

    rloptions.AddConcurrencyLimiter("QueuePolicy", options => {
        options.PermitLimit = 5;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });      

    rloptions.AddTokenBucketLimiter("BucketPolicy", options => {
        options.TokenLimit = 5;        
        options.TokensPerPeriod = 5;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(20);
        options.AutoReplenishment = true;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });
    */
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseMiddleware<APIInterceptor>();
app.UseRateLimiter();
app.MapControllers();

app.Run();
