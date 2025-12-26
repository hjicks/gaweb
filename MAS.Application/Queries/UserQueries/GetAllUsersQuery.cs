using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.UserQueries;

public record GetAllUsersQuery() : IRequest<Result>;
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IBlobService _blobService;
    public GetAllUsersQueryHandler(IUserRepository userRepository, IBlobService blobService) 
    {
        _userRepository = userRepository;
        _blobService = blobService;
    }
    public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetAllAsync()).Select(u => new UserGetDto
        {
            Id = u.Id,
            DisplayName = u.DisplayName,
            Username = u.Username!,
            Bio = u.Bio,
            Avatar = _blobService.EncodeBlobToBase64(u.Avatar!),
            IsVerified = u.IsVerified,
            IsBot = u.IsBot,
            LastSeenAt = u.LastSeenAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, users);
    }
}
