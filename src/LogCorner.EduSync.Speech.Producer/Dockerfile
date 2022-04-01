#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LogCorner.EduSync.Speech.Producer/LogCorner.EduSync.Speech.Producer.csproj", "LogCorner.EduSync.Speech.Producer/"]
COPY ["LogCorner.EduSync.Speech.ServiceBus/LogCorner.EduSync.Speech.ServiceBus.csproj", "LogCorner.EduSync.Speech.ServiceBus/"]
COPY ["LogCorner.EduSync.Speech.ElasticSearch/LogCorner.EduSync.Speech.ElasticSearch.csproj", "LogCorner.EduSync.Speech.ElasticSearch/"]
COPY ["LogCorner.EduSync.Speech.Projection/LogCorner.EduSync.Speech.Projection.csproj", "LogCorner.EduSync.Speech.Projection/"]
RUN dotnet restore "LogCorner.EduSync.Speech.Producer/LogCorner.EduSync.Speech.Producer.csproj"
COPY . .
WORKDIR "/src/LogCorner.EduSync.Speech.Producer"
RUN dotnet build "LogCorner.EduSync.Speech.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogCorner.EduSync.Speech.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCorner.EduSync.Speech.Producer.dll"]