# Práctica Programada 4 – Entity Framework Code First, CSV y TSV

**Estudiante:** Ricardo Patiño Jiménez  
**Carné:** FH22011118

---

## Comandos utilizados (CLI)

````bash
#crear carpeta
mkdir Models
mkdir Data

# abrir VSC desde consola
 code .

# Crear solución
dotnet new sln -n BooksEf
dotnet new console -n Books.Console
dotnet sln add .\Books.Console\Books.Console.csproj

# Crear proyecto de consola
dotnet new console -n PP4 -f net8.0

# Agregar proyecto a la solución
dotnet sln add BooksEf.sln

# Ejecutar el programa
dotnet run --project PP4

# compilar el programa
dotnet build

# correr el programa
dotnet run

# cambiamos al proyecto
 cd PP4/

# correr el proyecto
 dotnet run


---
## Instalar paquetes
dotnet add .\Books.Console\Books.Console.csproj package Microsoft.EntityFrameworkCore
dotnet add .\Books.Console\Books.Console.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add .\Books.Console\Books.Console.csproj package Microsoft.EntityFrameworkCore.Sqlite

## 4) Migracion y Base de Datos
dotnet build
dotnet ef migrations add InitialCreate
dotnet ef database update

---
# 3Crear/actualizar la base de datos SQLite
dotnet ef database update
---

## Preguntas de Reflexión

### 1 ¿Cómo resultaría el uso de Code First en una base NoSQL (p. ej., MongoDB)?

El enfoque **Code First** no se adapta fácilmente a bases de datos NoSQL, debido a que estas no poseen un esquema rígido ni claves foráneas.
En una base como **MongoDB**, los datos se almacenan como documentos JSON flexibles, y las relaciones se modelan mediante referencias o documentos embebidos, sin necesidad de definir migraciones estructurales.

- **Code First**: perdería gran parte de su utilidad, ya que la base de datos no impone una estructura estricta ni validaciones de integridad.
- **Database First**: también sería poco práctico, puesto que no hay un esquema formal del cual generar entidades.
- **Foreign Keys**: las bases NoSQL no manejan claves foráneas ni restricciones referenciales, por lo tanto, estas relaciones deben gestionarse desde la lógica de la aplicación.

En conclusión, la estrategia **Code First** es ideal para sistemas relacionales como SQLite o SQL Server, pero presenta limitaciones naturales en sistemas NoSQL, donde el esquema es flexible y las relaciones no son nativas.

---

### 2 ¿Qué otro carácter se puede usar para separar valores en archivos tabulares?

Además de la **coma (`,`)** y el **tabulador (`\t`)**, se pueden emplear otros separadores dependiendo del tipo de datos y del contexto.
Un ejemplo común es el **punto y coma (`;`)**, que se utiliza en países donde la coma se usa como separador decimal.

**Propuesta:**
- Usar el carácter **`;`** como delimitador principal.
- Asignar la extensión **`.ssv`** (*Semicolon-Separated Values*).

Otros ejemplos válidos:
- **Pipe (`|`)** → `.psv`
- **Tilde (`~`)** → `.tsv` no, porque ese ya representa el tab.
- **Dos puntos (`:`)** → `.csv2` en sistemas antiguos o específicos.

El criterio más importante al elegir un delimitador es que **no aparezca en los datos**, o que pueda escaparse adecuadamente.

---

##  Referencias

- [Microsoft Docs – Entity Framework Core Fluent API](https://learn.microsoft.com/en-us/ef/core/modeling/)
- [Microsoft Docs – System.IO Namespace](https://learn.microsoft.com/en-us/dotnet/api/system.io)
- [StackOverflow: Reading CSV with quotes in C#](https://stackoverflow.com/questions/13418412)
- **ChatGPT (OpenAI GPT-5)** – Asistente IA utilizado para la documentación y formato del proyecto. https://chatgpt.com/share/6912b3e1-d25c-800b-b123-05803f09cee1
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Microsoft Learn – Working with CSV and TSV Files](https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-and-write-to-a-text-file)



##  Estructura del Proyecto

```bash
PP4/
├── MySolution.sln
└── MyProject/
    ├── Program.cs
    ├── Data/
    │   └── BooksContext.cs
    ├── Models/
    │   ├── Author.cs
    │   ├── Title.cs
    │   ├── Tag.cs
    │   └── TitleTag.cs
    ├── data/
    │   ├── books.csv
    │   └── books.db  # (autogenerado)
    └── MyProject.csproj

````
