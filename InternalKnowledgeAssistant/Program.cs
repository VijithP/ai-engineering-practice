using InternalKnowledgeAssistant.Services;

var builder =
    WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Controllers
// --------------------------------------------------

builder.Services.AddControllers();

// --------------------------------------------------
// Swagger
// --------------------------------------------------

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------
// Application Services
// --------------------------------------------------

builder.Services.AddSingleton<DocumentService>();

builder.Services.AddSingleton<EmbeddingService>();

builder.Services.AddSingleton<VectorStoreService>();

builder.Services.AddSingleton<HybridSearchService>();

builder.Services.AddSingleton<GuardrailService>();

builder.Services.AddSingleton<CacheService>();

builder.Services.AddSingleton<OpenAiService>();

builder.Services.AddSingleton<KnowledgeAssistantService>();

// --------------------------------------------------
// CORS
// --------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// --------------------------------------------------
// Build
// --------------------------------------------------

var app = builder.Build();

// --------------------------------------------------
// Swagger
// --------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

// --------------------------------------------------
// Middleware
// --------------------------------------------------

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseStaticFiles();

app.MapControllers();

// --------------------------------------------------
// Root page
// --------------------------------------------------

app.MapGet(
    "/",
    () =>
        Results.Content(
            """
            <!DOCTYPE html>
            <html>
            <head>
                <title>Internal Knowledge Assistant</title>
            </head>
            <body>
                <h1>Internal Knowledge Assistant</h1>
                <p>API is running.</p>
                <p>Swagger: <a href="/swagger">Open Swagger</a></p>
            </body>
            </html>
            """,
            "text/html"));

app.Run();