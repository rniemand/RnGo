#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RnGo/RnGo.csproj", "RnGo/"]
RUN dotnet restore "RnGo/RnGo.csproj"
COPY . .
WORKDIR "/src/RnGo"
RUN dotnet build "RnGo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RnGo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RnGo.dll"]