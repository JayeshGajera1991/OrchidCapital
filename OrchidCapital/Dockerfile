ARG BUILDPLATFORM
ARG TARGETARCH
FROM mcr.microsoft.com/dotnet/aspnet:10.0.103 AS base

USER $APP_UID
WORKDIR /app
EXPOSE 4001

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0.103 AS build
# Choose Release or Debug
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy all project refs & restore
COPY ["OrchidCapital/OrchidCapital.csproj", "OrchidCapital/"]
RUN dotnet restore "./OrchidCapital/OrchidCapital.csproj"

# Copy source & build
COPY . .
WORKDIR "/src/OrchidCapital"
RUN dotnet build "./OrchidCapital.csproj" \ 
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Framework-dependent (no RID needed); UseAppHost=false keeps it portable
RUN dotnet publish "./OrchidCapital.csproj" \ 
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app

# Pull in the published DLLs (portable assemblies)
COPY --from=publish /app/publish ./

# Set environment & entrypoint
#ENV ASPNETCORE_ENVIRONMENT=UAT
ENV ASPNETCORE_URLS="http://+:4001"

ENTRYPOINT ["dotnet", "OrchidCapital.dll"]