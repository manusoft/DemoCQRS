using DemoCQRS.Data;
using MediatR;

namespace DemoCQRS.Features.Movies.Queries;

public sealed record GetMovieByIdQuery(int Id): IRequest<Movie?>;
