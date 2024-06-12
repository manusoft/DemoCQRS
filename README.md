# DemoCQRS - ASP.NET Web API Project

DemoCQRS is an ASP.NET Web API project showcasing the implementation of the Command Query Responsibility Segregation (CQRS) pattern. The project demonstrates two versions: one without using MediatR and another with MediatR.

## Branches

### Branch: `samples/without-mediatr`

This branch contains a simple implementation of CQRS without using the MediatR library.

#### Movie.cs

```csharp
namespace DemoCQRS.Data;
public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

#### AppDbContext.cs

```csharp
namespace DemoCQRS.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Movie> Movies { get; set; }
}
```

#### MovieController.cs

```csharp
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
```

#### Program.cs

```csharp
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));
```

### Branch: `master`

This branch includes the implementation of CQRS using the MediatR library.

#### Program.cs

```csharp
// Add MediatR services
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
```

#### CreateMovieCommand.cs

```csharp
namespace DemoCQRS.Features.Movies.Commands;
public sealed record CreateMovieCommand(string Name) : IRequest<int>;
```

#### CreatePlayerCommandHandler.cs

```csharp
namespace DemoCQRS.Features.Movies.Commands;
public sealed class CreatePlayerCommandHandler : IRequestHandler<CreateMovieCommand, int>
{
    private readonly AppDbContext _context;

    public CreatePlayerCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie() { Name = request.Name };
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie.Id;
    }
}
```

#### GetMovieByIdQuery.cs

```csharp
namespace DemoCQRS.Features.Movies.Queries;
public sealed record GetMovieByIdQuery(int Id): IRequest<Movie?>;
```

#### GetMoviePlayerByIdQueryHandler.cs

```csharp
namespace DemoCQRS.Features.Movies.Queries;
public sealed class GetMoviePlayerByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Movie?>
{
    private readonly AppDbContext _context;

    public GetMoviePlayerByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Movie?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FindAsync(request.Id);
        return (movie);
    }
}
```

#### MovieWithMediatRController.cs

```csharp
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
```

## Setup Instructions

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```
2. Switch to the desired branch:
   ```bash
   git checkout <branch-name>
   ```
3. Restore the dependencies:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

## Summary

This project demonstrates how to implement CQRS in an ASP.NET Web API project, with and without the MediatR library. Each branch contains a complete example, allowing for a direct comparison between the two approaches.
