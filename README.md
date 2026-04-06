# 🛒 Online E-Commerce Web API  

A production-ready E-Commerce RESTful API built using **ASP.NET Core**, following **Onion Layer Architecture** and industry best practices. This project was developed as part of the **Route Academy** ASP.NET course.

---

## 🏗️ Architecture: Onion Layered Architecture

The project is structured into four main layers, following the **Onion Architecture** to ensure that the application core is independent of external concerns:

1.  **Core Layer (Domain & Application):**
    This is the heart of the system, containing everything related to the business domain:
    * **Entities:** Domain models (Product, Order, Category, Brand, etc.).
    * **Service Abstractions:** Interfaces defining the contracts for Business Logic.
    * **Service Implementation:** The actual logic for **Order Processing**, and **Token Generation**.
2.  **Repository Layer (Infrastructure):**
    The Data Access and Infrastructure layer:
    * **Data Context:** Entity Framework Core implementation.
    * **Generic Repository:** Reusable data access patterns.
    * **Unit of Work:** Managing atomic database transactions.
    * **Data Seeding & Migrations:** Handling initial store data and schema updates.
3. **Shared:**
    A cross-cutting project that provides common utilities used by all other layers
    * **Common Models:** Global request/response models.
    * **Custom Exceptions:** Standardized error types.
    * **Constants & Enums:** Shared values used across the solution. 
4.  **API Layer (Presentation):**
    * The outermost layer.
    * Handles HTTP requests, **Controllers**, **DTOs**, and **Middlewares**.

---

## 💎 Design Patterns & Best Practices

* **Result Pattern:**  Used to handle operation responses (Success/Failure) without throwing exceptions, making the API more predictable and performant.
* **Specification Pattern:** For building dynamic queries, filtering, sorting, and pagination.
* **Generic Repository & Unit of Work:** To centralize data logic and manage transactions.
* **Dependency Injection:** For better decoupling and easier testing.

---

## 🛠️ Tech Stack

* **Framework:** .NET Core 8.0
* **Database:** MS SQL Server (EF Core)
* **Caching:** Redis (Used for Basket management)
* **Security:** Identity Framework & JWT Authentication
* **Documentation:** Swagger UI
* **Mapping:** AutoMapper
* **Payment:** Stripe API Integration

---

## 🚀 Main Modules & Endpoints

### 1. Account & Security
* **Authentication:** JWT-based login and registration.
* **Authorization:** Role-based access control.
* **Address Management:** Save and retrieve user shipping addresses.

### 2. Products Module
Handles the product catalog with advanced querying.
* `GET /api/products` - Support for:
    * **Pagination:** (PageSize, PageIndex)
    * **Sorting:** (Price Asc/Desc, Name)
    * **Filtering:** (By Brand or Type)
* `GET /api/products/{id}` - Detailed product view.

### 3. Basket Module (Powered by Redis)
Fast and scalable shopping cart management.
* `GET /api/basket?id={id}` - Retrieve basket.
* `POST /api/basket` - Update items.
* `DELETE /api/basket` - Clear basket after checkout.

### 4. Payment Module (Stripe Integration) 💳
Handles the financial transactions securely.
* **Payment Intent:** Integration with Stripe to create or update payment intents.
* **Security:** Use of Secret Keys and Publishable Keys for secure transactions.
* **Order Sync:** Automatically updates the order status based on payment success or failure.
* `POST /api/payments/{basketId}` - Initialize/Update payment process for a specific basket.

### 5. Order & Checkout
* **Order Creation:** Automated calculation of subtotal, delivery fees, and stock validation.
* **Order History:** Users can track their previous orders and status.
* **Delivery Methods:** Integration of different shipping options with varying costs.
---

# Setup & Installation 

# E-Commerce Solution (.NET 8)

Layered E-Commerce solution using .NET 8, EF Core, Redis.

## Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (or CLI)
- SQL Server
- Redis
- Stripe account


## Setup

1. **Clone Repository**  
   git clone <repo-url>  
   cd <repo-folder>

2. **Restore & Build**  
   dotnet restore  
   dotnet build

3. **Configure Secrets**  
   cd PresentationLayer/E-Commerce.Web  
   dotnet user-secrets init  
   dotnet user-secrets set "JWTOptions:SecretKey" "your-secret-key"  


4. **Update appsettings.Development.json**  
   - ConnectionStrings: DefaultConnection, IdentityConnection, RedisConnection  
   - URLs: BaseUrl  
   - JWTOptions: Issuer / Audience

5. **Apply EF Core Migrations**  
   cd InfrastructureLayer/ECommerce.Persistence  
   dotnet ef database update --project InfrastructureLayer/ECommerce.Persistence --startup-project PresentationLayer/E-Commerce.Web

6. **Run the Application**  
   Visual Studio: F5 or Ctrl+F5  
   CLI: cd PresentationLayer/E-Commerce.Web && dotnet run
