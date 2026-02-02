# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY src/ULearn.Api/*.csproj ./src/ULearn.Api/
COPY src/ULearn.Application/*.csproj ./src/ULearn.Application/
COPY src/ULearn.Domain/*.csproj ./src/ULearn.Domain/
COPY src/ULearn.Infrastructure/*.csproj ./src/ULearn.Infrastructure/
COPY tests/ULearn.UnitTests/*.csproj ./tests/ULearn.UnitTests/


RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish the API project
WORKDIR /app/src/ULearn.Api
RUN dotnet publish -c Release -o /app/out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the API port
EXPOSE 8080
EXPOSE 8081

# Set the entry point
ENTRYPOINT ["dotnet", "ULearn.Api.dll"]
