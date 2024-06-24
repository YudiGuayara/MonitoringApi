FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5009

ENV ASPNETCORE_URLS=http://+:5009

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MonitoringApi/MonitoringApi.csproj", "MonitoringApi/"]
RUN dotnet restore "MonitoringApi\MonitoringApi.csproj"
COPY . .
WORKDIR "/src/MonitoringApi"
RUN dotnet build "MonitoringApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MonitoringApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitoringApi.dll"]
