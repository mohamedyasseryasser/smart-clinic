# Smart Clinic Management System

A web-based clinic management application built with **ASP.NET Core MVC** and **SQL Server** to organize daily clinic operations. The system supports managing patients, appointments, visits, prescriptions, medications, departments, doctors, staff users, and invoices.

## Features

### 1. Authentication and User Management
- User authentication and account management using ASP.NET Core Identity.
- Role-based access for Admin, Doctor, and Receptionist.
- Manage user accounts and staff profiles.

### 2. Patient Management
- Add and update patient records.
- Search and filter patient information.
- Keep patient information organized in one system.

### 3. Appointment Management
- Schedule patient appointments.
- View and search appointments.
- Update appointment statuses.
- Confirm arrivals and cancel appointments.

### 4. Medical Visit Management
- Create and manage patient visits.
- Link visits to appointments.
- Record visit notes and diagnoses.

### 5. Medication and Category Management
- Add and manage medications.
- Organize medications into categories.
- Track stock quantities, prices, suppliers, and expiry dates.

### 6. Prescription Management
- Create prescriptions for patient visits.
- Add multiple medications to a prescription.
- Record quantity, dosage, frequency, duration, and notes for each medication.
- View and print prescription details.

### 7. Invoice and Billing Management
- Create invoices for medical visits.
- Track invoice information and status.
- Associate invoices with visits.

### 8. Department and Doctor Management
- Manage clinic departments.
- Manage doctor profiles and specializations.
- Associate doctors with departments.

### 9. Dashboard
- Display selected clinic activity indicators.
- Provide an overview of clinic operations.

### 10. Additional Features
- Server-side validation using ViewModels.
- Pagination on supported lists.
- Object mapping with AutoMapper.
- Database access and relationships managed with Entity Framework Core.
- Repository interfaces and implementations for data operations.

## Screenshots

### 1. Login
![Login](docs/images/Screenshot%202026-11-16%20145118.png)

### 2. Dashboard
![Dashboard](docs/images/Screenshot%202026-11-16%20145133.png)

### 3. Doctors
![Doctors](docs/images/Screenshot%202026-11-16%20145139.png)

### 4. Create Appointment
![Create Appointment](docs/images/Screenshot%202026-11-16%20145317.png)

### 5. Appointment Details
![Appointment Details](docs/images/Screenshot%202026-11-16%20145345.png)

### 6. Confirm Appointment
![Confirm Appointment](docs/images/Screenshot%202026-11-16%20145350.png)

### 7. Start Visit
![Start Visit](docs/images/Screenshot%202026-11-16%20145357.png)

### 8. Complete Visit
![Complete Visit](docs/images/Screenshot%202026-11-16%20145422.png)

### 9. Create Medical Prescription
![Create Medical Prescription](docs/images/Screenshot%202026-11-16%20145436.png)

### 10. Visit Details
![Visit Details](docs/images/Screenshot%202026-11-16%20145454.png)

### 11. Create Invoice
![Create Invoice](docs/images/Screenshot%202026-11-16%20145507.png)

### 12. Invoices
![Invoices](docs/images/Screenshot%202026-11-16%20145529.png)

## Architecture

The application uses the **ASP.NET Core MVC** pattern. Controllers handle incoming requests and coordinate with ViewModels, repository interfaces, and repository implementations. Entity Framework Core provides database access through the application’s `Context`, which connects to SQL Server.

```text
                         ┌─────────────────┐
                         │  User / Browser │
                         └────────┬────────┘
                                  │ HTTP request
                                  ▼
                         ┌─────────────────┐
                         │   Controllers   │
                         └───────┬─────────┘
                                 │
                  ┌──────────────┴──────────────┐
                  │                             │
                  ▼                             ▼
          ┌───────────────┐             ┌───────────────┐
          │   ViewModels  │             │  Repositories │
          │ Validation &  │             │ Interfaces and│
          │ request data  │             │ implementations│
          └───────┬───────┘             └───────┬───────┘
                  │                             │
                  ▼                             ▼
          ┌───────────────┐             ┌───────────────┐
          │  Razor Views  │             │ Entity Framework
          │    (.cshtml)  │             │ Core / Context│
          └───────────────┘             └───────┬───────┘
                                                │
                                                ▼
                                        ┌───────────────┐
                                        │  SQL Server   │
                                        └───────────────┘
```

### Main Application Layers

- **Controllers:** Handle HTTP requests and coordinate application operations.
- **ViewModels:** Carry data between forms, controllers, and views, including validation rules.
- **Razor Views:** Render the user interface using `.cshtml` files.
- **Repository Interfaces:** Define data-operation contracts.
- **Repository Implementations:** Implement operations for patients, appointments, visits, medicines, prescriptions, categories, invoices, and users.
- **Entity Framework Core:** Maps application entities to database tables and handles database queries.
- **SQL Server:** Stores application data.
- **ASP.NET Core Identity:** Manages authentication and user roles.
- **AutoMapper:** Maps between entities and ViewModels.

## Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core MVC
- **Target Framework:** .NET 8
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core with the SQL Server provider
- **Authentication:** ASP.NET Core Identity
- **Object Mapping:** AutoMapper
- **Frontend:** Razor Views, HTML, CSS, Bootstrap, and JavaScript
- **Client-side validation:** jQuery Validation and jQuery Unobtrusive Validation

## Tools

- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code
- Microsoft SQL Server, SQL Server Express, or LocalDB
- SQL Server Management Studio (SSMS), Azure Data Studio, or another SQL client
- Git and GitHub

## How to Run

### Prerequisites

Install the .NET 8 SDK and make sure a SQL Server instance is running. You also need a database account or Windows account with permission to create and access the application database.

### 1. Clone the repository

```bash
git clone https://github.com/mohamedyasseryasser/smart-clinic.git
cd smart-clinic
```

### 2. Configure the database connection

The application reads the SQL Server connection string from the `DefaultConnection` setting in `appsettings.json`.

Set `ConnectionStrings:DefaultConnection` to match your SQL Server instance and database. For example, for a local SQL Server using Windows authentication:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SmartClinicDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

Update the server name and authentication options for your environment. Do not commit real passwords or other sensitive connection details to a public repository. For local development, you can use .NET User Secrets or environment variables to keep secrets out of source control.

### 3. Restore dependencies

From the repository root, run:

```bash
dotnet restore "smart clinic/smart clinic.PL.csproj"
```

### 4. Apply database migrations

```bash
dotnet ef database update \
  --project "smart clinic/smart clinic.PL.csproj" \
  --startup-project "smart clinic/smart clinic.PL.csproj"
```

If the `dotnet ef` command is not available, install the EF Core command-line tool:

```bash
dotnet tool install --global dotnet-ef --version 9.0.0
```

The application requires a reachable database connection. Configure `DefaultConnection` before applying migrations or running the application.

### 5. Run the application

```bash
dotnet run --project "smart clinic/smart clinic.PL.csproj"
```

Open the local URL shown in the terminal output.

> The repository does not document a default demo username or password. Sign in with an account that has been created and assigned the required role in the database.

## Database Migrations

Entity Framework Core migrations are located in the `Migrations` folder. After configuring the database connection, `dotnet ef database update` creates or updates the database schema to the latest migration.

## License

No license file is currently specified. Add a `LICENSE` file if you want to define how others may use, modify, or distribute this project.
