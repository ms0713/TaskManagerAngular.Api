using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAngular.Api.Models;

public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }

    [DisplayFormat(DataFormatString = "d/M/yyyy")]
    public DateTime DateOfStart { get; set; }
    public int TeamSize { get; set; }

    //private Project() { }

    public Project() { }
}
