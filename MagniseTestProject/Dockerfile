# Base image for runtime (used in final stage)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MagniseTestProject/MagniseTestProject.csproj", "MagniseTestProject/"]
RUN dotnet restore "MagniseTestProject/MagniseTestProject.csproj"

# Copy the entire source
COPY . .
WORKDIR "/src/MagniseTestProject"

# Build project
RUN dotnet build "MagniseTestProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MagniseTestProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MagniseTestProject.dll"]
