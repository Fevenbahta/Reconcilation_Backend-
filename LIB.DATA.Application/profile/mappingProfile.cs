using AutoMapper;
using LIB.API.Application.DTOs.AccountInfo;
using LIB.API.Application.DTOs.EqubMember;
using LIB.API.Application.DTOs.EqubType.Validators;
using LIB.API.Application.DTOs.InReconciled;
using LIB.API.Application.DTOs.InRtgsAts.Validators;
using LIB.API.Application.DTOs.InRtgsCbc;
using LIB.API.Application.DTOs.Lottery;
using LIB.API.Application.DTOs.OutReconciled;
using LIB.API.Application.DTOs.OutRtgsAts.Validators;
using LIB.API.Application.DTOs.OutRtgsCbc;
using LIB.API.Application.DTOs.Transaction;

using LIB.API.Application.DTOs.User;
using LIB.API.Domain;

namespace LIB.API.Application.profile
{
    public class mappingProfile : Profile
    {
        public mappingProfile()
        {
          
           
            CreateMap<Users, UserDto>().ReverseMap();
            CreateMap<Transactions, TransactionDto>().ReverseMap();
            CreateMap<AccountInfos, AccountInfoDto>().ReverseMap();
            CreateMap<EqubMembers, EqubMemberDto>().ReverseMap();
            CreateMap<EqubTypes, EqubTypeDto>().ReverseMap();
            CreateMap<Lotteries, LotteryDto>().ReverseMap();
            CreateMap<InRtgsCbcs, InRtgsCbcDto>().ReverseMap();
            ;
            CreateMap<OutRtgsCbcs, OutRtgsCbcDto>().ReverseMap();
            CreateMap<InRtgsAtss, InRtgsAtsDto>().ReverseMap();
            CreateMap<OutRtgsAtss, OutRtgsAtsDto>().ReverseMap();
            CreateMap<OutRtgsCbcs, OutRtgsCbcsOracle>().ReverseMap();
            CreateMap<OutReconcileds, OutReconciledDto>().ReverseMap();
            CreateMap<InReconcileds, InReconciledDto>().ReverseMap();
            CreateMap<InRtgsCbcs, InRtgsCbcsOracle>().ReverseMap();
        }
    }
}

