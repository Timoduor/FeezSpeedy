# FeezSpeedy

## Under development

## Overview
FeezSpeedy is a comprehensive school fee management system built with ASP.NET Core. It allows parents to manage their children's school fees, apply for loans, track repayments, and handle dependent information. Administrators can approve fee requests, manage payment options, and oversee disbursements.

**Status: Under Development**  
This project is currently in active development. Features may be incomplete, and the codebase is subject to changes.

## Features
- **Parent Dashboard**: Manage dependents, apply for fee assistance, view repayment schedules.
- **Admin Panel**: Approve fee requests, configure payment options, manage disbursements.
- **Loan Management**: Preview and apply for educational loans.
- **Repayment Tracking**: Monitor repayment progress and schedules.
- **User Authentication**: Secure login and registration for parents and admins.
- **Responsive UI**: Built with ASP.NET Core MVC and integrated front-end components.

## Tech Stack
- **Backend**: ASP.NET Core 8.0, Entity Framework Core
- **Database**: SQL Server (configured via appsettings.json)
- **Frontend**: Razor Views, HTML, CSS, JavaScript
- **Authentication**: ASP.NET Core Identity
- **Deployment**: Docker support included

## Prerequisites
- .NET 8.0 SDK
- SQL Server or compatible database
- Visual Studio 2022 or VS Code with C# extensions

## Setup Instructions
1. Clone the repository:
   ```
   git clone <repository-url>
   cd FeezSpeedy
   ```

2. Restore NuGet packages:
   ```
   dotnet restore
   ```

3. Update the database:
   ```
   dotnet ef database update
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. Open your browser and navigate to `https://localhost:5001` (or the configured port).

## Configuration
- Update `appsettings.json` for database connection strings and other settings.
- Development-specific settings are in `appsettings.Development.json` (ignored in version control).

## Project Structure
- **Controllers/**: MVC controllers for handling requests.
- **Models/**: Entity models and view models.
- **Views/**: Razor views for the UI.
- **Data/**: Database context and migrations.
- **wwwroot/**: Static files and front-end assets.
- **Migrations/**: Entity Framework migrations.

## Contributing
This project is under development. Contributions are welcome once the initial version is stable. Please follow standard .NET coding practices.

## License
[Add license information here]

## Contact
[Add contact information here]