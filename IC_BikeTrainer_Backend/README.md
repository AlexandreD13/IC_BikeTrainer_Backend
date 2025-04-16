# IC Bike Trainer

---

## SQLite Database

Add migration:

```bash
$ dotnet ef migrations add InitialMigration --project .\IC_BikeTrainer_Backend.csproj
```

Apply migration:

```bash
$ dotnet ef database update --project .\IC_BikeTrainer_Backend.csproj
```
