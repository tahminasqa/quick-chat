# QuickChat

This application is a web-based communicator where registered users can seamlessly connect with individuals one-on-one, engage in vibrant group discussions, and dive into the intrigue of anonymous conversations. It also features real-time notifications during one-on-one communication, message analytics via the admin dashboard, and profile management.

# Quick Chat Reference Application architecture diagram
![Quick Chat Reference Application architecture diagram](https://github.com/tahminasqa/quick-chat/assets/55849117/bf320570-1ca7-4cfb-bf96-81b1068af3ab)

# Dashboard
![Dashboard](https://github.com/tahminasqa/quick-chat/assets/55849117/15dceb27-ddf7-446e-ab7f-d0a11c9c7198)

# Application walkthrough
![](https://media.giphy.com/media/v1.Y2lkPTc5MGI3NjExdHZkZjN1b25yZTVyZHVqcGc5ejBkaHRlZTVua2NtZzFha2F6ajZtNyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/EreWuSuqxuNxidgUV7/giphy.gif)

# Global Access: [Quick Chat](http://20.63.115.250:8090/)
> [!WARNING]
> Notes: Global access might be unreachable because of Azure subscription expiration

## Getting Started

### Prerequisites
- Clone the Quick Chat repository: https://github.com/tahminasqa/quick-chat
- (Windows only) [Install Visual Studio 2022](https://visualstudio.microsoft.com/downloads/).
  - During installation, ensure that the following are selected:
    - `ASP.NET and web development` workload.
- Install the [.NET 5 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)

- Install & start Docker Desktop:  https://docs.docker.com/engine/install/
### Running the solution

> [!WARNING]
> Remember to ensure that Docker is started

* (Windows only) Run the application from Visual Studio:
 - After cloning the project and open the `Quick.Chat.sln` file in Visual Studio
 - Change database configuration from `application.json` file
 - Execute `update-database` in the Package Manager Console, make sure you have selected `Quick.Chat.Server` as a default project
 - If everything went well, select `Quick.Chat.Server` project as a Startup
 - Build solution and run the application 

then look for lines like this in the console output in order to find the URL to open the QuickChat login :
```sh
Now listening on: https://localhost:5001
```

# Used Tools And Technology
- Blazor WebAssembly 5.0 with ASP.NET Core Hosted Model.
- MudBlazor Integrations – Super cool UI.
- SignalR Integrations – Real-time Messaging with Hubs.
- Cascade Parameters.
- SQL
- EF Core
- IdentityServer4
- Serilog
- Docker
- Azure VM Instance

# Features covered

- Sign Up & Login Functionality
- One-to-One Functionality
- Group Chat Functionality
- Anonymous Chat Functionality
- Notifications Alert
- Emoji
- Admin Dashboard
- Profile Management

# Dependencies
* bootstrap (>= 4.4.1)
* Microsoft.Extensions.DependencyInjection (>= 3.1.3)
* Tewr.Blazor.FileReader (>= 1.5.0.20109)
* Blazor Webassemly 5.0
* IdentityServer4

# Docker Images: ![image](https://github.com/tahminasqa/quick-chat/assets/55849117/a8f38a7d-50bc-4c2d-a2c8-2e2528605915)
### How to deploy Quick Chat docker image

> [!WARNING]
> You have the flexibility to utilize any Linux system for deploying QuickChat applications using Docker images, whether it's a Cloud-hosted virtual machine or your own Linux instance. Ensure that both Docker and docker-compose are installed on your Linux machine.

 - Pull docker images in your Linux machine: `docker pull tahminasqa/quick.chat:v2.0.2`
 - Utilize this YAML composition file to fetch the Docker image and manage your services.
```
  version: '3.4'

    services:
      quick.chat.service:
        image: tahminasqa/quick.chat:v2.0.2
        container_name: quick.chat.v2.0.2
        ports:
          - "8090:80"
        volumes:
          - /quick.chat/config/appsettings.json:/app/appsettings.json
          - /quick.chat/logs:/app/Logs:z

        restart: always
```
 - Make sure your database server has required database that can be possible by `update-database` from visual studio
 - You can manage your application configuration by utilizing the directory path of the `application.json` file as you mount the volume.
 - After setting up the docker-compose, you should be able to observe the docker container running on a specified port.
 - You can freely utilize the hosted URL to gain access to the application
```
Sample applicationSettings.json
{
  "ConnectionStrings": {
    "QuickChatServerDB": "Server={YOUR_DB_CONNECTION_STRING}"
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Skoruba": "Information",
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/Quick-Chat-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 31,
          "fileSizeLimitBytes": 10000000,
          "rollOnFileSizeLimit": true
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  },
  "AppSettings": {
    "ServicePort": 8080,
    "CertificatePassword": "IdentityServerSPAwe!@@$%G4974ssA7"
  },
  "IdentityServer": {
    "Clients": {
      "Quick.Chat.Client": {
        "Profile": "IdentityServerSPA"
      }
    },
    "Key": {
      "Type": "Development"
    }
  },
  "AllowedHosts": "*"
}
```


This project is licensed under GNU V3, so contributions/pull-requests are welcome. All contributors get listed here. 

**Contributors** 
- Tahmina Akter ([Github]([https://github.com/wsdt](https://github.com/tahminasqa)))
