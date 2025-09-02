using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.ClickDtos.ClickrequestDto;

public class ClickCompleteRequestDto
{
    public int Click_Trans_Id { get; set; }
    public int Service_Id { get; set; }
    public int Merchant_Trans_Id { get; set; }
    public decimal Amount { get; set; }
    public int Action { get; set; }
    public int Error { get; set; }
    public string? Error_Note { get; set; }
    public string? Sign_Time { get; set; }
    public string? Sign_String { get; set; }
    public int Merchant_Prepare_Id { get; set; }
}
