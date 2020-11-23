## Run the app
`dotnet watch run`  

## Scaffolding models  
Use the scaffolding tool to produce Create, Read, Update, and Delete (CRUD) pages (Views, Controllers) for a model:  
```
dotnet aspnet-codegenerator controller -name SomeModelController -m SomeModel -dc BracketsAppContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -f
```

## Database
### Development (SQLite)

appsettings.json:
```
"ConnectionStrings": {
  "DefaultConnection": "DataSource=app.db;Cache=Shared"
},
```

Startup.cs:
```
services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
```

### Production
[ASP.NET Core MVC Identity using PostgreSQL database](https://medium.com/@RobertKhou/asp-net-core-mvc-identity-using-postgresql-database-bc52255f67c4)

## Database migrations  
Generating and applying migrations
```
dotnet ef migrations add MigrationName  
dotnet ef database update <MigrationName>
```

Resetting all migrations and starting over
```
dotnet ef database update 0
dotnet ef migrations remove
```

