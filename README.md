# EasyCommerce-API 🛍️
A robust and scalable ecommerce platform built with ASP.NET Web API.

## 📖 Introduction

**EasyCommerce-API** is an back-end for ecommerce platform that provides a space for sellers to list their clothing and shoe products. It offers a user-friendly interface and a robust set of features to facilitate online commerce. The platform is built with ASP.NET Web API, ensuring a high level of performance and scalability.

## Table of Contents 📑
- [Introduction](#-introduction)
- [Project Structure](#-project-structure)
- [Installation](#-installation)
- [Usage](#-usage)
- [Contributing](#-contributing)
- [License](#-license)
- [Features](#-features)
- [API Endpoints](#-api-endpoints)
- [Database Schema](#-database-schema)
- [Authentication and Authorization](#-authentication-and-authorization)
- [Error Handling](#-error-handling)
- [Testing](#-testing)


## 🏗️ Project Structure
<details>
<summary>Click to expand</summary>

The project has the following structure:

- **Root Directory**
    - **Dependencies**: Contains third-party libraries and NuGet packages used by the project.
    - **Properties**: Stores configuration files for the project.
    - **Config**: Holds additional configuration files specific to the application's logic.
    - **Controllers**: Contains the C# classes responsible for handling API requests and responses.
    - **Data**: Houses the data access layer of the application.
    - **Hubs**: Contains SignalR hubs if the application uses real-time communication features.
    - **Middleware**: Holds custom middleware components that intercept and process HTTP requests before they reach the controllers.
    - **Models**: Stores the C# classes representing the data models used by the application.
    - **RequestHelpers**: Contains helper classes for processing and validating API request data.
    - **Services**: Holds the application's core business logic, implemented as reusable services.
    - **.env.dev**: Stores environment variables specific to the development environment.
    - **appsettings.json**: Contains the main application configuration settings.
    - **appsettings.Development.json**: Overrides or augments appsettings.json with development-specific settings.
    - **Dockerfile**: Specifies the instructions for building a Docker image for the application.
    - **Program.cs**: The main entry point of the application.

- **Subdirectory (C# Lib)**
    - **Dependencies**: Holds external libraries for this sub-project.
    - **Env.cs**: Defines environment variables specific to this sub-project.

</details>

## ⚙️ Installation
<details>
<summary>Click to expand</summary>

To install and run the project, you have two options:

1. **Using Docker Compose**: Run the command `docker-compose up --build`. This will build the Docker images and start the services, including the PostgreSQL container.

2. **Locally without Docker**: Change the `DB_CONNECTION_STRING` in the `appsettings.Development.json` file to point to your local PostgreSQL database. Then, run the project using your preferred method (e.g., your IDE, `dotnet run`, etc.).

Before building the project, you might want to run `dotnet restore` to ensure all NuGet packages used in the project are restored. However, this step is not strictly necessary as `dotnet build` will implicitly run `dotnet restore` if it's needed.

</details>

## 🚀 Usage
<details>
<summary>Click to expand</summary>

Instructions on how to use the project.

</details>

## 👥 Contributing
<details>
<summary>Click to expand</summary>

Instructions on how to contribute to the project.

</details>

## 📄 License
<details>
<summary>Click to expand</summary>

Information about the project's license.

</details>

## 💡 Features
<details>
<summary>Click to expand</summary>

List of features in the project.

</details>

## 🌐 API Endpoints
<details>
<summary>Click to expand</summary>

Description of the API endpoints and their functionality.

</details>

## 💾 Database Schema
<details>
<summary>Click to expand</summary>

Description of the database schema used in the project.

</details>

## 🔒 Authentication and Authorization
<details>
<summary>Click to expand</summary>

Description of how authentication and authorization is handled in the project.

</details>

## ⚠️ Error Handling
<details>
<summary>Click to expand</summary>

Description of how errors are handled in the project.

</details>

## 🧪 Testing
<details>
<summary>Click to expand</summary>

Description of how testing is done in the project.

</details>