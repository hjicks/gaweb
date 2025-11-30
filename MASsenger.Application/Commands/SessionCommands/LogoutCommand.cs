using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.SessionCommands
{
    public record LogoutCommand(int SessionId) : IRequest<Result<BaseResponse>>;
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<BaseResponse>>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork)
        {
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<BaseResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var dbSession = await _sessionRepository.GetByIdAsync(request.SessionId);
            if (dbSession == null)
                return new Result<BaseResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Description = "Session not found."
                };

            dbSession.IsExpired = true;
            _sessionRepository.Update(dbSession);
            await _unitOfWork.SaveAsync();

            return new Result<BaseResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Description = "Log out successful."
            };
        }
    }
}
