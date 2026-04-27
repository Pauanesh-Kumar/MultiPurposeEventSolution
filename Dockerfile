# Root Dockerfile for solution - builds and publishes App.API
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only project files first for layer caching
COPY ["App.API/App.API.csproj", "App.API/"]
COPY ["App.Application/App.Application.csproj", "App.Application/"]
COPY ["App.Infrastructure/App.Infrastructure.csproj", "App.Infrastructure/"]
COPY ["App.Domain/App.Domain.csproj", "App.Domain/"]

# Restore for the API project (restores referenced projects too)
RUN dotnet restore "App.API/App.API.csproj"

# Copy everything and build/publish
COPY . .
WORKDIR /src/App.API
RUN dotnet build "App.API.csproj" -c Release -o /app/build
RUN dotnet publish "App.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]