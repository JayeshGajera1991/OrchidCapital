FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrchidCapital/OrchidCapital.csproj", "OrchidCapital/"]
COPY ["Orchid.DataAccess/Orchid.DataAccess.csproj", "Orchid.DataAccess/"]
COPY ["Orchid.DataModels/Orchid.DataModels.csproj", "Orchid.DataModels/"]
COPY ["Orchid.EmailService/Orchid.EmailService.csproj", "Orchid.EmailService/"]
COPY ["Orchid.TokenValidator/Orchid.TokenValidator.csproj", "Orchid.TokenValidator/"]
COPY ["Orchid.UtilityHelper/Orchid.UtilityHelper.csproj", "Orchid.UtilityHelper/"]

RUN dotnet restore "OrchidCapital/OrchidCapital.csproj"

COPY . .

WORKDIR "/src/OrchidCapital"
RUN dotnet publish "OrchidCapital.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrchidCapital.dll"]