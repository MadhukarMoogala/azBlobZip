# In Memory Zipping Azure Storage Blobs

![Platforms](https://img.shields.io/badge/platform-Windows-blue.svg)
![.NET](https://img.shields.io/badge/.NET%20Core-3.1-blue.svg)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://opensource.org/licenses/MIT)

We had a requirement to serve a zip file from of selected Azure blob files from a given container to [Forge Design Automation](https://forge.autodesk.com/en/docs/design-automation/v3/developers_guide/overview/) service
Most of my searches went in vain, either they are related to ASP .NET or not working any more, this project takes inspiration from [Generate a Zip file from Azure Blob Storage Files](https://microsoft.github.io/AzureTipsAndTricks/blog/tip141.html)

This project is based on ASP .NET Core 3.1 technology, I have created a single REST endpoint `api/download` that will serve a zip file to the client, we can pass this server endpoint to Forge Design Automation services, `workitem` input URL.

## Demo

![Demo.Gif](https://github.com/MadhukarMoogala/azBlobZip/blob/master/azblobzip.gif)




## Prerequisites
- **ASP .NET Core** basic knowledge with C#
- [Azure Account](https://azure.microsoft.com/en-in/)
- Create a Storage Account and get a [Azure Connection String](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-keys-manage?tabs=azure-portal)

## Setup
 Source can be found at [Github](https://github.com/MadhukarMoogala/da-azfunc)
 Either you use set Environment Variables or through a configuration file.
 ### Environment Variables
 `AZURE_CONNECTION_STRING` :`Your Azure Connection String`
 `AZURE_CONTAINER`:`To zip the Container holding the Azure Blobs`
### Configuration File
- Add `appsettings` JSON file to the project

- Name it as `appsettingsLocal.json`

- Add the following contents

- ```json
  {
    "ConnectionStrings": {
      "AzureConnectionString": "Your Azure Connection String"
    },
    "AzureContainer": "To zip the Container holding the Azure Blobs"
  }
  ```

```bash
git clone https://github.com/MadhukarMoogala/azBlobZip.git
cd azBlobZip
dotnet restore
dotnet build
dotnet run
```



## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](https://github.com/MadhukarMoogala/design-migration/blob/master/LICENSE) file for full details.

## Written by

Madhukar Moogala, [Forge Partner Development](http://forge.autodesk.com/) @galakar


​	







