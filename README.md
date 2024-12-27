# MyCleanArchitectureApp

## Overview
MyCleanArchitectureApp is a .NET 8 Web API project that follows Clean Architecture and SOLID principles, implementing Domain-Driven Design (DDD). The application is structured into four main layers: Domain, Application, Infrastructure, and Web API.

## Project Structure
```
MyCleanArchitectureApp
├── src
│   ├── MyCleanArchitectureApp.Domain
│   ├── MyCleanArchitectureApp.Application
│   ├── MyCleanArchitectureApp.Infrastructure
│   └── MyCleanArchitectureApp.WebAPI
├── MyCleanArchitectureApp.sln
└── README.md
```

## Layers
- **Domain Layer**: Contains the core business logic, entities, and interfaces.
- **Application Layer**: Handles application logic, DTOs, and service interfaces.
- **Infrastructure Layer**: Manages data access, messaging, and external service integrations.
- **Web API Layer**: Exposes HTTP endpoints for client interactions.

## Features
- Order creation and management.
- Persistence to Azure SQL Database.
- Messaging with Azure Storage Queue.
- Deployment to Azure Container Service.

## Setup Instructions
1. Clone the repository.
2. Navigate to the project directory.
3. Restore dependencies using `dotnet restore`.
4. Update the `appsettings.json` file with your Azure SQL Database and Azure Storage Queue connection strings.
5. Run the application using `dotnet run`.

## Usage
- Use the `OrderController` to create and manage orders via HTTP requests.
- The application supports creating orders by sending a POST request to the `/orders` endpoint with the required order data.

## Architecture Overview
This project adheres to Clean Architecture principles, ensuring separation of concerns and maintainability. Each layer interacts with the others through well-defined interfaces, promoting testability and flexibility.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request for any enhancements or bug fixes.


