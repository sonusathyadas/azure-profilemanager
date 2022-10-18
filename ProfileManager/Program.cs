using ProfileManager.Helpers;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration.GetValue<string>("Azure:ApplicationInsights:InstrumentationKey"));
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetValue<string>("Azure:Redis:ConnectionString");
//     options.InstanceName = builder.Configuration.GetValue<string>("Azure:Redis:Prefix");
// });
builder.Services.AddDistributedMemoryCache();

//Register Azure services
builder.Services.AddScoped<BlobStorageHelper>();
builder.Services.AddScoped<CosmosHelper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
