#Base image for the build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TicTacToe.Server.csproj"
RUN dotnet publish "TicTacToe.Server.csproj" -c Release -o /app/publish

# Runtime stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.Server.dll"]
