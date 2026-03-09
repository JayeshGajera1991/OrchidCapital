# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0.103 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 3001


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0.103 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj", "OrchidCapitalCoreAPI/"]
COPY ["Orchid.DataAccess.csproj", "Orchid.DataAccess/"]
COPY ["Orchid.DataModels.csproj", "Orchid.DataModels/"]
# COPY ["Orchid.EmailService.csproj", "Orchid.EmailService/"]
COPY ["Orchid.TokenValidator.csproj", "Orchid.TokenValidator/"]
COPY ["Orchid.UtilityHelper.csproj", "Orchid.UtilityHelper/"]
RUN dotnet restore "./OrchidCapitalCoreAPI/OrchidCapitalCoreAPI.csproj"
COPY . .
WORKDIR "/src/OrchidCapitalCoreAPI"
RUN dotnet build "./OrchidCapitalCoreAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrchidCapitalCoreAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENV ASPNETCORE_ENVIRONMENT=UAT
ENV ASPNETCORE_URLS="http://+:3001"
ENTRYPOINT ["dotnet", "OrchidCapitalCoreAPI.dll"]