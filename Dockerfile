FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GymAPI/GymAPI.csproj", "GymAPI/"]
RUN dotnet restore "GymAPI/GymAPI.csproj"
COPY . .
WORKDIR "/src/GymAPI"
RUN dotnet build "GymAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GymAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GymAPI.dll"]