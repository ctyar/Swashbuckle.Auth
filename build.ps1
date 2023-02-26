Get-ChildItem -Path '.\artifacts' | Remove-Item -Force -Recurse

dotnet pack src\Swashbuckle.Auth\Swashbuckle.Auth.csproj -o artifacts