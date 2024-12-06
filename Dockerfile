# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Копіюємо проектний файл і відновлюємо залежності
COPY DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj ./DotnetApiPostgres.Api/
RUN dotnet restore ./DotnetApiPostgres.Api/DotnetApiPostgres.Api.csproj

# Копіюємо весь код
COPY DotnetApiPostgres.Api ./DotnetApiPostgres.Api

# Переходимо до каталогу з проектом
WORKDIR /src/DotnetApiPostgres.Api

# Компілюємо додаток
RUN dotnet build -c Release -o /app/build

# Встановлюємо інструмент dotnet-ef для міграцій
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
# Додати інструменти dotnet в PATH
#ENV PATH="/root/.dotnet/tools:$PATH"

# Публікуємо додаток
RUN dotnet publish -c Release -o /app/publish

# Base stage: використання образу для виконання ASP.NET Core додатку
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Копіюємо зібраний додаток з етапу build
COPY --from=build /app/publish .

# Відкриваємо порт для доступу до додатку
EXPOSE 80

# Запускаємо додаток
ENTRYPOINT ["dotnet", "DotnetApiPostgres.Api.dll"]

