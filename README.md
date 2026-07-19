# 🏠 RentAPlace – Property Rental Management Platform

[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Frontend-Angular-red.svg)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-blue.svg)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![JWT](https://img.shields.io/badge/Auth-JWT-green.svg)](https://jwt.io/)

RentAPlace is a **full-stack property rental platform** that connects **Property Owners** with **Renters** through a secure and user-friendly web application. Built using **ASP.NET Core Web API**, **Angular**, and **SQL Server**, the platform enables owners to list and manage properties while allowing renters to search, book, and communicate with property owners. The application features secure JWT authentication, role-based authorization, property management, booking workflows, messaging, and image uploads.

***

## ✨ Key Features

- 🔐 **JWT Authentication & Authorization:** Secure login with Owner and Renter roles.
- 🏠 **Property Management:** Owners can create, update, delete, and manage property listings.
- 🔍 **Advanced Property Search:** Filter, sort, and paginate property listings.
- 📅 **Booking Management:** Create, cancel, and update booking requests.
- 💬 **Owner-Renter Messaging:** Built-in communication between renters and property owners.
- 🖼️ **Property Image Upload:** Upload and manage images for property listings.
- 📖 **Swagger API Documentation:** Interactive API testing using Swagger UI.
- 🌱 **Seeded Demo Data:** Ready-to-use demo accounts for quick testing.

***

## 🛠️ Tech Stack

### Backend

- **Framework:** ASP.NET Core Web API (.NET 10)
- **Programming Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Authentication
- **Password Security:** BCrypt Password Hashing
- **Testing:** xUnit

### Frontend

- **Framework:** Angular
- **Language:** TypeScript
- **Routing:** Angular Router
- **Security:** Route Guards & HTTP Interceptor
- **Styling:** Tailwind CSS

***

## 🚀 Project Workflow

1. Users register or log in as either an **Owner** or **Renter**.
2. Owners create and manage property listings with images.
3. Renters browse available properties using filters and search options.
4. Renters submit booking requests for selected properties.
5. Owners approve or reject booking requests.
6. Owners and renters communicate through the built-in messaging system.
7. The platform securely manages authentication, bookings, and property information.

***

## 📂 Project Structure

```text
RentAPlace/
│
├── Backend/
│   ├── Controllers/
│   ├── Services/
│   ├── Data/
│   ├── Models/
│   ├── DTOs/
│   ├── Middleware/
│   ├── Migrations/
│   └── Tests/
│
├── Frontend/
│   └── src/
│       └── app/
│           ├── core/
│           ├── pages/
│           ├── layout/
│           └── shared/
│
├── docs/
├── README.md
└── .gitignore
```

***

## 📊 Applications

- 🏠 Property Rental Management
- 🏢 Apartment & House Listings
- 🏡 Vacation Home Booking
- 🏘️ Real Estate Platforms
- 📅 Online Reservation Systems
- 💬 Customer Communication Portal
- 🌐 Full-Stack Web Application Development
- 📱 Rental Marketplace Solutions

***

## 🎯 Future Enhancements

- 📱 Mobile application support.
- 💳 Online payment gateway integration.
- ⭐ Property reviews and ratings.
- 🗺️ Google Maps integration.
- ❤️ Wishlist and favorite properties.
- 📧 Email and SMS booking notifications.
- ☁️ Cloud deployment with CI/CD.
- 📊 Admin dashboard with analytics and reports.

***

## 💻 Installation

### Clone the Repository

```bash
git clone https://github.com/yourusername/RentAPlace.git

cd RentAPlace
```

### Backend Setup

Update the connection string in:

```text
Backend/appsettings.json
```

Run the backend:

```bash
dotnet restore Backend/Backend.csproj

dotnet run --project Backend/Backend.csproj
```

Backend URLs:

```
API:
http://localhost:5255

Swagger:
http://localhost:5255/swagger
```

### Frontend Setup

```bash
cd Frontend

npm install

npm start
```

Frontend URL:

```
http://localhost:4200
```

***

## 👤 Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| Owner | owner@demo.com | Password123! |
| Renter | renter@demo.com | Password123! |

***

## 🔗 Core API Endpoints

### 🔐 Authentication

- `POST /api/Auth/register`
- `POST /api/Auth/login`

### 🏠 Property

- `GET /api/Property`
- `GET /api/Property/{id}`
- `POST /api/Property`
- `PUT /api/Property/{id}`
- `DELETE /api/Property/{id}`
- `GET /api/Property/my`
- `POST /api/Property/upload-image`

### 📅 Booking

- `POST /api/Booking`
- `GET /api/Booking/my`
- `GET /api/Booking/owner`
- `PATCH /api/Booking/{id}/cancel`
- `PATCH /api/Booking/{id}/status`

### 💬 Messaging

- `GET /api/Message/my`
- `POST /api/Message`

***

## 📸 Output

- Secure login for Owners and Renters.
- Property listings with images.
- Advanced property search and filtering.
- Booking management dashboard.
- Owner-Renter messaging interface.
- Swagger API documentation for backend testing.

***

## 📈 Results

- Secure role-based authentication using JWT.
- Efficient property listing and booking workflow.
- Responsive Angular frontend with modern UI.
- Scalable RESTful backend using ASP.NET Core.
- Complete full-stack solution for property rental management.

***

## 🤝 Contributing

Contributions are welcome!

Feel free to fork the repository, create a feature branch, and submit a pull request.

***

## 📜 License

This project is licensed under the **MIT License**.

***

## 👨‍💻 Author

**Sravan**

AI | Machine Learning | Deep Learning | Computer Vision | Full Stack Developer

If you found this project useful, don't forget to ⭐ the repository!
