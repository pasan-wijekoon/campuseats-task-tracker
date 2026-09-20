# ── Stage 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore dependencies first (layer-cached)
COPY ["CampusEatsTaskTracker.csproj", "."]
RUN dotnet restore "CampusEatsTaskTracker.csproj"

# Copy everything and publish
COPY . .
RUN dotnet publish "CampusEatsTaskTracker.csproj" -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose port 80 (internal)
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CampusEatsTaskTracker.dll"]
