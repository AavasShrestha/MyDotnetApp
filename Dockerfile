# 1. Use SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copy the solution file
COPY SampleApplication.sln ./

# 3. Copy all project files
COPY Sample.API/*.csproj ./Sample.API/
COPY Sample.Data/*.csproj ./Sample.Data/
COPY Sample.Repository/*.csproj ./Sample.Repository/
COPY Sample.Service/*.csproj ./Sample.Service/

# 4. Restore dependencies
RUN dotnet restore

# 5. Copy everything else
COPY . ./

# 6. Build the solution
RUN dotnet publish -c Release -o /app/publish

# 7. Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5114
ENTRYPOINT ["dotnet", "Sample.API.dll"]
