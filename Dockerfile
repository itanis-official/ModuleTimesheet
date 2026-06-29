FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Build args for GitHub Packages auth (passed at build time, never stored in image)
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN

# 1. Copy project file and nuget.config
COPY ModuleHelpDesk-Timesheet/Modulehelpdesktimesheet.csproj ModuleHelpDesk-Timesheet/
COPY ModuleHelpDesk-Timesheet/nuget.config ModuleHelpDesk-Timesheet/

# 2. Restore — inject credentials inline, never baked into image layers
RUN dotnet nuget add source "https://nuget.pkg.github.com/itanis-official/index.json" \
    --name github-itanis \
    --username "${GITHUB_USERNAME}" \
    --password "${GITHUB_TOKEN}" \
    --store-password-in-clear-text \
    --configfile ModuleHelpDesk-Timesheet/nuget.config \
    || true

RUN dotnet restore ModuleHelpDesk-Timesheet/Modulehelpdesktimesheet.csproj \
    --configfile ModuleHelpDesk-Timesheet/nuget.config

# 3. Copy full source
COPY ModuleHelpDesk-Timesheet/ ModuleHelpDesk-Timesheet/

# 4. Publish
WORKDIR /src/ModuleHelpDesk-Timesheet
RUN dotnet publish -c Release -o /app/out

# --- Final stage (no credentials, no SDK) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Modulehelpdesktimesheet.dll"]
