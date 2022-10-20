# Weather Report application
This is a Jb Hi-Fi programming challenge

## Pre-requisites
1. Visual Studio 2022/JetBrains Rider 2021 or higher (https://visualstudio.microsoft.com/vs/)
2. .NET 6.x SDK (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
3. Docker (https://www.docker.com/products/docker-desktop)

## Installing Postgres on Docker
Install Postgres locally on Docker using the following command-line -
```
docker pull postgres
docker run --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres
```

## Creating the database
1. We use the db-first approach. Execute 'Database Scripts/Create Database.sql' to create the database -
2. If changes are made to the db schema -
    a. Execute the following lines -
    ```
    cd JbHifi.WeatherReport.Backend/JbHifi.WeatherReport.DataLibrary
    dotnet ef dbcontext scaffold "Host=localhost:5432;Database=postgres;Username=postgres;Password=<password>" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c "WeatherReportDbContext" --force
    cd ../..
    ```
    b. Remove the OnConfiguring method from Models/SchedulingDbContext.cs
