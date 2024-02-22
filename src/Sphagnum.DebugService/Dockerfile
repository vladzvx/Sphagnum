#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sphagnum.DebugService/Sphagnum.DebugService.csproj", "Sphagnum.DebugService/"]
COPY ["Sphagnum.Server/Sphagnum.Server.csproj", "Sphagnum.Server/"]
COPY ["Sphagnum.Common/Sphagnum.Common.csproj", "Sphagnum.Common/"]
COPY ["Sphagnum.Common.Contracts/Sphagnum.Common.Contracts.csproj", "Sphagnum.Common.Contracts/"]
RUN dotnet restore "./Sphagnum.DebugService/./Sphagnum.DebugService.csproj"
COPY . .
WORKDIR "/src/Sphagnum.DebugService"
RUN dotnet build "./Sphagnum.DebugService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sphagnum.DebugService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sphagnum.DebugService.dll"]