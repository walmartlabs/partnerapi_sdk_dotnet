FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

# copy everything else and build
COPY . ./
RUN ls -la Source
RUN dotnet build Source/Walmart.Sdk.Marketplace/Walmart.Sdk.Marketplace.csproj
RUN dotnet restore Sample/Sample.Core20.sln
RUN dotnet publish Sample/Sample.Core20.csproj -o ../out

# build runtime image
FROM microsoft/dotnet:2.0-runtime 
WORKDIR /app
COPY --from=build-env /app/out ./
RUN ls -la /app
ENTRYPOINT ["dotnet", "Walmart.Sdk.Marketplace.Sample.dll"]
