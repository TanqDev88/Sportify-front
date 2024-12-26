using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify_back.Models;

namespace Sportify_Back.Models
{
    public class ApplicationUser : IdentityUser
    {
    [Display(Name = "Nombre")]
    public string Name { get; set; }
    
    [Display(Name = "Apellido")]
    public string LastName { get; set; }
    
    [DisplayFormat(DataFormatString = "{0:##.###.###}")]
    [Range(10000000, 99999999, ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    public int DNI { get; set; }

    [Display(Name = "Document")]
    [NotMapped]
    public IFormFile? Document { get; set; }
    public string? DocumentName { get; set; }
    public byte[]? DocumentContent { get; set; }

    public int? PlansId { get; set; }
    public Plans Plans { get; set; }

    
    }
}