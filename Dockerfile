# Start from the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# COPY from the subfolder
COPY security-scan/security-scan.csproj ./security-scan/
WORKDIR /src/security-scan
RUN dotnet restore

COPY security-scan/. .

RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "security-scan.dll"]
