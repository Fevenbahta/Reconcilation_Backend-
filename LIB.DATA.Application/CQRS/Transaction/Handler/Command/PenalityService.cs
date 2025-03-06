/*using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Command
{
    public class PenaltyService
    {
        private readonly IEqubMemberRepository _equbMemberRepository;
        private readonly IEqubTypeRepository _equbTypeRepository;

        public PenaltyService(IEqubMemberRepository equbMemberRepository, IEqubTypeRepository equbTypeRepository)
        {
            _equbMemberRepository = equbMemberRepository;
            _equbTypeRepository = equbTypeRepository;
        }

        public async Task UpdatePenalties()
        {
            var equbMembers = (await _equbMemberRepository.GetAll()).Where(e=>e.Status!="1");
            DateTime currentDate = DateTime.Now;

            foreach (var equbMember in equbMembers)
            {
                // Only proceed if the member's completed status is not "COMPLETED"
                if (equbMember.CompletedStatus != "Completed")
                {
                    var equbType = await _equbTypeRepository.GetByName(equbMember.EqubType);
                    if (equbType == null)
                    {
                        throw new Exception($"EqubType with Name {equbMember.EqubType} not found.");
                    }

                    DateTime startDate = equbType.EqupStartDay;
                    decimal totalPenaltyAmount = CalculatePenaltyAmount(equbType, startDate, currentDate);

                    // Update EqubMember with penalty amount and status
                    equbMember.PenalityAmount = totalPenaltyAmount.ToString();
                    equbMember.UpdatedDate = currentDate.ToString();

                    // Save changes to EqubMember
                    await _equbMemberRepository.Update(equbMember);
                }
            }
        }


        public decimal CalculatePenaltyAmount(EqubTypes equbType, DateTime startDate, DateTime currentDate)
        {
            int gracePeriodDays = GetGracePeriodDays(equbType.TimeUnits, equbType.TimeQuantity);
            TimeSpan timeElapsed = currentDate - startDate;
            TimeSpan gracePeriodElapsed = timeElapsed - TimeSpan.FromDays(gracePeriodDays);

            if (gracePeriodElapsed.TotalDays <= 0)
            {
                return 0;
            }

            int penaltyPeriods = CalculatePenaltyPeriods(equbType.PenalityFrequency, gracePeriodElapsed, startDate, currentDate);
            return CalculatePenaltyAmount(equbType.PenalityType, equbType.PenalityAmount, decimal.Parse(equbType.Amount), penaltyPeriods);
        }

        private int GetGracePeriodDays(string timeUnits, string timeQuantity)
        {
            int timeQty = int.Parse(timeQuantity);
            return timeUnits.ToLower() switch
            {
                "day" => timeQty,
                "week" => timeQty * 7,
                "month" => timeQty * 30, // Approximation
                _ => throw new Exception("Invalid time units."),
            };
        }

        private int CalculatePenaltyPeriods(string penaltyFrequency, TimeSpan gracePeriodElapsed, DateTime startDate, DateTime currentDate)
        {
            return penaltyFrequency switch
            {
                "Daily" => (int)Math.Floor(gracePeriodElapsed.TotalDays),
                "Weekly" => (int)Math.Floor(gracePeriodElapsed.TotalDays / 7),
                "Monthly" => GetElapsedMonths(startDate, currentDate) - (int)Math.Floor(gracePeriodElapsed.TotalDays / 30),
                _ => throw new Exception("Invalid penalty frequency."),
            };
        }

        private int GetElapsedMonths(DateTime startDate, DateTime currentDate)
        {
            int totalMonthsDifference = ((currentDate.Year - startDate.Year) * 12) + (currentDate.Month - startDate.Month);
            if (currentDate.Day < startDate.Day)
            {
                totalMonthsDifference--;
            }
            return totalMonthsDifference;
        }
private decimal CalculatePenaltyAmount(string penaltyType, decimal penaltyAmount, decimal equbAmount, int penaltyPeriods)
        {
            return penaltyType switch
            {
                "Fixed ETB" => penaltyAmount * penaltyPeriods,
                "Percent" => (equbAmount * (penaltyAmount / 100)) * penaltyPeriods,
                _ => throw new Exception("Invalid penalty type."),
            };
        }


        public async Task UpdateEqubTypeStatusIfCompleted()
        
        {
            var equbTypes = (await _equbTypeRepository.GetAll()).Where(e=>e.Status !="1");

            foreach (var equbType in equbTypes)
            {
                var equbMembers = (await _equbMemberRepository.GetAll())
                    .Where(m => m.EqubType == equbType.EqubName && m.Status !="1")
                    .ToList();
                if(equbMembers.Count !=0 )
                { // Check if all members are completed and have an empty penalty amount
                bool allMembersCompleted = equbMembers.All(m => m.CompletedStatus == "Completed" );

                if (allMembersCompleted)
                {
                    // Update the EqubType status
                    equbType.Status = "Terminated"; // Assuming "1" represents the desired status
                    equbType.UpdatedDate = DateTime.Now.ToString();
                    equbType.UpdatedBy = "System";
                    await _equbTypeRepository.Update(equbType);
                }

                }
               
            }
        }

    }


}
*/

