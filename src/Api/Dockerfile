FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

COPY Ecommerce.sln Ecommerce.sln
COPY src/Api/Api.csproj src/Api/Api.csproj
COPY src/Lib/Lib.csproj src/Lib/Lib.csproj


RUN dotnet restore Ecommerce.sln

COPY . .
WORKDIR /app/src/Api
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "Api.dll" ]