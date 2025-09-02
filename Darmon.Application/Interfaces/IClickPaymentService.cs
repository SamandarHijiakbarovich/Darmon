using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.DTOs.PaymentDtos;
using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IClickPaymentService
{
    Task<ClickPrepareResponseDto> ProcessPrepareRequestAsync(ClickPrepareRequestDto request);
    Task<ClickCompleteResponseDto> ProcessCompleteRequestAsync(ClickCompleteRequestDto request);
    Task<bool> ValidateClickSignatureAsync(ClickPrepareRequestDto request);
    Task<string> GenerateClickSignatureAsync(ClickPrepareRequestDto request);
}
