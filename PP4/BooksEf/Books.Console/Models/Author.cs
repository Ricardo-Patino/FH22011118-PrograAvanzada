using System.ComponentModel.DataAnnotations;

namespace Books.Console.Models;

public class Author
{
    [Key]
    public int AuthorId { get; set; }

    [Required]
    public string AuthorName { get; set; } = null!;

    public ICollection<Title> Titles { get; set; } = new List<Title>();
}
