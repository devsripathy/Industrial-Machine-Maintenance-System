FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/Domain/SentinelOps.Domain.csproj", "src/Domain/"]
COPY ["src/Application/SentinelOps.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/SentinelOps.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Presentation/SentinelOps.Presentation.csproj", "src/Presentation/"]
COPY ["src/Shared/SentinelOps.Shared.csproj", "src/Shared/"]
RUN dotnet restore "src/Presentation/SentinelOps.Presentation.csproj"

COPY . .
RUN dotnet publish "src/Presentation/SentinelOps.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SentinelOps.Presentation.dll"]
