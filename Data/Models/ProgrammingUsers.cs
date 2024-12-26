using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sportify_back.Models;
using Sportify_Back.Models;

public class ProgrammingUsers
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    [Required]
    public int ClassId { get; set; }

    [ForeignKey("ClassId")]
    public Classes? Class { get; set; }

    [Required]
    public DateTime InscriptionDate { get; set; } = DateTime.Now;
}
