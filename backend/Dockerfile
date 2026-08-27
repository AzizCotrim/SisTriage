# Etapa de build — restaura e publica a aplicação
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só os .csproj primeiro para aproveitar o cache do Docker:
# enquanto as dependências não mudarem, o restore não roda de novo.
COPY SisTriage.Domain/*.csproj SisTriage.Domain/
COPY SisTriage.Application/*.csproj SisTriage.Application/
COPY SisTriage.Infrastructure/*.csproj SisTriage.Infrastructure/
COPY SisTriage.Repository/*.csproj SisTriage.Repository/
COPY SisTriage.API/*.csproj SisTriage.API/
RUN dotnet restore SisTriage.API/SisTriage.API.csproj

COPY . .
RUN dotnet publish SisTriage.API/SisTriage.API.csproj -c Release -o /app --no-restore

# Etapa final — imagem enxuta, só com o runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SisTriage.API.dll"]
