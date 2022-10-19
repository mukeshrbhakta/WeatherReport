FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JbHifi.WeatherReport.WebApi/JbHifi.WeatherReport.WebApi.csproj", "JbHifi.WeatherReport.WebApi/"]
RUN dotnet restore "JbHifi.WeatherReport.WebApi/JbHifi.WeatherReport.WebApi.csproj"
COPY . .
WORKDIR "/src/JbHifi.WeatherReport.WebApi"
RUN dotnet build "JbHifi.WeatherReport.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JbHifi.WeatherReport.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JbHifi.WeatherReport.WebApi.dll"]
