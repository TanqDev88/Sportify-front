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
    public class Activities
    {

        [Column("IdActivity")]
        public int Id { get; set; }

        [Required]
        public string NameActivity { get; set; }

        [Required]
        public string Description { get; set; }

        public List<Plans> Plans { get; set; } = new List<Plans>();

        public List<Teachers> Teachers { get; set; } = new List<Teachers>();

        public List<Classes> Classes { get; set; } = new List<Classes>();


        public bool Active { get; set; }
    }
}