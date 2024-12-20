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
    public class Programmings
    {

        [Column("IdProgramming")]
        public int Id { get; set; }

        [Required]
        public ICollection<Users> Users { get; set; }

        [Required]
        public ICollection<Classes> Classes { get; set; }

        public bool Active { get; set; }       

    }
}