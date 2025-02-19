# Imagen base para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
COPY *.sln ./
COPY SistemaApoyo.IOC/*.csproj SistemaApoyo.IOC/
COPY SistemaApoyo.BLL/*.csproj SistemaApoyo.BLL/
COPY SistemaApoyo.DAL/*.csproj SistemaApoyo.DAL/
COPY SistemaApoyo.DTO/*.csproj SistemaApoyo.DTO/
COPY SistemaApoyo.Model/*.csproj SistemaApoyo.Model/
COPY SistemaApoyo.Utility/*.csproj SistemaApoyo.Utility/
COPY WebApiApoyo/*.csproj WebApiApoyo/

RUN dotnet restore

# Copiar todo el código fuente
COPY . ./

# Compilar la aplicación
WORKDIR /app/WebApiApoyo
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish ./

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:5228
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5228

ENTRYPOINT ["dotnet", "WebApiApoyo.dll"]