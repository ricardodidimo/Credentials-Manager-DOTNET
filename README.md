# .NET 8 Credentials Management API Project

This is a demo API project built with **.NET 8** for a **Credentials Management** application. The project follows a **Clean Architecture** approach to ensure separation of concerns and maintainability. The API is designed to handle credentials securely, supporting **JWT authentication** for user verification. The project is fully automated for testing with **xUnit** and utilizes **GitHub Actions** for continuous integration (CI) to run tests automatically. It also uses **Docker** to ensure containerization and ease of deployment. The backend stores data using **PostgreSQL** and **Entity Framework Core** ORM.

## Key Features

- **Clean Architecture**: A well-organized code structure that separates business logic, infrastructure, and presentation layers for maintainability and scalability.
- **JWT Authentication**: Secure token-based authentication for users.
- **xUnit Testing**: Unit and integration tests to ensure the API works as expected.
- **CI/CD with GitHub Actions**: Automated pipelines to run tests and build the application.
- **Docker**: Containerized application for consistent deployment across environments.
- **PostgreSQL with Entity Framework Core**: A relational database for storing credentials and other application data.

## Accessing the hosted API
You can run the dockerized API locally or access the hosted version at [this azure deployment](https://passwordvault-g5egh7h6f6eububn.brazilsouth-01.azurewebsites.net/index.html)

## How to run

Before running the project, make sure you have the following installed:
- **Docker**: [Install Docker](https://www.docker.com/get-started)
- **Visual Studio Code** or any preferred IDE for C# development.

### Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/ricardodidimo/Credentials-Manager-DOTNET.git
```

### Set Up the Database
1. Create a *'.env'* file in the root directory that mirrors the keys in *'.env.examle'*, attributing your own values to the variables, especially the *'POSTGRES_CONNECTION'* variables that will point to a external postgres client. For this demo i'm
using a free hosted alternative at [supabase](https://supabase.com/). The remaning keys will all take random values of your preference.

2. Using **Docker**, you can quickly setup the container using Docker Compose.

   In the project root, you’ll find a `docker-compose.yml`. Run the following command to start the app's container:

   ```bash
   docker-compose up -d --build
   ```

   *And thats it!* You will have the demo app running at: [localhost:8080](http://localhost:8080) in your browser. The OpenAPI/Swagger UI will serve as documentation and allow you to interact with the proposed API.

### Proposed flow
  - Once you have the API running, you should first create a user in the *"CreateUserEndpoint"* section;
  - Before any futher action, you have to authenticate yourself as the newly created user in the *"AuthenticateUserEndpoint"* and collect the resulting JWT token. Back at the *top of the page* you will
    see a 'authorize' button that will ask you to insert your authentication token, copy-paste it there and now you are authenticated!
  - Next, you have to register a *"vault"* to containerize your credentials. See *"CreateVaultEndpoint"* section;
  - Finally, you can register credentials under a specific vault that will be stored and enable to later retrieval.

### GitHub Actions

1. The project includes a GitHub Actions pipeline configuration. When you push code to GitHub, the CI pipeline will automatically run the tests.
2. The pipeline is defined in `.github/workflows/ci.yml`. This file contains the steps for setting up the environment, restoring dependencies, building the project, and running tests.

## Project Structure

```
/src
  /Core
    - Entities, interfaces, and use cases. Ensures reusability for different infrastructure and presentation clients. 
  /Infrastructure
    - Data access, external services that implementes abstractions defined on 'Core'.
    - One design being a 'postgres with entity framework' infrastructure implementation.
  /WebApi
    - HTTP presentation layer. API endpoint definitions with ASPNET CORE.
  /tests
    - /Core - A simple test case used to exemplify preferred design decisions on test projects
/Dockerfile
/docker-compose.yml
.gitignore
README.md
```
