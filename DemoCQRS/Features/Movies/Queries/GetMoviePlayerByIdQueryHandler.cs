using DemoCQRS.Data;
using MediatR;

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
