# Weather Report application
This is a Jb Hi-Fi programming challenge

## Pre-requisites
1. Visual Studio 2022/JetBrains Rider 2021 or higher (https://visualstudio.microsoft.com/vs/)
2. .NET 6.x SDK (https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 
3. Git command-line tools (optional)
4. Access to cmd.exe(Windows) or Terminal(Nix)

## Frameworks and Tools used
1. Global Error Handling - Effective way to log errors
2. HttpClientFactory - Resolves the httClient quirks when handled directly
3. Serilog - log to file implementation 
4. JMESPath - Fast and easy way to transform JSON data
5. SonarLint - extension for writing resilient code

## Installing Postgres on Docker
Install Postgres locally on Docker using the following command-line -
```
docker pull postgres
docker run --name postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres
```

## Creating the database
We use the db-first approach. Execute 'Database Scripts/Create Database.sql' to create the database using VS Code, Dbeaver or Rider

## Updating the Entity Framework
If changes are made to the db schema -
a. Execute the following lines -
```
cd JbHifi.WeatherReport.Backend/JbHifi.WeatherReport.DataLibrary
dotnet ef dbcontext scaffold "Host=localhost:5432;Database=postgres;Username=postgres;Password=<password>" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c "WeatherReportDbContext" --force
cd ../..
```
b. Remove the OnConfiguring method from Models/SchedulingDbContext.cs.  

## Building the solution 
You can build the debug version of the solution using the following commands - 
```
dotnet restore
dotnet clean
dotnet build 
```
This will build both the Web API and UI components (located in the Backend and FrontEnd folders)

## Unit testing the solution
Backend unit tests can be executed using - 
```
dotnet test 
```

## Managing API keys
API keys are generated on the fly (because of security concerns they are not stored anywhere). To generate API keys perform the following steps - 
1. Open GenerateAllApiKeys_Success test in HelperTests.cs under the JbHifi.WeatherReport.UnitTests project
2. Comment off the Ignore attribute
3. Open the weatherreportapikeys table and copy the unique id column values to lines 47-51 in HelperTests.cs respectively
4. Debug the test and note down the Api keys. 

## Running the Web API
1. Execute the following commands - 
```
cd JbHifi.WeatherReport.Backend/JbHifi.WeatherReport.WebApi 
dotnet run 
```
2. Open Postman, 
   - Import > Link > http://localhost:5042/swagger/v1/swagger.json > Import
   - APIs > JbHifi.WeatherReport.WebApi > JbHifi.WeatherReport.WebApi > Authorization 
     - Type = API Key
     - Key  = API-Key
     - Value = Paste one of the API keys from step 'Managing API keys'
   - APIs > JbHifi.WeatherReport.WebApi > JbHifi.WeatherReport.WebApi > Variables > baseUrl
     - Set both Initial and Current Value's to http://localhost:5042
   - Select the /weatherforecast/getweatherforecast API 
     - Change the Params - city = washington and country = usa
     - Execute the API

## Running the UI
**NOTE** - Make sure the Web API is running 
1. Open appsettings.Development.json in the JbHifi.WeatherReport.FrontEnd.WebUI project and paste one of the API keys
2. Execute the following commands -
```
cd JbHifi.WeatherReport.FrontEnd/JbHifi.WeatherReport.FrontEnd.WebUI 
dotnet run 
```
2. Using your web browser, open https://localhost:7111/ or http://localhost:5051
3. Change the default values from Melbourne, Australia to Mumbai, India/NY, USA etc and click Get Weather Forecast. 
4. Results will be displayed in the response edit box

## Troubleshooting
1. Error : Your hourly limit has exceeded
   This happens if you access the web service more than 5 times in an hour. The issue can be resolved as follows - 
   - Method 1 - Truncate the audit table 
   - Method 2 - Change the API key in the appsettings.Development.json file and retry by restarting the UI service
2. Postman fails to execute the APIs with 'SSL Error: Unable to verify the first certificate' error
   Make sure the SSL certificate verification flag is disabled.

    