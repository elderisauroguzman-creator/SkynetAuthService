# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia el archivo del proyecto
COPY *.csproj ./
RUN dotnet restore

# Copia todo el código fuente
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configura el puerto dinámico de Railway
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

# Encuentra y ejecuta el DLL (Railway lo detectará automáticamente)
ENTRYPOINT ["dotnet", "AuthService.dll"]