 Smart Clinic Management System
Project Description
This project is a comprehensive web-based Smart Clinic Management System developed 
using ASP.NET Core MVC. It provides a robust platform for managing various aspects of a 
clinic's operations, including patient appointments, medical records, prescriptions, 
invoicing, and user management. The system is designed to streamline clinic workflows, 
improve efficiency, and enhance patient care.
Features
• 
User Authentication & Authorization: Secure login system with role-based access 
control for administrators, doctors, and receptionists.
• 
Patient Management: Register new patients, view patient details, and manage patient 
medical history.
• 
Appointment Scheduling: Create, view, update, and cancel patient appointments.
• 
Department Management: Organize clinic services by departments.
• 
Doctor Management: Manage doctor profiles and their associated departments.
• 
Medicine Management: Maintain a database of available medicines.
• 
Prescription Management: Generate and manage patient prescriptions.
• 
Invoice Management: Create and manage invoices for services rendered.
• 
Visit Management: Record and track patient visits.
• 
Dashboard: Overview of key clinic metrics and activities.
Technologies Used
• 
Backend: ASP.NET Core MVC
• 
Database: SQL Server
• 
ORM: Entity Framework Core
• 
Authentication: ASP.NET Core Identity
• 
Mapping: AutoMapper
• 
Frontend: HTML5, CSS3, JavaScript, jQuery, Bootstrap
Setup Instructions
1. Clone the repository:
Bash
git clone <repository_url>
cd smart-clinic
2. Database Configuration:
• 
Open 
appsettings.json and update the 
connection details.
DefaultConnection string with your SQL Server 
• 
Run database migrations to create the database and tables:
Bash
dotnet ef database update
3. Run the application:
Bash
dotnet run
The application will typically run on 
Usage
https://localhost:5001 or 
http://localhost:5000 .
Upon running the application, navigate to the login page. You can log in with existing user 
credentials or register new users (if enabled ). The system provides different functionalities 
based on the user's role (Admin, Doctor, Receptionist).
