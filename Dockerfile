# Brug base image for runtime miljøet
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Brug SDK image til at bygge projektet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuoteOTD - Service/QuoteOTD - Service.csproj", "QuoteOTD - Service/"]
RUN dotnet restore "QuoteOTD - Service/QuoteOTD - Service.csproj"

# Kopier kildekoden og byg applikationen
COPY ["QuoteOTD - Service/", "QuoteOTD - Service/"]
WORKDIR "/src/QuoteOTD - Service"
RUN dotnet build "QuoteOTD - Service.csproj" -c Release -o /app/build

# Publish applikationen
FROM build AS publish
RUN dotnet publish "QuoteOTD - Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Byg det endelige image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuoteOTD - Service.dll"]
