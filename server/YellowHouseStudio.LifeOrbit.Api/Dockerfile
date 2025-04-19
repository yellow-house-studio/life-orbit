FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["YellowHouseStudio.LifeOrbit.Api/YellowHouseStudio.LifeOrbit.Api.csproj", "YellowHouseStudio.LifeOrbit.Api/"]
COPY ["YellowHouseStudio.LifeOrbit.Application/YellowHouseStudio.LifeOrbit.Application.csproj", "YellowHouseStudio.LifeOrbit.Application/"]
COPY ["YellowHouseStudio.LifeOrbit.Domain/YellowHouseStudio.LifeOrbit.Domain.csproj", "YellowHouseStudio.LifeOrbit.Domain/"]
COPY ["YellowHouseStudio.LifeOrbit.Infrastructure/YellowHouseStudio.LifeOrbit.Infrastructure.csproj", "YellowHouseStudio.LifeOrbit.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "YellowHouseStudio.LifeOrbit.Api/YellowHouseStudio.LifeOrbit.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/YellowHouseStudio.LifeOrbit.Api"
RUN dotnet build "YellowHouseStudio.LifeOrbit.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "YellowHouseStudio.LifeOrbit.Api.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "YellowHouseStudio.LifeOrbit.Api.dll"] 