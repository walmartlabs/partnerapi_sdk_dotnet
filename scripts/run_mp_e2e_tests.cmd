Pushd %~dp0..\Source\Walmart.Sdk.Marketplace.E2ETests

dotnet restore Walmart.Sdk.Marketplace.E2ETests.csproj
dotnet test -l trx -c Release Walmart.Sdk.Marketplace.E2ETests.csproj

popd
