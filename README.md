# 🎬 Cinema Booking System - Full Stack Project

A comprehensive Cinema Management System featuring a **Spring Boot** backend, a **Vue.js** client for customers with an interactive seat map, and a **C# (Windows Forms)** admin panel for full management control.

## 📂 Project Structure

This is a Monorepo containing three main applications:

* **`cinema-server`**: Java Spring Boot (REST API Backend).
* **`cinema-client`**: Vue.js (Web Frontend for Customers).
* **`cinema-admin`**: C# .NET Windows Forms (Desktop App for Admins).
* **`database`**: SQL scripts for MySQL setup.

---

## ✨ Key Features

* **Interactive Seat Map:** Visual selection of seats (Available/Occupied/Selected) with real-time updates.
* **Admin Control:** Add/Delete Movies, Manage Screenings, and View/Cancel Reservations.
* **Search & Autocomplete:** Smart search for movies in the client app.
* **Robust Backend:** Handles data consistency and prevents double-booking using Spring Data JPA.

---

## 🛠️ Prerequisites

Before running the project, ensure you have the following installed on your machine:

1.  **Database:**
    * [MySQL Server](https://dev.mysql.com/downloads/mysql/)
    * MySQL Workbench or DataGrip (for managing the DB).
2.  **Backend (Java):**
    * [JDK 17 or 21](https://www.oracle.com/java/technologies/downloads/) (Java Development Kit).
    * IntelliJ IDEA (Recommended) or Eclipse.
3.  **Frontend (Vue):**
    * [Node.js (LTS Version)](https://nodejs.org/en/) - Includes `npm`.
4.  **Admin Client (C#):**
    * [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
    * *Note:* The Admin Client requires **Windows OS** to run Windows Forms.

---

## 🚀 How to Run (Step-by-Step)

Please follow this order to ensure everything connects correctly.

### 1. Database Setup 🗄️

1.  Open MySQL Workbench or your preferred SQL tool.
2.  Create the database and tables by running the script located in `database/setup.sql`.
3.  *Quick Setup Command:*
    ```sql
    CREATE DATABASE cinema_db;
    ```
    *(Ensure the tables `movies`, `screenings`, and `reservations` are created as per the project schema).*

### 2. Backend Server (Spring Boot) ☕

1.  Navigate to the server folder:
    ```bash
    cd cinema-server
    ```
2.  Configure Database Credentials:
    * Open `src/main/resources/application.properties`.
    * Ensure `spring.datasource.username` and `spring.datasource.password` match your MySQL installation (Default: `root` / `root`).
3.  Run the application:
    * Via IntelliJ: Open the project and run `CinemaServerApplication.java`.
    * Via Terminal:
        ```bash
        ./mvnw spring-boot:run
        ```
    * *The server will start at `http://localhost:8080`.*

### 3. Customer Client (Vue.js) 🌐

1.  Navigate to the client folder:
    ```bash
    cd cinema-client
    ```
2.  Install dependencies (first time only):
    ```bash
    npm install
    ```
3.  Run the development server:
    ```bash
    npm run dev
    ```
4.  Open the link shown in the terminal (usually `http://localhost:5173`).

### 4. Admin Client (C#) 🖥️

1.  Navigate to the admin folder:
    ```bash
    cd cinema-admin
    ```
2.  Run the application:
    ```bash
    dotnet run
    ```
3.  The Admin Window will open. You can now add movies and schedule screenings.

---

## ⚙️ Configuration Notes

* **CORS:** The Java server is configured to accept requests from `http://localhost:5173`. If your Vue app runs on a different port, update the `CorsConfig.java` file in the backend.
* **Database Reset:** The server is configured with `ddl-auto=update`. If you encounter schema errors, you can switch it to `create` temporarily in `application.properties` to reset the DB, or drop/re-create the database manually.

---

## 🤝 Contribution

Feel free to fork this repository and submit pull requests.

---

**Author:** [Your Name]