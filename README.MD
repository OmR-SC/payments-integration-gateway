# Payments Integration Gateway 💳

A robust **Payment Gateway API** built with **.NET 8**, designed to simulate real-world banking transaction processing. This project demonstrates high-standard software engineering practices, focusing on scalability, maintainability, and secure data handling.

The architecture follows **Domain-Driven Design (DDD)** and **Clean Architecture** principles to decouple business logic from infrastructure and external concerns.

## 🚀 Key Features

* **Secure Transaction Processing:** Handles payment authorization with strict validation rules.
* **Idempotency Handling:** Prevents duplicate charges for the same order using unique Merchant Order IDs.
* **State Machine Management:** Manages payment lifecycles (Pending → Processing → Approved/Rejected).
* **Auditability:** Full persistence of transaction history for legal and dispute resolution purposes.
* **Clean Architecture:** Strict separation of concerns (Domain, Application, Infrastructure, API).

## 🛠️ Tech Stack & Patterns

* **Core:** .NET 8 (C#)
* **Database:** SQL Server / Entity Framework Core (Code-First)
* **Architecture:** Clean Architecture, Domain-Driven Design (DDD)
* **API:** ASP.NET Core Web API (RESTful)
* **Versioning:** Git
* **Planned Features (Roadmap):** * XML Serialization/Deserialization for Legacy Banking Systems.
    * XSLT Transformations for ISO-8583 simulation.

## 📂 Project Structure

The solution is organized into four layers:

1.  **Domain:** The heart of the system. Contains Entities (Payment), Enums, and Business Exceptions. No external dependencies.
2.  **Application:** Contains Interfaces and Service logic. Defines *what* the system does.
3.  **Infrastructure:** Implements the Interfaces. Handles the Database (EF Core) and external integrations.
4.  **API:** The entry point. Handles HTTP requests, DTO mapping, and dependency injection.

## 🔧 Getting Started

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/OmR-SC/payments-integration-gateway.git
    ```

2.  **Configure Database:**
    Update the connection string in `appsettings.json` to point to your SQL Server instance.

3.  **Run Migrations:**
    Apply the Code-First migrations to create the database schema:
    ```bash
    dotnet ef database update --project Infrastructure --startup-project API
    ```

4.  **Run the Application:**
    ```bash
    dotnet run --project API
    ```

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).