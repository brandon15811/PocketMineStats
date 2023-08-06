## Deployment
Deploying PocketMine-MP Stats is easy:

### Development:
Pre-requisites:
- Git
- .NET IDE of your choice
- MySQL/MariaDB server

Steps:
1. Clone the repo
2. Open the PocketMineStats.sln file in your IDE
3. Set your [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#set-a-secret) using the example below
    ```
    {
        "ConnectionStrings": {
            "StatsContext": "server=127.0.0.1;user id=stats;password=password;database=stats"
        }
    } 
    ```
4. Run the project, or use `dotnet run` from the PocketMineStats.Web directory if you're using the CLI

### Production

Pre-requisites:
- Git
- Docker

Steps:
1. Clone the repo
2. `cd` into it
3. Copy `example.env` to `.env` **and edit it to change the db passwords**
    1. Optional: Set the APPLICATIONINSIGHTS_CONNECTION_STRING to a valid Azure Application Insights connection string for diagnostics
4. Run `docker compose up mariadb` to initialize the database, and then Ctrl+C when it's done loading
5. Run `docker compose up -d` to start the server (Note: this uses Docker Compose V2, so the command is `docker compose` instead of `docker-compose`)

And that's it! The server is now accessible on http://localhost:5040.
