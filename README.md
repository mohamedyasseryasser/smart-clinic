
# Smart Clinic Management System

A web-based clinic management application developed using **ASP.NET Core MVC** and **SQL Server** to organize and manage daily clinic operations. The system provides an integrated solution for managing patients, appointments, medical visits, prescriptions, medications, departments, doctors, users, and invoices.

## Features

### 1. Authentication & User Management
- User authentication and account management using ASP.NET Core Identity.
- Role-based access control for Admin, Doctor, and Receptionist.
- Manage user accounts and professional profiles.
- Organize clinic staff information.

### 2. Patient Management
- Add, update, and manage patient records.
- Search and filter patient information.
- Maintain centralized patient medical records.

### 3. Appointment Management
- Schedule new patient appointments.
- View, search, and filter appointments.
- Update appointment statuses.
- Confirm patient arrivals.
- Cancel appointments.

### 4. Medical Visit Management
- Create and manage patient visits.
- Link medical visits to patients and appointments.
- Maintain organized visit records.

### 5. Medication Management
- Add and manage medications.
- Organize medications into medical categories.
- Maintain medication information.

### 6. Prescription Management
- Create prescriptions for patient visits.
- Add multiple medications to prescriptions.
- Link prescription items to their corresponding medications.
- Manage prescription records.

### 7. Invoice & Billing Management
- Generate invoices for medical visits.
- Update and track invoice statuses.
- Associate invoices with their corresponding visits.

### 8. Department & Doctor Management
- Manage clinic departments.
- Organize doctor information and professional profiles.
- Associate doctors with clinic departments.

### 9. Dashboard
- Display selected clinic activity indicators.
- Provide an overview of clinic operations.

### 10. Additional Features
- Server-side data validation using ViewModels.
- Pagination for supported data lists.
- Object mapping using AutoMapper.
- Database relationships managed through Entity Framework Core.
- Repository Pattern for organizing data access.

## Architecture

The application follows the **ASP.NET Core MVC** architectural pattern with a Repository layer to separate application responsibilities and data access.

### Application Workflow

```text
User / Browser
      |
      v
  Controllers
      |
      v
 ViewModels
      |
      v
  Razor Views
      |
      v
Repository Interfaces
      |
      v
Repository Implementations
      |
      v
Entity Framework Core
      |
      v
   SQL Server
```

### Project Structure

```text
smart clinic/
│
├── Controllers/
├── Models/
├── Views/
├── Migrations/
├── repo/
├── viewmodels/
├── mapping/
├── enums/
│
├── Program.cs
└── smart clinic.PL.csproj
```

### Architecture Components

- **Controllers:** Handle incoming requests and coordinate application operations.
- **Models:** Represent application entities such as Patient, Appointment, Visit, Prescription, and Invoice.
- **Views:** Provide the user interface using ASP.NET Core Razor Views.
- **ViewModels:** Separate data presentation and input models from database entities.
- **Repositories:** Encapsulate data access logic using repository interfaces and implementations.
- **AutoMapper:** Handles mapping between entities and ViewModels.
- **DbContext:** Manages database entities and relationships using Entity Framework Core.
- **Dependency Injection:** Registers application services, repositories, database context, Identity, and AutoMapper.

## Technologies & Tools

| Category | Technologies |
|---|---|
| Programming Language | C# |
| Backend Framework | ASP.NET Core MVC (.NET 8) |
| Frontend | Razor Views, HTML, CSS, JavaScript |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Core Identity |
| Object Mapping | AutoMapper |
| Architecture | MVC, Repository Pattern |
| Database Management | EF Core Migrations |

## Prerequisites

Before running the application, make sure you have installed:

- .NET 8 SDK
- SQL Server (Express or Developer Edition)
- Entity Framework Core CLI (dotnet-ef)

## Installation & Setup

### 1. Clone the Repository

```bash
git clone <YOUR_REPOSITORY_URL>
```

### 2. Navigate to the Project Directory

```bash
cd "smart clinic/smart clinic"
```

### 3. Configure the Database Connection

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Set your SQL Server connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<YOUR_CONNECTION_STRING>"
```

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Apply Database Migrations

```bash
dotnet ef database update
```

### 6. Run the Application

```bash
dotnet run --launch-profile http
```

The application will be available at:

```text
http://localhost:5219
```

## Database Configuration

The application uses SQL Server as its database and Entity Framework Core for database operations.

- **Connection String:** `ConnectionStrings:DefaultConnection`
- **Migrations Directory:** `Migrations/`
- **Database Updates:** Managed through Entity Framework Core CLI.

## Security Considerations

- Database credentials should be stored using User Secrets during development.
- Production credentials should be managed through environment variables or a secure secret manager.
- Authentication and authorization middleware must be configured correctly.
- Role-based authorization should be applied to protected controllers and actions.

## Future Improvements

- Add automated unit and integration tests.
- Improve dashboard analytics and reporting.
- Introduce advanced search and filtering capabilities.
- Enhance application security and authorization policies.
- Add appointment notification functionality.

## Project Status

Developed as a practical web application demonstrating ASP.NET Core MVC development, database management, authentication, repository-based data access, and real-world clinic workflow organization.

---

**Built with ASP.NET Core MVC, Entity Framework Core, and SQL Server.**
