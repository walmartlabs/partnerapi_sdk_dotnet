Pushd %~dp0..\Source

dotnet restore Walmart.Sdk.Base.Tests\Walmart.Sdk.Base.Tests.csproj
dotnet test -l trx -c Release Walmart.Sdk.Base.Tests\Walmart.Sdk.Base.Tests.csproj 

dotnet restore Walmart.Sdk.Marketplace.IntegrationTests\Walmart.Sdk.Marketplace.IntegrationTests.csproj
dotnet test -l trx -c Release Walmart.Sdk.Marketplace.IntegrationTests\Walmart.Sdk.Marketplace.IntegrationTests.csproj

popd
