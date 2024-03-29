using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services
    .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(o => { o.DetailedErrors = true;});
builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; });
builder.Services.AddMediaQueryService();
builder.Services.AddScoped<RoundService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<User>>();
builder.Services.AddScoped<IClaimsTransformation, ApplicationUserClaimsTransformation>();
builder.Services.AddHttpContextAccessor();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationId();
});
builder.Services.AddTransient<IEmailSender, EmailService>();
builder.Services.Configure<EmailSettings>(con => builder.Configuration?.GetSection(nameof(EmailSettings)).Bind(con));
builder.Logging.AddSerilog();

var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetSection("SyncfusionKey")?.Value);
app.Logger.LogInformation("Starting Wordle Clone Wars");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevOrLocal())
{
    app.UseMigrationsEndPoint();
}
else
{
    await SeedData.EnsureSeedDataAsync(app.Services);
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
