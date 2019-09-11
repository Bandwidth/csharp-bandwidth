@echo off

set /p NUGET_API_KEY="Nuget API key:  "

dotnet pack -c Release
dotnet nuget push src\Bandwidth.Net\bin\Release\*.nupkg -s https://api.nuget.org/v3/index.json -k %NUGET_API_KEY%