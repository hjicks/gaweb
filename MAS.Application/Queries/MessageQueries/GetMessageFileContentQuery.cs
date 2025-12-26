using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Queries.MessageQueries;

public record GetMessageFileContentQuery(int MessageId) : IRequest<Result>;
public class GetMessageFileContentQueryHandler : IRequestHandler<GetMessageFileContentQuery, Result>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBaseRepository<FileContent> _fileContentRepository;
    private readonly IBlobService _blobService;
    public GetMessageFileContentQueryHandler(IMessageRepository messageRepository, IUserRepository userRepository,
        IBaseRepository<FileContent> fileContentRepository, IBlobService blobService)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _fileContentRepository = fileContentRepository;
        _blobService = blobService;
    }
    public async Task<Result> Handle(GetMessageFileContentQuery request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.MessageNotFound);

        var content = await _fileContentRepository.GetByIdAsync(request.MessageId);
        if (content == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.FileContentNotFound);

        Log.Information($"Message {content.MessageId} file contents fetched.");
        return Result.Success(StatusCodes.Status200OK, new FileGetDto
        {
            MessageId = content.MessageId,
            Content = _blobService.EncodeBlobToBase64(content.Content)
        });
    }
}
