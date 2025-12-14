using MAS.Application.Interfaces;
using MAS.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Commands.ChannelChatCommands
{
    public record DeleteChannelChatCommand(Int32 channelChatId) : IRequest<TransactionResultType>;
    public class DeleteChannelChatCommandHandler : IRequestHandler<DeleteChannelChatCommand, TransactionResultType>
    {
        private readonly IChannelChatRepository _channelChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteChannelChatCommandHandler(IChannelChatRepository channelChatRepository, IUnitOfWork unitOfWork)
        {
            _channelChatRepository = channelChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(DeleteChannelChatCommand request, CancellationToken cancellationToken)
        {
            var channelChat = await _channelChatRepository.GetByIdAsync(request.channelChatId);
            if (channelChat == null)
                return TransactionResultType.ForeignKeyNotFound;
            _channelChatRepository.Delete(channelChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
