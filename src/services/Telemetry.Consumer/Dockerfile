FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/Telemetry.Consumer/Telemetry.Consumer.csproj", "src/services/Telemetry.Consumer/"]
RUN dotnet restore "src/services/Telemetry.Consumer/Telemetry.Consumer.csproj"
COPY . .
WORKDIR "/src/src/services/Telemetry.Consumer"
RUN dotnet build "Telemetry.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Telemetry.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Telemetry.Consumer.dll"]
