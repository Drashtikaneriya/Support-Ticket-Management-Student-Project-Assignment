# 🎫 Support Ticket Management API

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)](https://dotnet.microsoft.com/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

A professional, production-ready ASP.NET Core Web API for a Helpdesk Support System. Designed with clean architecture principles and robust security.

## 💻 Tech Stack
- **Backend**: ASP.NET Core 8.0 (Web API)
- **Database**: SQL Server (Entity Framework Core)
- **Security**: JWT Authentication, Role-Based Access Control (RBAC)
- **Documentation**: Swagger UI
- **Testing**: Postman / Swagger

## 🚀 Key Features
- **JWT Authentication**: Secure login and session management.
- **RBAC**: Three distinct roles (**MANAGER**, **SUPPORT**, **USER**) with specific permissions.
- **Ticket Lifecycle**: Managed workflow (OPEN → IN_PROGRESS → RESOLVED → CLOSED).
- **Audit Logs**: Automatic tracking of every ticket status change.
- **Comment System**: Real-time communication on support tickets.

## 📂 Project Structure
```text
Controllers/    - API endpoints with role-based protection.
Models/         - Database entities (Ticket, User, Comment, AuditLog).
Services/       - Business logic (Transitions, JWT generation).
DTOs/           - Data Transfer Objects for clean API contracts.
Data/           - AppDbContext, Seeding, and Data configurations.
```


## 🔌 API Endpoints

| Method | Endpoint | Access | Description |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/auth/login` | Public | Authenticate user and get JWT |
| `GET` | `/api/tickets` | All Roles | List tickets (filtered by role) |
| `POST` | `/api/tickets` | USER | Create a new support ticket |
| `PATCH` | `/api/tickets/{id}/status` | SUPPORT/MANAGER | Update ticket status |
| `POST` | `/api/users` | MANAGER | Create new users (Support/Manager) |


## 🛡 License
Distributed under the MIT License. See `LICENSE` for more information.

---
**Author**: [Drashtikaneriya](https://github.com/Drashtikaneriya)

