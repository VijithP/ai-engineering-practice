using Microsoft.EntityFrameworkCore;
using InternalKnowledgeBase.Data;
using InternalKnowledgeBase.Services;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var openAiApiKey =
    Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    ?? throw new InvalidOperationException(
        "The OPENAI_API_KEY environment variable is required."
    );

builder.Services.AddSingleton<DocumentService>();
builder.Services.AddSingleton<KeywordSearchService>();
builder.Services.AddSingleton<VectorSearchService>(
    _ => new VectorSearchService(openAiApiKey)
);
builder.Services.AddSingleton<HybridSearchService>();
builder.Services.AddSingleton<RagService>(
    _ => new RagService(openAiApiKey)
);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseSqlite(
            "Data Source=knowledgebase.db"
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
