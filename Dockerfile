FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Копіюємо проектний файл для відновлення залежностей
COPY DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj ./DotnetApiPostgres.Api/

RUN dotnet restore ./DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj

# Копіюємо весь код програми
COPY DotnetApiPostgres.Api ./DotnetApiPostgres.Api

WORKDIR /src/DotnetApiPostgres.Api

RUN dotnet build -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 80
ENTRYPOINT ["dotnet", "DotnetApiPostgres.Api.dll"]

