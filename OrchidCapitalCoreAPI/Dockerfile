FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj", "OrchidCapitalCoreAPI/"]
COPY ["Orchid.DataAccess/Orchid.DataAccess.csproj", "Orchid.DataAccess/"]
COPY ["Orchid.TokenValidator/Orchid.TokenValidator.csproj", "Orchid.TokenValidator/"]
COPY ["Orchid.UtilityHelper/Orchid.UtilityHelper.csproj", "Orchid.UtilityHelper/"]
COPY ["Orchid.EmailService/Orchid.EmailService.csproj", "Orchid.EmailService/"]

RUN dotnet restore "OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj" --disable-parallel

COPY . .

WORKDIR "/src/OrchidCapitalCoreAPI"

RUN dotnet publish "OrchidCapitalCoreAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    --verbosity normal