using MediatR;

namespace DemoCQRS.Features.Movies.Commands;

public sealed record CreateMovieCommand(string Name) : IRequest<int>;
