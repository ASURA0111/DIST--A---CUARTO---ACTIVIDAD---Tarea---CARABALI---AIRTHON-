# 1. Imagen base para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# 2. Imagen del SDK para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos el archivo de proyecto y restauramos dependencias
COPY ["OAuthJWT.csproj", "."]
RUN dotnet restore "./OAuthJWT.csproj"

# Copiamos el resto del código y construimos
COPY . .
WORKDIR "/src/."
RUN dotnet build "./OAuthJWT.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publicamos la aplicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OAuthJWT.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Construimos la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OAuthJWT.dll"]