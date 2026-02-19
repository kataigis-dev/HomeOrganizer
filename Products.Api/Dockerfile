# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["Products.Api/Products.Api.csproj", "Products.Api/"]
COPY ["Products.Application/Products.Application.csproj", "Products.Application/"]
COPY ["Products.Domain/Products.Domain.csproj", "Products.Domain/"]
COPY ["Products.Infrastructure/Products.Infrastructure.csproj", "Products.Infrastructure/"]
COPY ["Helpers/Helpers.Web/Helpers.Web.csproj", "Helpers/Helpers.Web/"]
COPY ["Helpers/Helpers.Common/Helpers.Common.csproj", "Helpers/Helpers.Common/"]
COPY ["Helpers/Helpers.SourceGenerator/Helpers.SourceGenerator.csproj", "Helpers/Helpers.SourceGenerator/"]

# Restore dependencies
RUN dotnet restore "Products.Api/Products.Api.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/Products.Api"
RUN dotnet build "Products.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Products.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Products.Api.dll"]
