$ErrorActionPreference = 'Stop'
# Verify: repeatable health check (assumes deps already installed)
dotnet build --configuration Release
dotnet test --configuration Release --no-build
