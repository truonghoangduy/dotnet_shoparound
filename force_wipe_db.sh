# dotnet ef database update 0 \
dotnet ef migrations remove -c ShopDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -f \
&& dotnet ef migrations remove -c WebIdentityDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -f \
&& dotnet ef migrations add MainModel -c ShopDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations \
&& dotnet ef migrations add IndentityModel -c WebIdentityDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Identity/Data/Migrations \
&& dotnet ef database update -c ShopDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj \
&& dotnet ef database update -c WebIdentityDBContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj \