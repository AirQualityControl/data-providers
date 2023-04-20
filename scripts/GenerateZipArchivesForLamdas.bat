@echo off
for %%F in (AirSnitch.DataProviders.EcoCity, AirSnitch.DataProviders.Lun, AirSnitch.DataProviders.SaveDnipro) do (       
  @echo off                                                 
  cd ..
  cd .\src\%%F\src\%%F
  dotnet restore
  dotnet build --configuration Release --output build_output
  cd build_output
  powershell.exe -NoProfile -Command "Compress-Archive -Path * -DestinationPath ..\%%F.zip"
  cd ..
  move /Y %%F.zip ..\..\..\..\scripts
  rmdir build_output /s /q 
  cd ..\..\..\..\scripts
  echo The archive %%F.zip has been generated.
)