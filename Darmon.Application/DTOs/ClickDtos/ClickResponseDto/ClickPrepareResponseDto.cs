using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ClickDtos.ClickResponseDto;

public class ClickPrepareResponseDto
{
    public int Click_Trans_Id { get; set; }
    public int Merchant_Trans_Id { get; set; }
    public int Merchant_Prepare_Id { get; set; }
    public int Error { get; set; }
    public string? Error_Note { get; set; }
}
