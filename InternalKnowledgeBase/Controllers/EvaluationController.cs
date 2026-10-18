using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalKnowledgeBase.Data;

public class EvaluationController : Controller
{
    private readonly ApplicationDbContext _db;

    public EvaluationController(
        ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var evaluations =
            await _db.Evaluations
                .OrderByDescending(
                    x => x.EvaluatedAt
                )
                .ToListAsync();

        return View(evaluations);
    }
}