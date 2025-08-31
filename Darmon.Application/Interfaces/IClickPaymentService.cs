using Darmon.Application.DTOs.PaymentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IClickPaymentService
{
   
        Task<ClickResponseDto> PrepareAsync(PrepareRequestDto dto);
        Task<ClickResponseDto> CompleteAsync(CompleteRequestDto dto);
    
}
