using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record DeletePrivateChatCommand(Int32 privateChatId) : IRequest<TransactionResultType>;
    public class DeletePrivateChatCommandHandler : IRequestHandler<DeletePrivateChatCommand, TransactionResultType>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeletePrivateChatCommandHandler(IPrivateChatRepository privateChatRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(DeletePrivateChatCommand request, CancellationToken cancellationToken)
        {
            var privateChat = await _privateChatRepository.GetByIdAsync(request.privateChatId);
            if (privateChat == null)
                return TransactionResultType.ForeignKeyNotFound;
            _privateChatRepository.Delete(privateChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
