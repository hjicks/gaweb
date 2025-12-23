using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.SessionQueries;

public record GetAllSessionsQuery() : IRequest<Result>;
public class GetAllSessionsQueryHandler : IRequestHandler<GetAllSessionsQuery, Result>
{
    private readonly ISessionRepository _sessionRepository;
    public GetAllSessionsQueryHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
    public async Task<Result> Handle(GetAllSessionsQuery request, CancellationToken cancellationToken)
    {
        var sessions = (await _sessionRepository.GetAllAsync()).Select(s => new SessionGetDto
        {
            Id = s.Id,
            ExpiresAt = s.ExpiresAt,
            UserId = s.UserId,
            ClientName = s.ClientName,
            OS = s.OS,
            IsRevoked = s.IsRevoked,
            RevokedAt = s.RevokedAt,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, sessions);
    }
}
