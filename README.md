# RentAPlace

RentAPlace is a full-stack property rental platform where **Owners** can list/manage properties and **Renters** can explore, book, and message owners.

## Features

- JWT-based authentication and role-based authorization (`Owner`, `Renter`)
- Property CRUD for owners
- Property search with filters, sorting, and pagination
- Booking workflow (create, cancel, owner status updates)
- Owner-renter messaging
- Image upload for property listings
- Swagger API docs
- Seeded demo data for quick testing

## Tech Stack

### Backend
- ASP.NET Core Web API (`.NET 10`)
- Entity Framework Core (SQL Server)
- JWT Bearer Auth
- BCrypt password hashing
- xUnit tests

### Frontend
- Angular (standalone components)
- TypeScript
- Angular Router, Guards, HTTP Interceptor
- TailwindCSS setup

## Project Structure

```text
RentAPlace/
  Backend/
    Controllers/
    Services/
    Data/
    Models/
    DTOs/
    Middleware/
    Migrations/
    Tests/
  Frontend/
    src/app/
      core/
      pages/
      layout/
      shared/
  docs/
```
##  Getting Started

###  Prerequisites

- .NET SDK 10
- SQL Server / SQL Server Express
- Node.js + npm

---

##  1) Clone Repository

```bash
git clone https://github.com/<your-username>/RentAPlace.git
cd RentAPlace
```
## 2) Backend Setup

Update connection string in:

Backend/appsettings.json

Then run:
```bash
dotnet restore Backend/Backend.csproj
dotnet run --project Backend/Backend.csproj
```

Backend runs at:

API → http://localhost:5255
Swagger → http://localhost:5255/swagger
## 3) Frontend Setup
```bash
cd Frontend
npm install
npm start
```

Frontend runs at:

http://localhost:4200

##  Demo Credentials (Seeded)

| Role | Email | Password |
|------|-------|----------|
| Owner | owner@demo.com | Password123! |
| Renter | renter@demo.com | Password123! |

##  Core API Endpoints

###  Auth

- `POST /api/Auth/register`  
- `POST /api/Auth/login`  

### Property

- `GET /api/Property`  
- `GET /api/Property/{id}`  
- `POST /api/Property` *(Owner)*  
- `PUT /api/Property/{id}` *(Owner)*  
- `DELETE /api/Property/{id}` *(Owner)*  
- `GET /api/Property/my` *(Owner)*  
- `POST /api/Property/upload-image` *(Owner)*  

### Booking

- `POST /api/Booking` *(Renter)*  
- `GET /api/Booking/my` *(Renter)*  
- `GET /api/Booking/owner` *(Owner)*  
- `PATCH /api/Booking/{id}/cancel` *(Renter)*  
- `PATCH /api/Booking/{id}/status` *(Owner)*  

###  Message

- `GET /api/Message/my`  
- `POST /api/Message`  