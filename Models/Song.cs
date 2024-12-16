#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace database.Models;
public class Song
{
    [Key]
    public int SongId { get; set; }
    [Required]  
    public string Title { get; set; } 
    [Required]
    [Range(0,2024)]    
    public int Year { get; set; }
    [Required]  
    public string Genre { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
                
