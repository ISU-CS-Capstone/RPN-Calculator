#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

# Added this dockerfile in order to connect the blazor project to an actual website.
# Brek Cranney

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RPNCalculator.Web/RPNCalculator.Web.csproj", "RPNCalculator.Web/"]
COPY ["RPNCalculator.Common/RPNCalculator.Common.csproj", "RPNCalculator.Common/"]
RUN dotnet restore "./RPNCalculator.Web/RPNCalculator.Web.csproj"
COPY . .
WORKDIR "/src/RPNCalculator.Web"
RUN dotnet build "./RPNCalculator.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RPNCalculator.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RPNCalculator.Web.dll"]