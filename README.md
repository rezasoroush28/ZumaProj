ZumaProj
A simple and clean ASP.NET Core Web API project built using Clean Architecture and CQRS Pattern, with Entity Framework Core for data access.
This project allows users to manage ToDo Items with basic CRUD operations.

🛠️ Technology Stack
ASP.NET Core 8.0 (Web API)

Entity Framework Core

Clean Architecture

CQRS Pattern (using MediatR)

FluentValidation

xUnit & Moq (for Unit Testing)

SQL Server (Local DB)

🏗️ Project Structure (Clean Architecture)

Layer	Description
Domain	Core business models and enums
Application	Commands, CommandHandlers, DTOs, Validations
Infrastructure	EF Core DbContext, Repository implementations
Presentation	API Controllers
🧩 Features Implemented
CRUD operations for ToDo Items:

Create a ToDo Item

List all ToDo Items (filter by status optional)

Update a ToDo Item

Delete a ToDo Item

MediatR for handling all Commands

Input validation using FluentValidation

Exception handling inside CommandHandlers

Database integration with EF Core and Migrations

Unit tests for CommandHandlers

📝 ToDoItem Entity
Properties:

Id: int

Title: string

Description: string

Status: ToDoStatus (Enum: JustMade, InProgress, Done)

📂 Important Folders

Path	What it Contains
Domain/Entities	Domain models like ToDoItem
Domain/Enums	Enum for ToDoStatus
Application/Commands	Command Requests
Application/CommandHandlers	Handlers for executing the commands
Application/Validation	Validators for request validation
Infrastructure/Contexts	EF Core DbContext
Infrastructure/Repositories	Repository pattern implementation
Presentation/Controllers	Web API controllers
🚀 Getting Started
Prerequisites
Visual Studio 2022+ or VS Code

.NET 8 SDK

SQL Server (Local)

1. Clone the Repository
bash
Copy
Edit
git clone https://github.com/rezasoroush28/ZumaProj.git
2. Setup the Database
Update your appsettings.json (already configured like this):

json
Copy
Edit
"ConnectionStrings": {
  "SqlServer": "Server=.;Database=Zuma;Trusted_Connection=True;MultipleActiveResultSets=true"
}
3. Apply Migrations
Run these commands in the Package Manager Console:

bash
Copy
Edit
Update-Database
4. Run the Project
Start the project using Visual Studio or with command line:

bash
Copy
Edit
dotnet run
Visit the Swagger UI at:
https://localhost:{port}/swagger/index.html

📚 API Endpoints

HTTP Method	Endpoint	Description
GET	/api/todos	List all ToDo items
POST	/api/todos	Create a new ToDo item
PUT	/api/todos/{id}	Update an existing ToDo item
DELETE	/api/todos/{id}	Delete a ToDo item
🧪 Testing
Unit tests written using xUnit and Moq.

Tests cover CommandHandlers for success scenarios.

(Optional) You can add tests for failure scenarios and validations.

To run tests:

bash
Copy
Edit
dotnet test
🔥 Future Improvements
Add Global Exception Handling Middleware

Add comprehensive Validation tests

Enhance failure scenario unit tests

Deploy to Azure or AWS

👨‍💻 Author
Reza Soroush

✅ Status
✅ Project structure and features match the original requirements
✅ Fully working CRUD APIs with Clean Architecture
✅ Basic Unit Tests added