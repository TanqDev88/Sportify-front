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
    public class Classes
    {

        [Column("IdClasses")]
        public int Id { get; set; }

        [Required (ErrorMessage= "El nombre de la clase es obligatorio")]
        public string Name { get; set; }

        public Activities? Activities { get; set; }

        [ForeignKey("Activities")]
        public int  ActivityId { get; set; }

        [Required (ErrorMessage= "Debe indicar un día y horario de la clase")]
        public DateTime Sched { get; set; }

        public Teachers? Teachers { get; set; }

        [Display(Name ="Teachers")]
        public int  TeachersId { get; set; }

        public List<Programmings>? Programmings { get; set; } = new List<Programmings>();

        [Required(ErrorMessage= "Debe indicar el cupo máximo de la clase")]
        public int Quota { get; set; }

        public bool Active { get; set; }
        
    }
}