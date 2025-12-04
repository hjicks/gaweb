using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.SessionCommands
{
    public record LogoutCommand(int SessionId) : IRequest<Result>;
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork)
        {
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var dbSession = await _sessionRepository.GetByIdAsync(request.SessionId);
            if (dbSession == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Session not found." });

            dbSession.IsExpired = true;
            _sessionRepository.Update(dbSession);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status200OK, new BaseResponse("Log out successful."));
        }
    }
}
