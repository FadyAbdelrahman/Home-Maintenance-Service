# HomeFix - Home Maintenance Service System

**Student Name:** Fady Abdelrahman  
**Student ID:** 20053593  
**Lecturer:** Dinesh Kanojiya  
**Module:** B8IT1247 – Advanced Web Technologies  
**Academic Year:** 2025/2026

---

## 📋 Project Overview

HomeFix is a full-stack web-based home maintenance service booking system that connects customers with professional service providers. The application allows users to browse services, submit service requests, track their requests by email, and rate completed services.

This project demonstrates the implementation of a modern web application using ASP.NET Core Web API for the backend and vanilla JavaScript for the frontend, with SQLite for data persistence.

---

## 🚀 Technologies Used

### Backend
* **ASP.NET Core 9.0** - Web API framework
* **Entity Framework Core 9.0** - ORM for database access
* **SQLite** - Lightweight file-based database
* **C#** - Server-side programming language

### Frontend
* **HTML5 / CSS3** - Page structure and responsive styling
* **JavaScript (ES6+)** - Dynamic content and user interaction
* **Fetch API** - HTTP communication with the backend

---

## ✨ Key Features Implemented

1. **Service Browsing**: Browse 6 different home maintenance services across multiple categories
2. **Category Filtering**: Filter services by category (Electrical, Plumbing, HVAC, Carpentry, Painting, Appliance)
3. **Service Requests**: Submit service requests with customer details and preferred date
4. **Request Tracking**: Track all service requests using email address
5. **Rating System**: Rate and review completed services with star ratings
6. **Request Management**: Cancel pending requests
7. **Authentication**: Basic login system for user access
8. **RESTful API**: 12 fully functional API endpoints following REST principles
9. **Responsive Design**: Mobile-friendly interface that works on all devices
10. **Data Persistence**: SQLite database with seed data for 6 services

---

## 🏗️ System Architecture

The application follows a **layered architecture** with clear separation of concerns:

### Presentation Layer (Frontend)
- 5 HTML pages (index, services, my-requests, login, contact)
- CSS styling with responsive design
- JavaScript for dynamic content and API communication

### API Layer (Backend)
- **ServicesController**: Manages service CRUD operations (6 endpoints)
- **ServiceRequestsController**: Manages customer requests (6 endpoints)
- ASP.NET Core with Dependency Injection

### Data Access Layer
- Entity Framework Core with AppDbContext
- Two models: Service and ServiceRequest
- Database seeding for initial data

### Database Layer
- SQLite database (homemaintenance.db)
- Two tables with foreign key relationship (1:Many)

---

## 📂 Project Structure
