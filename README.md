## Run the app
`dotnet watch run`  

## Scaffolding models  
Use the scaffolding tool to produce Create, Read, Update, and Delete (CRUD) pages for a model:  
`dotnet aspnet-codegenerator controller -name SomeModelController -m SomeModel -dc BracketsAppContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries`

## Database
### Development

appsettings.json:
```
"ConnectionStrings": {
    "BracketsAppContext": "Data Source=BracketsApp.db"
}
```

### Production
[ASP.NET Core MVC Identity using PostgreSQL database](https://medium.com/@RobertKhou/asp-net-core-mvc-identity-using-postgresql-database-bc52255f67c4)

## Database migrations  

`dotnet ef migrations add MigrationName`  
`dotnet ef database update`  