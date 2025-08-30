using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.DTOs.CategotyDtos;

public class CreateCategoryDto
{
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }
    public string ImageUrl { get; set; }
}
