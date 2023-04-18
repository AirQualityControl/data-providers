@echo off

set /p project_name=Enter project name like example AirSnitch.DataProviders.MyAwesomeProvier 

set "dotnet_lambda_tools="
for /f "tokens=*" %%a in ('dotnet tool list --global ^| findstr /i Amazon.Lambda.Tools') do set "dotnet_lambda_tools=%%a"

if "%dotnet_lambda_tools%"=="" (
  dotnet tool install -g Amazon.Lambda.Tools
  dotnet new --install Amazon.Lambda.Templtes
) 
cd ..
cd src
dotnet new lambda.EmptyFunction --name %project_Name%
set /p dummy=Press any key to continue...