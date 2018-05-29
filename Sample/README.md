# Sample application for SDK Library

The purpose of this application is to show example of application code that is using Walmart Marketplace API.
In addition, you can use this application as a standalone tool to interact with API.

## Prerequisites

* If you are using VS 2015 you should use Sample.Net46.sln
* In case of VS 2017 please use Sample.Core20.sln
* You can also build this app with `dotnet` with the following command `dotnet build Sample.Core20.sln`

## Usage

To run this application you need to create credentials file with your information. Create `settings`, it should have a credentials.json file with a following format:

```json
{
  "ConsumerId": "<consumer-id>",
  "PrivateKey": "<creds>"
}
```

Moreover, you can put override default settings for the application. Just create settings.json in `settings` folder. The following keys are available for override:

```json
{
  "ServiceName": "<your-value>",
  "ChannelType": "<your-value>",
  "BaseUrl": "<your-value>",
}
```

you can see default values [here](defaultSettings.json)