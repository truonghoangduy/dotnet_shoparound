## SHOPAROUND
Weird name :<, just an dotnet courses in my university campus
<div style="display:flex"></div>
<img src="./docs/web_app.jpg" style="width:45%; text-align:center">
<!-- <a href="http://www.youtube.com/watch?feature=player_embedded&v=lkmvnjypENw" target="_blank"><img src="http://img.youtube.com/vi/lkmvnjypENw/2.jpg" 
alt="Bash Into Zsh!" style="width:45%;border-color:white" /></a> -->
<!-- https://www.youtube.com/watch?v=lkmvnjypENw&t=18s -->
```dotnet core (v5) MVC Web Application```

#### Focus on theses
1. Clean Architecture
2. C# Entities(ORM) & Identities(Auth) out of the box framework
3. Docker (the cheep way)
4. dotnet core CLI, SDK tool
5. What about microservices

-----
#### ```Installation The cheap way```
If you have docker, well bellow line will be enough to bring 2 postgres database else you could change Database just by config in ```src/Web/appsettings.json``` with their corresponding database port *SQL only

```
docker-compose up
--- shoparound (main)
---- shoparound-identity (auth stuff)
```

Run migration, bump up database schema ASAP, underlining using ```dotnet ef migrations CLI flag```
```
exe(Window,Linux) ./fore_wipe_db.sh
```
---- ```#thecheapway :<```
```docker container only for database stuff``` weird right building Web app in debug mode is priceless things that dev wanted. This is the monolithic application, clean architecture approach to MVC the entry point of the application(project) in solution located at ```src/Web/Web.csproj``` other project are just code bootstrapping. 
#### ```Folder structure```
```
-- src/ApplicationCore [Entities,shared Interface,...]
-- src/Infrastructure [DBContext, Seeder,...]
-- src/Web [ASP core MVC]
```

---- ```Database seeding```
```
dev(env) automatic seeding when database null 
- config in ```src/Web/Program.cs``` 
-- located at ```src/Infrastructure/Data/ShopDBInitializer.cs
```
