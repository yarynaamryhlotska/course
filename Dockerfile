FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Копіюємо проект
COPY DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj ./DotnetApiPostgres.Api/

# Встановлюємо всі залежності
RUN dotnet restore ./DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj

# Копіюємо весь код
COPY DotnetApiPostgres.Api ./DotnetApiPostgres.Api

WORKDIR /src/DotnetApiPostgres.Api

# Встановлюємо інструмент dotnet-ef
RUN dotnet tool install --global dotnet-ef

# Додаємо шлях до інструмента до PATH
ENV PATH=$PATH:/root/.dotnet/tools

# Будуємо проект
RUN dotnet build -c Release -o /app/build

# Публікуємо проект
RUN dotnet publish -c Release -o /app/publish

# Налаштовуємо базовий образ для контейнера
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "DotnetApiPostgres.Api.dll"]

