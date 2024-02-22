#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sphagnum.DebugClient/Sphagnum.DebugClient.csproj", "Sphagnum.DebugClient/"]
COPY ["Sphagnum.Client/Sphagnum.Client.csproj", "Sphagnum.Client/"]
COPY ["Sphagnum.Common/Sphagnum.Common.csproj", "Sphagnum.Common/"]
COPY ["Sphagnum.Common.Contracts/Sphagnum.Common.Contracts.csproj", "Sphagnum.Common.Contracts/"]
RUN dotnet restore "./Sphagnum.DebugClient/./Sphagnum.DebugClient.csproj"
COPY . .
WORKDIR "/src/Sphagnum.DebugClient"
RUN dotnet build "./Sphagnum.DebugClient.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sphagnum.DebugClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sphagnum.DebugClient.dll"]