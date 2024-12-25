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
        [Key]
        [Column("IdClasses")]
        public int Id { get; set; }

        [Required (ErrorMessage= "El nombre de la clase es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [ForeignKey("Activities")]
        [Display(Name = "Actividades")]
        public int  ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        [Display(Name = "Actividades")]
        public Activities? Activities { get; set; }

        [Required (ErrorMessage= "Debe indicar un día y horario de la clase")]
        [Display(Name = "Fecha")]
        public DateTime Sched { get; set; }

        [Display(Name = "Profesores")]
        public Teachers? Teachers { get; set; }

        [Display(Name ="Profesores")]
        [ForeignKey("TeachersId")]
        public int  TeachersId { get; set; }

        public List<Programmings>? Programmings { get; set; } = new List<Programmings>();

        [Required(ErrorMessage= "Debe indicar el cupo máximo de la clase")]

        [Display(Name = "Cupo")]
        public int Quota { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }
        
    }
}