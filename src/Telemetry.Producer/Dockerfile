FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Telemetry.Producer/Telemetry.Producer.csproj", "src/Telemetry.Producer/"]
RUN echo "Restaurando projeto..."
RUN dotnet restore "src/Telemetry.Producer/Telemetry.Producer.csproj"
COPY . .
WORKDIR "/src/src/Telemetry.Producer"
RUN echo "Compilando projeto..."
RUN dotnet build "Telemetry.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Telemetry.Producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Telemetry.Producer.dll"]