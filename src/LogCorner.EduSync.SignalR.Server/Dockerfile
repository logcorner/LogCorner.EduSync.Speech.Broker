#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["LogCorner.EduSync.SignalR.Server/LogCorner.EduSync.SignalR.Server.csproj", "LogCorner.EduSync.SignalR.Server/"]
COPY ["LogCorner.EduSync.SignalR.Common/LogCorner.EduSync.SignalR.Common.csproj", "LogCorner.EduSync.SignalR.Common/"]
RUN dotnet restore "LogCorner.EduSync.SignalR.Server/LogCorner.EduSync.SignalR.Server.csproj"
COPY . .
WORKDIR "/src/LogCorner.EduSync.SignalR.Server"
RUN dotnet build "LogCorner.EduSync.SignalR.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogCorner.EduSync.SignalR.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCorner.EduSync.SignalR.Server.dll"]
