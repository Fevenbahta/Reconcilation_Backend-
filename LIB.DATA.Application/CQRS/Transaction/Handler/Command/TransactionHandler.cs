using LIB.API.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Command
{
    public class TransactionHandler
    {
        private readonly PenaltyService _penaltyService;
        private readonly IEqubMemberRepository _equbMemberRepository;
        private readonly IEqubTypeRepository _equbTypeRepository;

        public TransactionHandler(PenaltyService penaltyService, IEqubMemberRepository equbMemberRepository, IEqubTypeRepository equbTypeRepository)
        {
            _penaltyService = penaltyService;
            _equbMemberRepository = equbMemberRepository;
            _equbTypeRepository = equbTypeRepository;
        }
        public async Task HandleAllTransactionsAsync(DateTime currentDate)
        {
            var equbMembers = await _equbMemberRepository.GetAll();

            foreach (var equbMember in equbMembers)
            {
                var equbType = await _equbTypeRepository.GetByName(equbMember.EqubType);
                decimal penaltyAmount = _penaltyService.CalculatePenaltyAmount( equbType, currentDate, equbType.EqupStartDay, equbMember);

                equbMember.PenalityAmount = penaltyAmount.ToString();
                equbMember.UpdatedDate = currentDate.ToString();

                await _equbMemberRepository.Update(equbMember);
            }
        }
    }

}