using LIB.API.Application.Contracts.Persistence;
using LIB.API.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LIB.API.Application.CQRS.Transaction.Handler.Command
{
    public class PenaltyService
    {
        private readonly IEqubMemberRepository _equbMemberRepository;
        private readonly IEqubTypeRepository _equbTypeRepository;

        public PenaltyService(IEqubMemberRepository equbMemberRepository, IEqubTypeRepository equbTypeRepository)
        {
            _equbMemberRepository = equbMemberRepository;
            _equbTypeRepository = equbTypeRepository;
        }

        public async Task UpdatePenalties()
        {
            var equbMembers = (await _equbMemberRepository.GetAll()).Where(e => e.Status != "1");
            DateTime currentDate = DateTime.Now;

            foreach (var equbMember in equbMembers)
            {
                if (equbMember.CompletedStatus != "Completed")
                {
                    var equbType = await _equbTypeRepository.GetByName(equbMember.EqubType);
                    if (equbType == null)
                    {
                        throw new Exception($"EqubType with Name {equbMember.EqubType} not found.");
                    }

                    DateTime startDate = equbType.EqupStartDay;
                    DateTime lastUpdatedDate = DateTime.Parse(equbMember.UpdatedDate);
                    decimal totalAmountPaid = equbMember.TotalAmountPaid;
                    decimal paidPenaltyAmount = decimal.Parse(equbMember.PaidPenalityAmount);

                    // Check if the penalty needs to be applied
                    bool applyPenalty = ShouldApplyPenalty(equbType.TimeUnits, equbType.TimeQuantity, startDate, currentDate, lastUpdatedDate, totalAmountPaid, paidPenaltyAmount, equbType, equbMember);



                    if (applyPenalty)


                    {



                        decimal calculatedPenalty = CalculatePenaltyAmount(equbType, lastUpdatedDate, currentDate, equbMember);

                        // Convert the existing PenalityAmount from string to decimal
                        decimal currentPenaltyAmount = decimal.Parse(equbMember.PenalityAmount);


                        // Add the calculated penalty to the current penalty amount
                        decimal updatedPenaltyAmount = currentPenaltyAmount + calculatedPenalty;
                        // Convert the result back to string and assign it to PenalityAmount
                        equbMember.PenalityAmount = updatedPenaltyAmount.ToString();

                        equbMember.UpdatedDate = currentDate.ToString();
                        await _equbMemberRepository.Update(equbMember);
                    }
                }
            }
        }

        private bool ShouldApplyPenalty(string timeUnits, string timeQuantity, DateTime startDate, DateTime currentDate, DateTime lastUpdatedDate, decimal totalAmountPaid, decimal paidPenaltyAmount, EqubTypes equbType, EqubMembers equbMember)
        {
            int gracePeriodDays = GetGracePeriodDays(timeUnits, timeQuantity);

            // Truncate time from currentDate and lastUpdatedDate to only consider the date part
            DateTime currentDateTruncated = currentDate.Date;
            DateTime startDateTruncated = startDate.Date;
            DateTime lastUpdatedDateTruncated = lastUpdatedDate.Date;

            TimeSpan timeElapsed = currentDateTruncated - startDateTruncated;
            TimeSpan gracePeriodElapsed = currentDateTruncated - lastUpdatedDateTruncated;


            if (lastUpdatedDateTruncated <= startDateTruncated)
            {
                gracePeriodElapsed = currentDateTruncated - startDateTruncated;

            }

            // Calculate the total amount that should be paid
            decimal totalAmountToPay = decimal.Parse(equbType.Amount) * GetTotalPeriods(timeUnits, timeQuantity, startDateTruncated, currentDateTruncated);

            // Determine if the payment is less than expected and if the grace period is exceeded
            bool paymentIsInsufficient = (totalAmountPaid - paidPenaltyAmount) < totalAmountToPay;

            bool gracePeriodExceeded = false;
            if(equbType.Amount == "100")
            {
                var n = 0;
            }
             if (lastUpdatedDateTruncated <= startDateTruncated && totalAmountPaid == 0)
            {
                gracePeriodExceeded = gracePeriodElapsed.TotalDays >= 1;

            }
           else  if (lastUpdatedDateTruncated <= startDateTruncated  )
            {
                gracePeriodExceeded = gracePeriodElapsed.TotalDays >= gracePeriodDays;

            }
         
            else
            {
                gracePeriodExceeded = gracePeriodElapsed.TotalDays >= gracePeriodDays;


            }

            if (gracePeriodExceeded)
            {
                bool X = true;
            }
            if (!(paymentIsInsufficient && gracePeriodExceeded) && ( gracePeriodElapsed.TotalDays == gracePeriodDays || gracePeriodElapsed.TotalDays==1) )
            {
                equbMember.UpdatedDate = currentDate.ToString();
            }
            return paymentIsInsufficient && gracePeriodExceeded;

        }

        public decimal CalculatePenaltyAmount(EqubTypes equbType, DateTime startDate, DateTime currentDate, EqubMembers equbMember)
        {
            // Get the grace period in days
            int gracePeriodDays = GetGracePeriodDays(equbType.TimeUnits, equbType.TimeQuantity);
            TimeSpan timeElapsed = currentDate.Date - startDate.Date;
            TimeSpan gracePeriodElapsed = timeElapsed;

            // No penalty if within the grace period
            if (gracePeriodElapsed.TotalDays <= 0)
            {
                return 0;
            }

            // Calculate the number of periods missed based on grace period elapsed
            int missedPeriods = (int)Math.Floor(gracePeriodElapsed.TotalDays / gracePeriodDays);
            if (DateTime.Parse(equbMember.UpdatedDate).Date <= equbType.EqupStartDay.Date && equbMember.TotalAmountPaid == 0)
            {
                missedPeriods = 1;
            }
            // Calculate the total amount to be paid
            int totalPeriods = GetTotalPeriods(equbType.TimeUnits, equbType.TimeQuantity, startDate, currentDate);
            decimal totalAmountToPay = decimal.Parse(equbType.Amount) * totalPeriods;
            // Amount actually paid and amount left to be paid
            decimal amountPaid = equbMember.TotalAmountPaid - decimal.Parse(equbMember.PaidPenalityAmount);
            decimal amountLeftToPay = totalAmountToPay - amountPaid;

            // Ensure that missed periods are correctly adjusted based on amount paid
            //  missedPeriods = Math.Min(totalPeriods, (int)Math.Floor((amountLeftToPay) / decimal.Parse(equbType.Amount)));

            // Calculate the penalty based on the missed periods
            decimal penalty = CalculatePenaltyAmount(equbType.PenalityType, equbType.PenalityAmount, decimal.Parse(equbType.Amount), missedPeriods);

            return penalty;
        }


        private int GetTotalPeriods(string timeUnits, string timeQuantity, DateTime startDate, DateTime endDate)
        {
            int totalDays = (endDate - startDate).Days;
            int periodQuantity = int.Parse(timeQuantity);

            switch (timeUnits.ToLower())
            {
                case "day":
                    return totalDays / periodQuantity;
                case "week":
                    return (totalDays / (7 * periodQuantity) + 1);
                case "month":
                    return (totalDays / (30 * periodQuantity) + 1); // Approximation
                default:
                    throw new ArgumentException("Invalid time units");
            }
        }


        private int GetGracePeriodDays(string timeUnits, string timeQuantity)
        {
            int timeQty = int.Parse(timeQuantity);
            return timeUnits.ToLower() switch
            {
                "day" => timeQty,
                "week" => timeQty * 7,
                "month" => timeQty * 30, // Approximation
                _ => throw new Exception("Invalid time units."),
            };
        }


        private decimal CalculatePenaltyAmount(string penaltyType, decimal penaltyAmount, decimal equbAmount, int penaltyPeriods)
        {
            return penaltyType switch
            {
                "Fixed ETB" => penaltyAmount * penaltyPeriods,
                "Percent" => (equbAmount * (penaltyAmount / 100)) * penaltyPeriods,
                _ => throw new Exception("Invalid penalty type."),
            };
        }

        public async Task UpdateEqubTypeStatusIfCompleted()
        {
            var equbTypes = (await _equbTypeRepository.GetAll()).Where(e => e.Status != "1");

            foreach (var equbType in equbTypes)
            {
                var equbMembers = (await _equbMemberRepository.GetAll())
                    .Where(m => m.EqubType == equbType.EqubName && m.Status != "1")
                    .ToList();
                if (equbMembers.Count != 0)
                {
                    // Check if all members are completed
                    bool allMembersCompleted = equbMembers.All(m => m.CompletedStatus == "Completed");

                    if (allMembersCompleted)
                    {
                        // Update the EqubType status
                        equbType.Status = "Terminated"; // Assuming "1" represents the desired status
                        equbType.UpdatedDate = DateTime.Now.ToString();
                        equbType.UpdatedBy = "System";
                        await _equbTypeRepository.Update(equbType);
                    }
                }
            }
        }

        private string GetEqubTypeAmount(string timeUnits, string timeQuantity)
        {

            return "100";
        }
    }
}
