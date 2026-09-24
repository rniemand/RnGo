# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
# Self-contained, fully trimmed build for Alpine (musl). Trimming roots are configured in RnGo.csproj.
ARG PUBLISH_PROPS="-r linux-musl-x64 -p:SelfContained=true \
  -p:PublishTrimmed=true -p:TrimMode=full \
  -p:InvariantGlobalization=true \
  -p:DebugType=none \
  -p:DebuggerSupport=false \
  -p:EventSourceSupport=false \
  -p:MetadataUpdaterSupport=false"
WORKDIR /src

COPY ["src/RnGo/RnGo.csproj", "RnGo/"]
COPY ["src/RnGo.Core/RnGo.Core.csproj", "RnGo.Core/"]
RUN dotnet restore "./RnGo/RnGo.csproj" $PUBLISH_PROPS

COPY "/src/RnGo/" "/src/RnGo/"
COPY "/src/RnGo.Core/" "/src/RnGo.Core/"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
WORKDIR "/src/RnGo"
RUN dotnet publish "./RnGo.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore $PUBLISH_PROPS
# Drop native diagnostics components (debugger, crash dumps, LTTng tracing, alternate GCs) that are never loaded at runtime
RUN cd /app/publish && rm -f createdump libmscordaccore.so libmscordbi.so libcoreclrtraceptprovider.so libclrgc.so libclrgcexp.so

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["./RnGo"]
