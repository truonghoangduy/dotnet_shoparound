## SHOPAROUND
Weird name :<, just an dotnet courses in my university campus
<img src="./docs/web_app.jpg" style="width:50%; text-align:center">
```dotnet core (v5) MVC Web Application```

#### Focus on theses
1. Clean Architecture
2. C# Entities(ORM) & Identities(Auth) out of the box framework
3. Docker (the cheep way)
4. dotnet core CLI, SDK tool
5. What about microservices

-----
#### Installation ```The cheap way```
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
