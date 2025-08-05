# Clinical Trials Medicine Monitor - ASP.NET Web API

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)

## Overview

This ASP.NET Web API application is designed to monitor experimental medicines during various clinical trials.

## Features

- **Medicine Tracking**: Monitor experimental medicines throughout clinical trial phases
- **Patient Management**: Track patient participation and responses to treatments
- **Supply Management**: Delivery of medicines to clinics
- **Data Analytics**: Generate reports on medicine performance metrics
- **Role-based Access**: Secure access control for researchers, clinicians, and administrators

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI for documentation
- JWT Authentication
- AutoMapper
- FluentValidation
- xUnit for unit testing

## Getting Started

### Prerequisites

- .NET 8.0 or later
- PostgreSQL
- Visual Studio 2022 or VS Code with C# extensions

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/clinical-trials-medicine-monitor.git
   ```

2. Navigate to the project directory:
   ```bash
   cd clinical-trials-medicine-monitor
   ```

3. Configure the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=ClinicalTrialsDB;Trusted_Connection=True;"
   }
   ```

4. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

## API Documentation

The API is documented using Swagger UI. After running the application, access the documentation at:

```
https://localhost:5001/swagger
```

## Project Structure

```
ClinicalTrialsMedicineMonitor/
├── Controllers/         # API controllers
├── Models/             # Data models and DTOs
├── Services/           # Business logic services
├── Repositories/       # Data access layer
├── Migrations/         # Database migrations
├── wwwroot/            # Static files
├── appsettings.json    # Configuration
└── Program.cs          # Application entry point
```

## Authentication

The API uses JWT Bearer authentication. To authenticate:

1. Request a token from the `/api/auth/login` endpoint
2. Include the token in subsequent requests in the Authorization header:
   ```
   Authorization: Bearer {your-token}
   ```

## Testing

Run unit tests with:
```bash
dotnet test
```

## Environment Variables

For production environments, configure these variables:

- `ASPNETCORE_ENVIRONMENT`: Set to "Production"
- `JWT_SECRET`: Your JWT signing key
- `DB_CONNECTION_STRING`: Production database connection string

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Create a new Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For questions or support, please contact:
[Your Name] - [your.email@example.com]

Project Link: [https://github.com/your-username/clinical-trials-medicine-monitor](https://github.com/your-username/clinical-trials-medicine-monitor)
