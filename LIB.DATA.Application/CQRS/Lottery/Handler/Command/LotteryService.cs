using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using LIB.API.Application.Contracts.Persistence;
using LIB.API.Application.CQRS.EqubMember.Request.Command;
using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Domain;
using MediatR;

public class LotteryService
{
    private readonly IEqubMemberRepository _equbMemberRepository;
    private readonly IEqubTypeRepository _equbTypeRepository;
    private readonly ILotteryRepository _lotteryRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private static readonly Random _random = new Random();
   

    public LotteryService(
        IEqubMemberRepository equbMemberRepository,
        IEqubTypeRepository equbTypeRepository,
        ILotteryRepository lotteryRepository,
        IMediator mediator, IMapper mapper)
    {
        _equbMemberRepository = equbMemberRepository;
        _equbTypeRepository = equbTypeRepository;
        _lotteryRepository = lotteryRepository;
       _mediator = mediator;
        _mapper = mapper;
    }

    public async Task RunLottery()
    {
        var equbTypes = (await _equbTypeRepository.GetAll()).Where(e => e.Status != "1");
        var equbMembers = (await _equbMemberRepository.GetAll()).Where(e => e.Status != "1").ToList();

        foreach (var equbType in equbTypes)
        {
            if (!int.TryParse(equbType.TimeQuantity, out int timeQuantity))
            {
                throw new ArgumentException($"Invalid TimeQuantity value for EqubType: {equbType.EqubName}");
            }

            var lastLotteryDate = await _lotteryRepository.GetLastLotteryDateByEqubType(equbType.EqubName);
            DateTime startDate = equbType.EqupStartDay;
            DateTime nextLotteryDate = lastLotteryDate == null || lastLotteryDate == DateTime.MinValue
           ? startDate
           : CalculateNextLotteryDate(lastLotteryDate.Value, equbType.TimeUnits, timeQuantity);

            // If multiple lotteries have been missed, catch up on them
            while (DateTime.Now.Date >= nextLotteryDate.Date)
            {
                var currentEligibleMembers = equbMembers
                    .Where(m => m.EqubType == equbType.EqubName && m.Status != "Winner")
                    .ToList();

                // Run lottery only if there are eligible members
                if (currentEligibleMembers.Any())
                {
                    await RunLotteryForEqubType(equbType, currentEligibleMembers, nextLotteryDate);

                    // Re-fetch the updated members list
                    equbMembers = (await _equbMemberRepository.GetAll()).Where(e => e.Status != "1").ToList();
                }

                // Update the next lottery date to the start of the next period
                nextLotteryDate = CalculateNextLotteryDate(nextLotteryDate, equbType.TimeUnits, timeQuantity);
            }
        }
    }

    private async Task RunLotteryForEqubType(EqubTypes equbType, List<EqubMembers> equbMembers, DateTime lotteryDate)
    {
        var eligibleMembers = equbMembers
            .Where(m => m.EqubType == equbType.EqubName && m.Status != "Winner")
            .ToList();

        if (eligibleMembers.Count == 0)
            return;

        // Select a single winner
        var winner = eligibleMembers[_random.Next(eligibleMembers.Count)];

        var lottery = new Lotteries
        {
            EqubType = equbType.EqubName,
            EqubMember = winner.FullName,
            LotteryDate = lotteryDate,
            Status = "Winner",
            LotteryRound = await _lotteryRepository.GetNextLotteryRound(equbType.EqubName),
            UpdatedDate = DateTime.Now.ToString(),
            UpdatedBy = winner.Id
        };

        await _lotteryRepository.AddLot(lottery);

        // Update only the selected winner's status using the command
        var updateCommand = new UpdateEqubMemberCommand
        {
            EqubMemberDto = new EqubMemberDto
            {
                Id = winner.Id,
                EqubType = winner.EqubType,
                FullName = winner.FullName,
                PhoneNo = winner.PhoneNo,
                CompletedStatus = winner.CompletedStatus,
                NoOfTimesPaid = winner.NoOfTimesPaid,
                TotalAmountPaid = winner.TotalAmountPaid,
                PenalityAmount = winner.PenalityAmount,
                PenalityType = winner.PenalityType,
                TotalAmountLeft = winner.TotalAmountLeft,
                Status = "Winner", // Update the status
                UpdatedDate = winner.UpdatedDate, // Update the updated date
                UpdatedBy = "Systems",
                PaidPenalityAmount = winner.PaidPenalityAmount
            }
        };

        await _mediator.Send(updateCommand);
        var result = await _mediator.Send(updateCommand);
    }

    private DateTime CalculateNextLotteryDate(DateTime startDate, string timeUnit, int timeQuantity)
    {
        return timeUnit.ToLower() switch
        {
            "day" => startDate.AddDays(timeQuantity),
            "week" => startDate.AddDays(timeQuantity * 7),
            "month" => startDate.AddMonths(timeQuantity),
            _ => throw new ArgumentException("Invalid time unit")
        };
    }
}
