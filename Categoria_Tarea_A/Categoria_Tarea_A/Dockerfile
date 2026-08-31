FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Categoria_Tarea_A.csproj", "."]
RUN dotnet restore "./Categoria_Tarea_A.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Categoria_Tarea_A.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Categoria_Tarea_A.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Categoria_Tarea_A.dll"]