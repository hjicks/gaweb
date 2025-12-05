using MASsenger.Application.Dtos.ChannelChatDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Commands.ChannelChatCommands
{
    public record UpdateChannelChatCommand(ChannelChatUpdateDto channelChat) : IRequest<TransactionResultType>;
    public class UpdateChannelChatCommandHandler : IRequestHandler<UpdateChannelChatCommand, TransactionResultType>
    {
        private readonly IChannelChatRepository _channelChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateChannelChatCommandHandler(IChannelChatRepository channelChatRepository, IUnitOfWork unitOfWork)
        {
            _channelChatRepository = channelChatRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateChannelChatCommand request, CancellationToken cancellationToken)
        {
            var channelChat = await _channelChatRepository.GetByIdAsync(request.channelChat.Id);
            if (channelChat == null)
                return TransactionResultType.ForeignKeyNotFound;
            channelChat.Name = request.channelChat.Name;
            channelChat.Username = request.channelChat.Username;
            channelChat.Description = request.channelChat.Description;
            _channelChatRepository.Update(channelChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
