using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record AddPrivateChatCommand() : IRequest<TransactionResultType>;
    public class AddPrivateChatCommandHandler : IRequestHandler<AddPrivateChatCommand, TransactionResultType>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddPrivateChatCommandHandler(IPrivateChatRepository privateChatRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(AddPrivateChatCommand request, CancellationToken cancellationToken)
        {
            var newPrivateChat = new PrivateChat();
            await _privateChatRepository.Add(newPrivateChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
