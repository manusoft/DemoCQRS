using DemoCQRS.Data;
using DemoCQRS.Features.Movies.Commands;
using DemoCQRS.Features.Movies.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace DemoCQRS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieWithMediatRController : ControllerBase
{
    private readonly ISender _sender;

    public MovieWithMediatRController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieCommand command)     
    {
        var movieId = await _sender.Send(command);
        return Ok(movieId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var movie = await _sender.Send(new GetMovieByIdQuery(id));

        if(movie == null) 
            return NotFound();

        return Ok(movie);
    }
}
