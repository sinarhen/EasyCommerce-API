FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Ecommerce.sln Ecommerce.sln
COPY src/Api/Api.csporj src/Api/Api.csproj
COPY src/Lib/Lib.csproj src/Lib/Lib.csproj


RUN dotnet restore ECommerce.sln

COPY src/Api src/Api
COPY src/Lib src/Lib
WORKDIR /app/src/Api
RUN dotnet publish -c Release -o /app/src/out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.dll"]
