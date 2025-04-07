# IceSync - Workflow Synchronization Application

## Overview
IceSync is a web application that synchronizes workflow data between a Universal Loader API and a local SQL Server database. It provides a user interface to view workflows, manually trigger workflow executions, and automatically synchronizes data every 30 minutes.

## Live Demo
https://ivandimitrov1.github.io/icesync/

## Technical Stack
- **Frontend**: React
- **Backend**: .NET 8 Web API
- **Database**: Postgres
- **Architecture**: Clean Architecture
- **ORM**: Entity Framework Core (migrations are done on startup)
- **Background Processing**: Hangfire
- **External communication**: Refit
- **Testing**: XUnit, TestContainers
- **Docker support**: true
- **CI/CD**: Github actions, automated deploy
- **FE Host**: Github pages
- **BE Host**: Azure
  
## Features
- Workflow listing
- Running a worfklow
- Manual workflow synchronization (via hangfire, visible and enabled for all)
- Automatic synchronization every 30 minutes

## Prerequisites
- .NET 8 SDK
- Node.js (for React frontend)
- SQL Server
- Docker (needed for the tests and app deploy)

## Development phase 
- start a sqlserver via docker or download and start local sqlserver 
- start the **IceSync.sln** file and then build and start the project, (adjust the appsettings.json if needed), it should be accessbile from localhost:8081/swagger
- open frontend folder and run '**npm install**' , then '**npm start**'. The backend url is already binded in the .env file.

- to add a new migration, set Infrastructure prj as a start project and run 'dotnet ef --startup-project ../IceSync.Api/ migrations add MigrationScriptName -o Data/Migrations'

## App deploy
- open the terminal from the root folder and execute **docker compose up**, then the FE will be accessible from localhost:3000 and the backend from localhost:8080
