using DemoCQRS.Data;
using MediatR;

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
