#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/WorkoutService/Workout.Api/Workout.Api.csproj", "Services/WorkoutService/Workout.Api/"]
COPY ["Services/WorkoutService/Workout.Infrastructure/Workout.Infrastructure.csproj", "Services/WorkoutService/Workout.Infrastructure/"]
COPY ["Services/WorkoutService/Workout.Core/Workout.Core.csproj", "Services/WorkoutService/Workout.Core/"]
RUN dotnet restore "Services/WorkoutService/Workout.Api/Workout.Api.csproj"
COPY . .
WORKDIR "/src/Services/WorkoutService/Workout.Api"
RUN dotnet build "Workout.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Workout.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Workout.Api.dll"]