FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy project files
COPY ModuleHelpDesk-Timesheet/Modulehelpdesktimesheet.csproj ModuleHelpDesk-Timesheet/
COPY Contrat-d-evenement/ITANIS.SharedEvents.csproj Contrat-d-evenement/

# 2. Restore
RUN dotnet restore ModuleHelpDesk-Timesheet/Modulehelpdesktimesheet.csproj

# 3. Copy source code (specific folders, not COPY . .)
COPY ModuleHelpDesk-Timesheet/ ModuleHelpDesk-Timesheet/
COPY Contrat-d-evenement/ Contrat-d-evenement/

# 4. Publish
WORKDIR /src/ModuleHelpDesk-Timesheet
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Modulehelpdesktimesheet.dll"]