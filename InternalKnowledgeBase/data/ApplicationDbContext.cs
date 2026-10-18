using Microsoft.EntityFrameworkCore;

namespace InternalKnowledgeBase.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<EvaluationResult> Evaluations => Set<EvaluationResult>();
}
