#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LogCorner.EduSync.Speech.Projection.WorkerService/LogCorner.EduSync.Speech.Projection.WorkerService.csproj", "LogCorner.EduSync.Speech.Projection.WorkerService/"]
COPY ["LogCorner.EduSync.Speech.Consumer/LogCorner.EduSync.Speech.Consumer.csproj", "LogCorner.EduSync.Speech.Consumer/"]
COPY ["LogCorner.EduSync.Speech.Resiliency/LogCorner.EduSync.Speech.Resiliency.csproj", "LogCorner.EduSync.Speech.Resiliency/"]
COPY ["LogCorner.EduSync.Speech.ServiceBus/LogCorner.EduSync.Speech.ServiceBus.csproj", "LogCorner.EduSync.Speech.ServiceBus/"]
COPY ["LogCorner.EduSync.Speech.ElasticSearch/LogCorner.EduSync.Speech.ElasticSearch.csproj", "LogCorner.EduSync.Speech.ElasticSearch/"]
COPY ["LogCorner.EduSync.Speech.Projection/LogCorner.EduSync.Speech.Repository.csproj", "LogCorner.EduSync.Speech.Projection/"]
COPY ["LogCorner.EduSync.Speech.Telemetry/LogCorner.EduSync.Speech.Telemetry.csproj", "LogCorner.EduSync.Speech.Telemetry/"]
RUN dotnet restore "./LogCorner.EduSync.Speech.Projection.WorkerService/./LogCorner.EduSync.Speech.Projection.WorkerService.csproj"
COPY . .
WORKDIR "/src/LogCorner.EduSync.Speech.Projection.WorkerService"
RUN dotnet build "./LogCorner.EduSync.Speech.Projection.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LogCorner.EduSync.Speech.Projection.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCorner.EduSync.Speech.Projection.WorkerService.dll"]