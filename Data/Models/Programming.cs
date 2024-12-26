using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sportify_Back.Models;

namespace Sportify_back.Models
{
    public class Programmings
    {

        [Column("IdProgramming")]
        public int Id { get; set; }

        public bool Active { get; set; }       

    }
}