# Hotel Management System - .NET Solution

This repository contains a full-stack .NET / C# hotel management solution built with a layered, service-oriented architecture. It features a RESTful Web API and database persistence powered by SQL Server.

---

## Solution Architecture

The project is structured into three main layers to ensure separation of concerns, maintainability, and scalability:

1. **HotelAccesoDatos**: Data access and persistence layer responsible for direct communication with SQL Server, entity mapping, and CRUD operations.
2. **HotelServiciosAPI**: RESTful Web API that centralizes business logic and exposes JSON endpoints for client consumption.
3. **HotelWebMVC**: Presentation layer built with ASP.NET Core MVC that consumes services provided by the Web API.

---

## Tech Stack

* **Programming Language:** C# (.NET)
* **Backend & API:** ASP.NET Core Web API
* **Frontend:** ASP.NET Core MVC / HTML5 / CSS3 / JavaScript
* **Database:** Microsoft SQL Server
* **Version Control:** Git & GitHub

---

## Repository Structure

Proyecto3UNED/
├── HotelAccesoDatos/       # Data Access Layer (SQL Server)
├── HotelServiciosAPI/      # RESTful Web API (Endpoints)
├── HotelWebMVC/            # User Interface (MVC Pattern)
├── ScriptProyecto3.sql     # Database creation script and seed data
└── Daniel_Chaves_Proyecto_3.slnx # Visual Studio Solution file

## Prerequisites & Setup

To run this project locally, ensure you have the following installed:

1. **Visual Studio 2022** (with ASP.NET and web development workload).
2. **SQL Server / SQL Server Express** & **SQL Server Management Studio (SSMS)**.
3. **.NET SDK**.

### Getting Started:

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/danielchavesmesa-afk/Proyecto3UNED.git](https://github.com/danielchavesmesa-afk/Proyecto3UNED.git)
Set up the Database:

Open SQL Server Management Studio (SSMS).

Execute the ScriptProyecto3.sql file located in the root directory to generate the database, tables, and sample data.

Configure the Connection String:

Review and update the ConnectionString in the appsettings.json file inside HotelServiciosAPI and/or HotelAccesoDatos to point to your local SQL Server instance.

Run the Application:

Open Daniel_Chaves_Proyecto_3.slnx in Visual Studio.

Set both projects (HotelServiciosAPI and HotelWebMVC) to start simultaneously (or run the API project first, followed by the MVC client).

Build and run the solution (F5).

Author
Daniel Chaves Meza - Computer Engineering Student

GitHub: @danielchavesmesa-afk