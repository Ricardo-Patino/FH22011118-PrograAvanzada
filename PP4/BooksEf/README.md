# Práctica Programada 4 – Entity Framework Code First, CSV y TSV

**Estudiante:** Ricardo Patiño Jiménez  
**Carné:** FH22011118

---

## Comandos utilizados (CLI)

```bash
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

## 🧱 Estructura del Proyecto

```
