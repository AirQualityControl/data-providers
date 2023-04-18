@echo off

set /p project_name=Enter project name example AirSnitch.DataProviders.EcoCity 

set "dotnet_lambda_tools="
for /f "tokens=*" %%a in ('dotnet tool list --global ^| findstr /i Amazon.Lambda.Tools') do set "dotnet_lambda_tools=%%a"

if "%dotnet_lambda_tools%"=="" (
  dotnet tool install -g Amazon.Lambda.Tools
  dotnet new --install Amazon.Lambda.Templtes
  dotnet tool install --global Amazon.Lambda.TestTool-7.0
) 
cd ..
cd src
dotnet new lambda.EmptyFunction --name %project_Name%
set /p dummy=Press any key to continue...