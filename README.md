# PaolaTravel

A full-stack web application designed to manage travel itineraries, bookings, and customer information for a fictional travel agency. This project showcases a complete web development solution built entirely within the **.NET ecosystem**, featuring an **ASP.NET Core Web API backend (`Travel.API`)**, an **ASP.NET Core MVC (or Razor Pages) frontend (`Travel.Web`)**, and a **SQL Server database**.

This application is an excellent example for understanding how to integrate different components of a .NET web solution, from data persistence to API communication and UI rendering, all under one roof.

---

## Table of Contents

* [Features](#features)
* [Technologies Used](#technologies-used)
* [Architecture](#architecture)
* [Getting Started](#getting-started)
    * [Prerequisites](#prerequisites)
    * [Database Setup](#database-setup)
    * [Backend Setup & Run (`Travel.API`)](#backend-setup--run-travelapi)
    * [Frontend Setup & Run (`Travel.Web`)](#frontend-setup--run-travelweb)
* [Project Structure](#project-structure)
* [API Endpoints (Backend)](#api-endpoints-backend)
* [How to Contribute](#how-to-contribute)
* [License](#license)
* [Contact](#contact)

---

## Features

* **Customer Management:** Add, view, update, and delete customer records.
* **Booking Management:** Handle travel bookings, associating them with customers and destinations.
* **Destination Management:** Manage available travel destinations.
* **Seamless Integration:** Direct and efficient communication between the ASP.NET Core frontend and backend.
* **Database Integration:** Persistent storage of all application data in a SQL Server database.

---

## Technologies Used

### Frontend (`Travel.Web`)

* **ASP.NET Core MVC / Razor Pages:** For building dynamic web pages with server-side rendering and interactive UI.
* **C#:** The primary programming language.
* **HTML5 / CSS3 / JavaScript:** For structuring, styling, and client-side interactivity.
* **Bootstrap (or similar framework):** For responsive and modern UI styling.

### Backend (`Travel.API`)

* **ASP.NET Core 8 (or later):** A cross-platform, high-performance framework for building modern, cloud-enabled, Internet-connected applications.
* **C#:** The primary programming language.
* **Entity Framework Core:** An object-relational mapper (ORM) that enables .NET developers to work with a database using .NET objects.
* **ASP.NET Core Web API:** For creating RESTful HTTP services.
* **Swagger/OpenAPI:** For API documentation and testing.

### Database

* **SQL Server:** A robust relational database management system.

---

## Architecture

The PaolaTravel application employs a **layered architecture** within the .NET ecosystem:

1.  **Frontend (`Travel.Web` - Presentation Layer):**
    * An ASP.NET Core project utilizing **MVC** (Model-View-Controller) or **Razor Pages** to render web pages.
    * It handles user interface logic, routing, and directly calls the backend API to retrieve and send data.

2.  **Backend (`Travel.API` - Application/API Layer):**
    * An ASP.NET Core Web API project.
    * It exposes **RESTful endpoints** for the `Travel.Web` frontend (and potentially other clients) to consume.
    * This layer contains business logic, data validation, and manages all communication with the database.

3.  **Database (Data Layer):**
    * A **SQL Server** instance that stores all application data (customers, bookings, destinations).
    * It's accessed by the backend via **Entity Framework Core**.

This setup ensures a clean separation, allowing both the frontend and backend to evolve independently while benefiting from the unified .NET development environment.

---

## Getting Started

Follow these instructions to set up and run the PaolaTravel application on your local machine.

### Prerequisites

Before you begin, ensure you have the following installed:

* **Visual Studio 2022 (or later):** With the "ASP.NET and web development" workload installed.
* **.NET SDK 8.0 (or later):** [Download from Microsoft](https://dotnet.microsoft.com/download)
* **SQL Server Instance:** You'll need a running SQL Server instance (e.g., SQL Server Express, SQL Server Developer Edition, or a localdb instance that comes with Visual Studio).
* **SQL Server Management Studio (SSMS) or Azure Data Studio:** For running SQL scripts and managing your database.

### Database Setup

1.  **Open SQL Server Management Studio (SSMS) or Azure Data Studio.**
2.  **Connect to your SQL Server instance.**
3.  **Create a new database:**
    * Right-click on "Databases" and select "New Database...".
    * Name the new database `PaolaTravelDB`. Click "OK".
4.  **Execute the SQL Schema Script:**
    * Open the `database/scripts.sql` file from the cloned repository in SSMS/Azure Data Studio.
    * Ensure the selected database context in the toolbar is `PaolaTravelDB`.
    * Execute the script (`F5` or "Execute" button) to create the necessary tables and populate initial data.

    *(Optional: If your backend uses Entity Framework Core Migrations, you could alternatively update the database via the backend project after configuring the connection string, typically using `dotnet ef database update` from the backend project directory.)*

### Backend Setup & Run (`Travel.API`)

1.  **Open the Solution in Visual Studio:**
    * Navigate to the root of the cloned repository and open `PaolaTravel.sln`.
2.  **Configure Database Connection:**
    * In Solution Explorer, find the `Travel.API` project.
    * Open `Travel.API/appsettings.json` (and `appsettings.Development.json`).
    * Locate the `ConnectionStrings` section.
    * Update the `DefaultConnection` string to point to your SQL Server instance.
        * **Example for LocalDB (Common with Visual Studio):**
            ```json
            "ConnectionStrings": {
              "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PaolaTravelDB;Trusted_Connection=True;MultipleActiveResultSets=true"
            }
            ```
        * **Example for SQL Server Express:**
            ```json
            "ConnectionStrings": {
              "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PaolaTravelDB;Trusted_Connection=True;MultipleActiveResultSets=true"
            }
            ```
        * *Adjust `YourServerName` and `YourInstanceName` as per your setup.*
3.  **Build the Backend Project:**
    * Right-click on the `Travel.API` project in Solution Explorer and select "Build".
4.  **Run the Backend API:**
    * Right-click on the `Travel.API` project and select "Debug" > "Start New Instance".
    * Alternatively, set `Travel.API` as the startup project and press `F5` (or click the "Start" button).
    The backend API will typically start on `https://localhost:7216` (or a similar port). You'll see messages in your terminal/output window indicating the URLs where the API is listening.
    * **Open the Swagger UI:** Navigate to `https://localhost:7216/swagger/index.html` in your browser to view and test the API endpoints.

### Frontend Setup & Run (`Travel.Web`)

1.  **Configure API Base URL (if necessary):**
    * The `Travel.Web` frontend needs to know where the `Travel.API` backend is running. This is typically configured in `appsettings.json` within the `Travel.Web` project or directly in its code.
    * Verify that `Travel.Web/appsettings.json` points to the correct backend API URL (e.g., `https://localhost:7216`).
2.  **Set as Startup Project:**
    * In Visual Studio, right-click on the `Travel.Web` project in Solution Explorer.
    * Select "Set as Startup Project".
3.  **Run the Frontend Application:**
    * Press `F5` (or click the "Start" button) in Visual Studio.
    * Visual Studio will build and launch the `Travel.Web` application in your default web browser, typically at `https://localhost:7134` (or a similar port).

Now you should have both your ASP.NET Core API backend and your ASP.NET Core web frontend running, with the frontend communicating seamlessly with the backend to manage your travel data!

---

## Project Structure
PaolaTravel/
├── backend/                  # ASP.NET Core Web API Project (Travel.API)
│   ├── Controllers/          # API endpoint definitions
│   ├── Entities/             # Database model classes
│   ├── Data/                 # Database context and migration files (if EF Core Migrations are used)
│   ├── appsettings.json      # Backend configuration (e.g., database connection string)
│   └── Travel.API.csproj     # Backend project file
├── frontend/                 # This folder contains the frontend (Travel.Web)
│   ├── Travel.Web/           # ASP.NET Core Web Application Project (Travel.Web)
│   │   ├── Controllers/      # MVC controllers (if using MVC)
│   │   ├── Pages/            # Razor Pages (if using Razor Pages)
│   │   ├── Views/            # MVC Views (if using MVC)
│   │   ├── Models/           # Models for the frontend
│   │   ├── wwwroot/          # Static files (CSS, JS, images)
│   │   ├── appsettings.json  # Frontend configuration (e.g., API base URL)
│   │   └── Travel.Web.csproj # Frontend project file
└── database/                 # SQL Server Database Scripts
└── scripts.sql           # SQL script to create database schema and populate data


---

## API Endpoints (Backend)

The `Travel.API` backend provides a set of RESTful API endpoints. Once the backend is running, you can explore them via **Swagger UI** at `https://localhost:7216/swagger/index.html` (the port may vary based on your Visual Studio setup).

Example Endpoints:

* `/api/customers`
* `/api/bookings`
* `/api/destinations`

---

## How to Contribute

Contributions are highly welcome! If you have suggestions for improvements, new features, or bug fixes, please consider:

1.  Forking the repository.
2.  Creating a new branch (`git checkout -b feature/your-feature-name`).
3.  Making your changes.
4.  Commit your changes (`git commit -m 'Add some feature'`).
5.  Push to the branch (`git push origin feature/your-feature-name`).
6.  Opening a Pull Request with a clear description of your changes.

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Contact

If you have any questions or feedback about this project, please feel free to reach out:

* **Your GitHub Profile:** [https://github.com/dcaric](https://github.com/dcaric)
* **Your Website:** [www.dcapps.net](http://www.dcapps.net)
