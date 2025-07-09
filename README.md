# MagniseTaskProject Application

This is a test project for Magnise company, this project getting data from fintacharts using websockets with jwt. This project using Automapper, MSSQL Server, EF(CodeFirst), Docker, env file.

## Prerequisites
- .NET SDK (version specified in the project)
- Docker (for containerized deployment)
- MSSQL (or your preferred database)

## Setup

### 1. Database Configuration

#### 1.1 Entity Framework Core Migrations
Run the following commands to set up the database:

```bash
# Create a new migration
dotnet ef migrations add Init

# Apply the migration to the database
dotnet ef database update Init

### 2. Environment Configuration
Create a .env file in the project root with the following values:
FINTACHARTS_USERNAME=your_username_here
FINTACHARTS_PASSWORD=your_password_here

Note: Replace your_username_here and your_password_here with your actual credentials.

### 3. Running with Docker

#### 3.1 Build the Docker Image

```bash
docker build -t magnisetask .

#### 3.2 Run the Container

```bash
docker run -d -p 8080:8080 -p 8081:8081 --name magnisetask magnisetask
