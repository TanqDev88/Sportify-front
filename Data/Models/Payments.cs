using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using Sportify_Back.Models;

namespace Sportify_back.Models
{
    public class Payments
    {

        public int Id { get; set; }

        
        //[ForeignKey("UsersId")]
        public ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey("Users")]
        public string UsersId { get; set; } 

        [ForeignKey("PlansId")]
        public Plans? Plans { get; set; }
          [ForeignKey("Plans")]
        public int PlansId { get; set; }

        public int  PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
 
        public DateTime? Fecha { get; set; }


        [NotMapped ]
        public int CardNumber { get; set; }

        [NotMapped]
        public DateFormat ExpirationDate { get; set; }

        [NotMapped]
        public int SecurityCode { get; set; }

    }
}