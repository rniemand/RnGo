FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src

COPY "src/RnGo/RnGo.csproj" "RnGo/"
COPY "src/RnGo.Core/RnGo.Core.csproj" "RnGo.Core/"

RUN dotnet restore "RnGo/RnGo.csproj"

COPY "src/RnGo/" "RnGo/"
COPY "src/RnGo.Core/" "RnGo.Core/"

WORKDIR "/src/RnGo"
RUN dotnet build "RnGo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RnGo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RnGo.dll"]