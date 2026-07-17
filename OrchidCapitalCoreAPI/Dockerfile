FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy every project file first
COPY ["OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj", "OrchidCapitalCoreAPI/"]
COPY ["Orchid.DataAccess/Orchid.DataAccess.csproj", "Orchid.DataAccess/"]
COPY ["Orchid.DataModels/Orchid.DataModels.csproj", "Orchid.DataModels/"]
COPY ["Orchid.EmailService/Orchid.EmailService.csproj", "Orchid.EmailService/"]
COPY ["Orchid.TokenValidator/Orchid.TokenValidator.csproj", "Orchid.TokenValidator/"]
COPY ["Orchid.UtilityHelper/Orchid.UtilityHelper.csproj", "Orchid.UtilityHelper/"]

# Restore the API project and all its referenced projects
RUN dotnet restore "OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj"

# Copy the remaining source code
COPY . .

WORKDIR "/src/OrchidCapitalCoreAPI"

RUN dotnet publish "OrchidCapitalCoreAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8081

ENTRYPOINT ["dotnet", "OrchidCapitalCoreAPI.dll"]