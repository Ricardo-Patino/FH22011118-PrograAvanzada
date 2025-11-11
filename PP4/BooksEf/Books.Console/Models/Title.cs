using System.ComponentModel.DataAnnotations;

namespace Books.Console.Models;

public class Title
{
    [Key]
    public int TitleId { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public string TitleName { get; set; } = null!;

    public Author Author { get; set; } = null!;
    public ICollection<TitleTag> TitleTags { get; set; } = new List<TitleTag>();
}
