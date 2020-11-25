# BracketzApp
63180149 Nik Klavžar  
63180064 Matic Cavalleri

Trenutno na trgu ne obstaja nobena preprosta in dobra aplikacija za ustvarjanje ekip, turnirjev in turnirskih dreves. V trenutnih mračnih časih se velikokrat znajdemo zvečer doma in se želimo s prijatelji pomeriti v naši najljubši spletni igri, kot je na primer šah. Ampak, ker igro lahko igrata samo dve osebi hkrati, vi pa bi si želeli tekmovati z vsemi, ste primorani sami določiti kdo najprej igra s kom, in kako bo potekal vaš mini turnirček. To vam vzame čas, poleg tega pa nekateri od vas ne bodo zadovoljni s svojimi tekmeci.
Lahko ste s prijatelji na igrišču, kjer želite igrati košarko, ampak ne veste kako bi se razdelili v dve ekipi. Ko bi le imeli aplikacijo, ki bi glede na informacije o kvaliteti igralcev to storila za vas.

-----
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

