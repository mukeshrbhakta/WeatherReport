FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JbHifi.WeatherReport.FrontEnd/JbHifi.WeatherReport.FrontEnd.WebUI/JbHifi.WeatherReport.FrontEnd.WebUI.csproj", "JbHifi.WeatherReport.FrontEnd.WebUI/"]
RUN dotnet restore "JbHifi.WeatherReport.FrontEnd/JbHifi.WeatherReport.FrontEnd.WebUI/JbHifi.WeatherReport.FrontEnd.WebUI.csproj"
COPY . .
WORKDIR "/src/JbHifi.WeatherReport.FrontEnd.WebUI"
RUN dotnet build "JbHifi.WeatherReport.FrontEnd.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JbHifi.WeatherReport.FrontEnd.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JbHifi.WeatherReport.FrontEnd.WebUI.dll"]
