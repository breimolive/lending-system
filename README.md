# AngularDotNetTemplate

# How to run the project

## Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/cli)

## Steps to run clone the project

1. Clone the repository:
   ```bash
   git clone
    ```
2. Navigate to the project directory:
3. ```bash
   cd AngularDotNetTemplate
   ```

## Steps to run the project

Run the following commands in the terminal to start the project.
This will start both the Angular frontend and the .NET backend.

1. ```bash
   cd Server
   ```
2. Run the .NET application:
    ```bash
    dotnet run
    ```

## Steps to publish the project

Run the following commands in the terminal to publish the project.
This will publish the application to the `bin/Release/net8.0/publish` directory.
The client SPA will be published to the `wwwroot` directory of the server project.

1. Navigate to the project directory:
   ```bash
   cd Server
   ```
2. Publish the .NET application:
   ```bash
    dotnet publish
    ```


## Jetbrains Rider Run Configuration

### Run Configuration

![Server Configuration](/docs/server.png)

### Publish Configuration

![Publish Configuration](/docs/publish.png)


