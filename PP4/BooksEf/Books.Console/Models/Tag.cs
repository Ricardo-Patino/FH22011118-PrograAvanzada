using System.ComponentModel.DataAnnotations;

namespace Books.Console.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required]
    public string TagName { get; set; } = null!;

    public ICollection<TitleTag> TitleTags { get; set; } = new List<TitleTag>();
}
