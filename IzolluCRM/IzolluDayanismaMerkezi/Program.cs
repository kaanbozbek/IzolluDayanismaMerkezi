using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using IzolluVakfi.Data;
using IzolluVakfi.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure QuestPDF License
QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

// Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        sqliteOptions.CommandTimeout(60); // 60 seconds timeout
    });
    // Enable detailed errors in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Add Application Services
builder.Services.AddScoped<ActivityLogService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<DonorService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<MemberScholarshipCommitmentService>();
builder.Services.AddScoped<SystemSettingsService>(); // System-wide settings
builder.Services.AddScoped<ScholarshipPaymentService>(); // Payment tracking
builder.Services.AddScoped<TranscriptService>();
builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<MeetingService>();
builder.Services.AddScoped<VillageService>();
builder.Services.AddScoped<AidService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Create database if it doesn't exist and initialize settings
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    
    // Enable SQLite WAL mode for better concurrency and performance
    try
    {
        await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL;");
        await context.Database.ExecuteSqlRawAsync("PRAGMA synchronous = NORMAL;");
        await context.Database.ExecuteSqlRawAsync("PRAGMA temp_store = MEMORY;");
        await context.Database.ExecuteSqlRawAsync("PRAGMA mmap_size = 30000000000;");
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("SQLite WAL mode and performance optimizations enabled");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(ex, "Failed to enable SQLite WAL mode");
    }
    
    var settingsService = scope.ServiceProvider.GetRequiredService<SettingsService>();
    await settingsService.InitializeDefaultSettingsAsync();
    
    // Initialize system settings (creates if not exists)
    var systemSettingsService = scope.ServiceProvider.GetRequiredService<SystemSettingsService>();
    await systemSettingsService.GetOrCreateSettingsAsync();
}

app.Run();
