
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using LIB.API.Application.CQRS.InRtgsCbc.Request.Command;
using LIB.API.Application.DTOs.InRtgsCbc.Validators;
using LIBPROPERTY.Application.Exceptions;

using LIB.API.Application.Contracts.Persistence  ;
using LIB.API.Domain;


public class UpdateInRtgsCbcCommandHandler : IRequestHandler<UpdateInRtgsCbcCommand, Unit>
{
    public IInRtgsCbcRepository _InRtgsCbcRepository;
    public IMapper _mapper;

    public UpdateInRtgsCbcCommandHandler(IInRtgsCbcRepository InRtgsCbcRepository, IMapper mapper)
    {
        _InRtgsCbcRepository = InRtgsCbcRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateInRtgsCbcCommand request, CancellationToken cancellationToken)
    {
        var validator = new InRtgsCbcDtoValidators();
        var validationResult = await validator.ValidateAsync(request.InRtgsCbcDto);
        if (validationResult.IsValid == false)
            throw new ValidationException(validationResult);

        var use = await _InRtgsCbcRepository.GetInRtgsCbcByReferenceNoAsync(request.InRtgsCbcDto.REFNO);



        var add = _mapper.Map(request.InRtgsCbcDto, use);

        await _InRtgsCbcRepository.Update(add);
        return Unit.Value;
    }
}

