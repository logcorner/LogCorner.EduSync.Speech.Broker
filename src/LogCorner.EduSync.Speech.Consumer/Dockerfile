#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LogCorner.EduSync.Speech.Consumer/LogCorner.EduSync.Speech.Consumer.csproj", "LogCorner.EduSync.Speech.Consumer/"]
COPY ["LogCorner.EduSync.Speech.ServiceBus/LogCorner.EduSync.Speech.ServiceBus.csproj", "LogCorner.EduSync.Speech.ServiceBus/"]
COPY ["LogCorner.EduSync.Speech.ElasticSearch/LogCorner.EduSync.Speech.ElasticSearch.csproj", "LogCorner.EduSync.Speech.ElasticSearch/"]
COPY ["LogCorner.EduSync.Speech.Projection/LogCorner.EduSync.Speech.Projection.csproj", "LogCorner.EduSync.Speech.Projection/"]
RUN dotnet restore "LogCorner.EduSync.Speech.Consumer/LogCorner.EduSync.Speech.Consumer.csproj"
COPY . .
WORKDIR "/src/LogCorner.EduSync.Speech.Consumer"
RUN dotnet build "LogCorner.EduSync.Speech.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogCorner.EduSync.Speech.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCorner.EduSync.Speech.Consumer.dll"]