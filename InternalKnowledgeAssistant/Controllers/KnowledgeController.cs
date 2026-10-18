using InternalKnowledgeAssistant.Models;
using InternalKnowledgeAssistant.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalKnowledgeAssistant.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KnowledgeController : ControllerBase
{
    private readonly KnowledgeAssistantService _assistant;

    public KnowledgeController(
        KnowledgeAssistantService assistant)
    {
        _assistant = assistant;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask(
        [FromBody] KnowledgeRequest request)
    {
        try
        {
            var response =
                await _assistant.AskAsync(
                    request.Question);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(
                new
                {
                    error = ex.Message
                });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message
                });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(
            new
            {
                status = "healthy",
                service =
                    "Internal Knowledge Assistant",
                timestamp =
                    DateTime.UtcNow
            });
    }
}