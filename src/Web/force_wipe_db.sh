dotnet ef database update 0 \
&& dotnet ef migrations remove \
&& dotnet ef migrations add InitialCreate -o Infrastructure -o ../Infrastructure/Migrations \
&& dotnet ef database update