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

## Screenshots

### 1. Dashboard
![Dashboard](docs/images/Screenshot%202026-11-16%20145118.png)

### 2. Screenshot 2
![Screenshot 2](docs/images/Screenshot%202026-11-16%20145133.png)

### 3. Screenshot 3
![Screenshot 3](docs/images/Screenshot%202026-11-16%20145139.png)

### 4. Screenshot 4
![Screenshot 4](docs/images/Screenshot%202026-11-16%20145317.png)

### 5. Screenshot 5
![Screenshot 5](docs/images/Screenshot%202026-11-16%20145345.png)

### 6. Screenshot 6
![Screenshot 6](docs/images/Screenshot%202026-11-16%20145350.png)

### 7. Screenshot 7
![Screenshot 7](docs/images/Screenshot%202026-11-16%20145357.png)

### 8. Screenshot 8
![Screenshot 8](docs/images/Screenshot%202026-11-16%20145422.png)

### 9. Screenshot 9
![Screenshot 9](docs/images/Screenshot%202026-11-16%20145436.png)

### 10. Screenshot 10
![Screenshot 10](docs/images/Screenshot%202026-11-16%20145454.png)

### 11. Screenshot 11
![Screenshot 11](docs/images/Screenshot%202026-11-16%20145507.png)

### 12. Screenshot 12
![Screenshot 12](docs/images/Screenshot%202026-11-16%20145529.png)

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
