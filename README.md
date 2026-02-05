# 🚖 Taxis Backend API (.NET Framework)

Enterprise-style RESTful API built with .NET Framework for managing taxi fleets, drivers, accidents (sinisters), documents and role-based system users.

Designed with layered architecture, JWT authentication and production-ready Azure compatibility.

---

# 📌 System Overview

This backend provides:

• Secure authentication using JWT  
• Role-based authorization (Admin / Basic User)  
• Full CRUD operations across all modules  
• Relational SQL Server database (30+ tables)  
• File metadata management for cloud storage  
• Audit logging (Login / Error / Change tracking)  
• Dashboard statistical calculations (monthly growth metrics)  

⚠️ Repository Note  
This public repository is NOT currently deployed in Azure.  
However, the production implementation was deployed using:

• Azure App Service  
• Azure SQL Database  
• Azure Static Web Apps (Frontend)  
• Firebase Storage / Azure Blob Storage  

---

# 🏗️ Architecture

Layered architecture implementation:

```
Controllers  → HTTP handling layer
Services     → Business logic layer
Repositories → Data access abstraction
Data Layer   → SQL Server interaction
Auth Layer   → JWT security
Logs Layer   → Auditing & traceability
```

Designed for scalability and cloud deployment.

---

# 🔧 Technologies

• .NET Framework Web API  
• Entity Framework  
• SQL Server (Azure compatible)  
• JWT Authentication  
• Firebase Storage / Azure Blob Storage  
• GitHub Actions (CI/CD ready)  

---

# 🔐 Authentication Flow
- JWT-based authentication
- - **Refresh Tokens**
- Role-based authorization (Admin / User / Guest)
- Secure password hashing
- Change tracking logs
- Error logging for auditing and traceability
  
## Login

```http
POST /api/auth/login
```

### Request Body
```json
{
  "name": "Kevin",
  "password": "plainOrHashedPassword"
}
```

### Response
```json
{
  "token": "JWT_TOKEN",
  "expiresIn": 3600,
  "roleId": 3,
  "permissions": {
    "driver": true,
    "admin": true,
    "permissionair": true,
    "unit": true,
    "sinister": true,
    "extraData": true,
    "pdf": true
  }
}
```

JWT must be included in all secured requests:

```http
Authorization Header Required:
Authorization: Bearer {token}
```

---

# 👤 USERS MODULE (usersData + userPermissions + roles)
- User registration (admin only)
- Role management
- Secure password reset
- Permission configuration per module

Tables:
- usersData
- userPermissions
- roles

## GET – Paginated List

```http
GET /api/users?page=1
```

```json
[
  {
    "id": 1,
    "username": "admin",
    "role": "Admin",
    "isActive": true
  }
]
```

---

## POST – Create User (Admin Only)

```http
POST /api/users
```

```json
{
  "name": "Leo",
  "password": "hashedPassword",
  "email": "leo@email.com",
  "roleId": 2,
  "permissions": {
    "driver": true,
    "admin": false,
    "permissionair": true,
    "unit": true,
    "sinister": false,
    "extraData": false,
    "pdf": true
  }
}
```

### Response
```json
{
  "message": "User created successfully",
  "statusCode": 201
}
```

---

# 🚖 Drivers Endpoint
- Driver registration and management
- License tracking
- Emergency contact management
- Relationship with vehicles and accident(Sinisters)
## GET – Paginated

```http
GET /api/drivers?page=1
```

```json
[
  {
    "id": 15,
    "fullName": "Juan Perez",
    "licenseNumber": "A1234567",
    "status": "Active",
    "assignedUnit": "TX-204"
  }
]
```

---

## POST – Create Driver

```http
POST /api/drivers
```

```json
Request:
{
  "name": "Juan",
  "lm1": "Perez",
  "lm2": "Lopez",
  "phone": "6681234567",
  "st1": 9,
  "st2": 10,
  "st3": 11,
  "settlement": 2,
  "extNumber": 4567,
  "birth": "1999-02-06",
  "admin": 2,
  "licenseEx": "2026-12-01",
  "ingressPay": 1,
  "status": 1
}
```

### Response
```json
{
  "message": "Driver registered successfully",
  "statusCode": 201
}
```
DELETE
```bash
/api/drivers/{id}
```
Soft delete (status change or logical flag).
---

# ⚠️ Sinisters (Accidents)

## POST – Register Accident

```http
POST /api/sinisters
```

```json
{
  "driverId": 15,
  "unitId": 3,
  "description": "Rear collision",
  "insuranceId": 2,
  "date": "2026-01-14"
}
```

### Response
```json
{
  "message": "Sinister registered successfully",
  "statusCode": 201
}
```

---

# 📊 Dashboard Metrics

## GET – Monthly Growth

```http
GET /api/dashboard/monthly-growth?year=2026
```

```json
{
  "driversIncrease": 12,
  "sinistersIncrease": 4,
  "percentageGrowthDrivers": 8.4,
  "percentageGrowthSinisters": 2.1
}
```

Used for real-time dashboard charts in the frontend.

---

# 📊 Logs Endpoint

## GET – Login History

```http
GET /api/logs/login
```

```json
[
  {
    "username": "admin",
    "loginDate": "2026-01-15T08:30:22",
    "ipAddress": "192.168.1.10"
  }
]
```

---

# 🗄️ Database Structure

• SQL Server relational database  
• 30+ normalized tables  
• Foreign key relationships  
• Indexed for optimized queries  
• Designed for Azure SQL scalability  
• Supports 500+ active users  

---

# 🚀 Running Locally

## 1️⃣ Clone repository

```bash
git clone https://github.com/elkelvinx/ApiTaxis
```

## 2️⃣ Open solution in Visual Studio

Open `.sln` file.

## 3️⃣ Configure Database Connection

Edit `web.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection"
       connectionString="Server=YOUR_SERVER;
                         Database=YOUR_DATABASE;
                         User Id=YOUR_USER;
                         Password=YOUR_PASSWORD;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## 4️⃣ Restore NuGet Packages

```bash
Update-Package -Reinstall
```

## 5️⃣ Run Application

Press:

```
F5
```

API will run at:

```
https://localhost:44319/api/
```

---

# 🌐 Frontend Repository

Angular 17 Frontend:

https://github.com/elkelvinx/CrudTaxis

---

# ☁️ Production Deployment (Enterprise Implementation)

Production version deployed with:

• Azure App Service  
• Azure SQL Database  
• Azure Static Web Apps  
• Cloud Storage integration  
• CI/CD with GitHub Actions  

---

# 🧪 Future Improvements

• Enhanced audit system  
• Performance caching layer  
• Microservices architecture migration  
• Distributed logging integration  

---

# 📄 License

MIT
