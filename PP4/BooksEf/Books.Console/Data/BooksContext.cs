using Microsoft.EntityFrameworkCore;
using Books.Console.Models;

namespace Books.Console.Data;

public class BooksContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TitleTag> TitlesTags => Set<TitleTag>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // ./data/books.db
        var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
        Directory.CreateDirectory(dataDir);
        var dbPath = Path.Combine(dataDir, "books.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabla TitleTag se llama "TitlesTags"
        modelBuilder.Entity<TitleTag>().ToTable("TitlesTags");

        // Orden de columnas para Title: TitleId, AuthorId, TitleName
        modelBuilder.Entity<Title>(entity =>
        {
            entity.Property(t => t.TitleId).HasColumnOrder(0);
            entity.Property(t => t.AuthorId).HasColumnOrder(1);
            entity.Property(t => t.TitleName).HasColumnOrder(2);
        });

        // Relaciones 
        modelBuilder.Entity<Title>()
            .HasOne(t => t.Author)
            .WithMany(a => a.Titles)
            .HasForeignKey(t => t.AuthorId);

        modelBuilder.Entity<TitleTag>()
            .HasOne(tt => tt.Title)
            .WithMany(t => t.TitleTags)
            .HasForeignKey(tt => tt.TitleId);

        modelBuilder.Entity<TitleTag>()
            .HasOne(tt => tt.Tag)
            .WithMany(t => t.TitleTags)
            .HasForeignKey(tt => tt.TagId);
    }
}
