using Darmon.Application.DTOs.ClickDtos.ClickrequestDto;
using Darmon.Application.DTOs.ClickDtos.ClickResponseDto;
using Darmon.Application.DTOs.ClickDtos.RequestResponseDto;
using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Interfaces;

public interface IClickPaymentService
{
    Task<ClickPrepareResponseDto> Prepare(ClickPrepareRequestDto clickRequest);
    Task<ClickCompleteResponseDto> Complete(ClickCompleteRequestDto clickRequest);
    Task<RequestResponseDto<string>> Create(float amount, int userId);
    Task<string> GenerateSignString_Prepare(long clickTransId, int serviceId, string merchantTransId, float amount, int action, string signTime);
    Task<string> GenerateSignString_Complete(long click_trans_id, int service_id, long click_paydoc_id, string merchant_trans_id, int merchant_prepare_id, float amount, int action, string sign_time);
}
