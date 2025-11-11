using Microsoft.EntityFrameworkCore;
using Books.Console.Data;
using Books.Console.Models;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

using var db = new BooksContext();

// Se confirma que la base de datos ya tiene datos-autores
var hasData = await db.Authors.AnyAsync();

if (!hasData)
{
    Console.WriteLine("La base de datos se está leyendo para crear los datos del archivo CSV.");
    await LoadFromCsvAsync(db);
    Console.WriteLine("Base de datos lista.");
}
else
{
    Console.WriteLine("La base de datos se está leyendo para crear los archivos TSV.");
    await ExportToTsvAsync(db);
    Console.WriteLine("Base de datos lista.");
}

static async Task LoadFromCsvAsync(BooksContext db)
{
    var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
    var csvPath = Path.Combine(dataDir, "books.csv");

    if (!File.Exists(csvPath))
    {
        Console.WriteLine("No hay resultados para su búsqueda. No se encontró el archivo CSV en ./data/books.csv");
        return;
    }

    var lines = await File.ReadAllLinesAsync(csvPath);

    // encabezado: Author,Title,Tags
    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrWhiteSpace(line)) continue;

        string author;
        string title;
        string tagsPart;

        // autor entre comillas: "Apellido, Nombre", Titulo, Tags
        if (line.StartsWith("\""))
        {
            var closingQuote = line.IndexOf('"', 1);
            author = line.Substring(1, closingQuote - 1);

            // brinca la ","
            var rest = line.Substring(closingQuote + 2);
            var firstComma = rest.IndexOf(',');
            title = rest.Substring(0, firstComma);
            tagsPart = rest.Substring(firstComma + 1);
        }
        else
        {
            // caso sencillo: Autor, Titulo, Tags
            var firstComma = line.IndexOf(',');
            author = line.Substring(0, firstComma);

            var rest = line.Substring(firstComma + 1);
            var secondComma = rest.IndexOf(',');
            title = rest.Substring(0, secondComma);
            tagsPart = rest.Substring(secondComma + 1);
        }

        // caso si  no existe el autor, se crea
        var authorEntity = await db.Authors.FirstOrDefaultAsync(a => a.AuthorName == author);
        if (authorEntity == null)
        {
            authorEntity = new Author { AuthorName = author };
            db.Authors.Add(authorEntity);
            await db.SaveChangesAsync();
        }

        // 2️ título
        var titleEntity = new Title
        {
            AuthorId = authorEntity.AuthorId,
            TitleName = title
        };
        db.Titles.Add(titleEntity);
        await db.SaveChangesAsync();

        // 3️ tags separados por barra vertical: |
        var tagNames = tagsPart.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var tagName in tagNames)
        {
            var tagEntity = await db.Tags.FirstOrDefaultAsync(t => t.TagName == tagName);
            if (tagEntity == null)
            {
                tagEntity = new Tag { TagName = tagName };
                db.Tags.Add(tagEntity);
                await db.SaveChangesAsync();
            }

            db.TitlesTags.Add(new TitleTag
            {
                TitleId = titleEntity.TitleId,
                TagId = tagEntity.TagId
            });
            await db.SaveChangesAsync();
        }
    }
}

static async Task ExportToTsvAsync(BooksContext db)
{
    var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
    Directory.CreateDirectory(dataDir);

    // Se unen 4 tablas
    var rows =
        from a in db.Authors
        from t in a.Titles
        from tt in t.TitleTags
        join tg in db.Tags on tt.TagId equals tg.TagId
        select new
        {
            a.AuthorName,
            t.TitleName,
            tg.TagName
        };

    var list = await rows.ToListAsync();

    // Se agrupa por la primera letra del autor
    var groups = list.GroupBy(r => r.AuthorName[0]);

    foreach (var g in groups)
    {
        var fileName = $"{g.Key}.tsv";
        var filePath = Path.Combine(dataDir, fileName);

        // Se ordena de forma descendente por Autor > Título > Tag
        var ordered = g
            .OrderByDescending(x => x.AuthorName)
            .ThenByDescending(x => x.TitleName)
            .ThenByDescending(x => x.TagName);

        var sb = new StringBuilder();
        sb.AppendLine("AuthorName\tTitleName\tTagName");

        foreach (var item in ordered)
        {
            sb.AppendLine($"{item.AuthorName}\t{item.TitleName}\t{item.TagName}");
        }

        await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
    }
}

