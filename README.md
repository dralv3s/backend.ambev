# Ambev Developer Evaluation Project

This document provides instructions and requirements for running the Ambev Developer Evaluation project.

## Requirements

To run this project, you will need the following software installed on your machine:

* **Visual Studio 2022:** Required for building and running the .NET backend application.
* **Docker:** Required for setting up and running the PostgreSQL database container.

## Instructions

Follow the steps below to get the project up and running:

### 1. Set up the PostgreSQL Database using Docker

The project uses a PostgreSQL database. You can easily set up a containerized instance using Docker with the following command:

```bash
docker run --name dev_eval_pgsql -e POSTGRES_PASSWORD=Pass@word -e POSTGRES_USER=sa -e POSTGRES_DB=DeveloperEvaluation -p 5432:5432 -d postgres
```

### 2. Run the Project using Visual Studio 2022
Once the database container is running, you can run the project using Visual Studio 2022:

Open the Solution: Navigate to the project's root directory and open the solution file (Ambev.DeveloperEvaluation.sln) in Visual Studio 2022.

Once visual studio finishes loading the solution just press F5 to start the project, it will launch a new tab in the browser with the swagger interface where you can easily test the endpoints available 
