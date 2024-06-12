using DemoCQRS.Data;
using Microsoft.AspNetCore.Mvc;

namespace DemoCQRS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _context;
    public MovieController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return Ok(movie.Id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        return Ok(movie);
    }
}
