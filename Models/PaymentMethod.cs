using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify_back.Models
{
public class PaymentMethod
{
    [Column("IdPaymentMethod")]
    public int Id { get; set; }
    [Display(Name = "Método De Pago")]
    public string Tipo { get; set; } 
    public string Detalle { get; set; } 

    [Required]
    public List<Payments> Payments { get; set; }
    

}
}