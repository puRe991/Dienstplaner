FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["ShiftPilot.sln", "./"]
COPY ["src/ShiftPilot.API/ShiftPilot.API.csproj", "src/ShiftPilot.API/"]
COPY ["src/ShiftPilot.Core/ShiftPilot.Core.csproj", "src/ShiftPilot.Core/"]
COPY ["src/ShiftPilot.Data/ShiftPilot.Data.csproj", "src/ShiftPilot.Data/"]

RUN dotnet restore "ShiftPilot.sln"

# Copy all source code
COPY . .

WORKDIR "/src/src/ShiftPilot.API"
RUN dotnet build "ShiftPilot.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShiftPilot.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShiftPilot.API.dll"]
