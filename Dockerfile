# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY Eventix.csproj ./
RUN dotnet restore "./Eventix.csproj"

# Copy all source code
COPY . ./

# Publish the app (release configuration) to /app/publish
RUN dotnet publish "Eventix.csproj" -c Release -o /app/publish

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Eventix.dll"]
