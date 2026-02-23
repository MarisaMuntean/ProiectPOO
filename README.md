# ProiectPOO — E-Commerce Application

**ProiectPOO** is a C# desktop application developed using **Object-Oriented Programming (OOP)** principles to simulate an **e-commerce platform**.  
It provides distinct functionalities for two types of users — **Clients** and **Administrators** — enabling product browsing, order management, and stock control in a structured and interactive way.

---

## Project Overview

The application is built as part of an Object-Oriented Programming project and demonstrates the application of inheritance, abstraction, polymorphism, and encapsulation through a real-world scenario — an online store.  
It integrates a **Microsoft Access** database and implements features for both end-users and store administrators.

---

## Roles and Functionalities

### Client
The **Client** can:
- Browse available products  
- Manage the shopping cart:
  - View, add, edit quantities, or remove products  
  - Display full cart content  
- Manage the wishlist:
  - Add products  
  - Delete products  
  - Move products from wishlist to cart  
  - View wishlist items  
- Manage orders:
  - Place new orders  
  - Track order status  
  - Cancel orders still “in processing”  
  - Rate delivered products  
  - View IDs of placed orders  

#### Cart Logic
- When a product is **added**, stock decreases (`Stoc - CantitateProdusCoș`)  
- When a product is **edited**, stock updates (`Stoc - CantitateActualizatăProdus`)  
- When a product is **deleted** or an order is **canceled**, stock increases accordingly  

#### Related Classes
- `MagazinClient` — central class managing the client menu  
- `MetodeCOS`, `MetodeWISHLIST`, `MetodeURMĂRIRE` — implement cart, wishlist, and order-tracking logic  
- `Client` — inherits from `Utilizator`, used in `CreareCont` and `Înregistrare`  
- `CautareClient` — searches database by name/surname without a `Client` object  

---

### Administrator
The **Administrator** manages store operations:
- Manage product database (add, edit, delete, view)
- Manage and track customer orders
- Access and monitor stock information  

#### Related Classes
- `MagazinAdmin` — administrator menu and interface  
- `AdminBD` — methods for managing products in the database  
- `AdminMetodeComanda` — methods for managing customer orders  
- `Admin` — inherits from `Utilizator`, used in `CreareCont` and `Înregistrare`  
- `CautareAdmin` — searches database by name/surname  

---

### Discount System
The **Discount** class hierarchy manages various types of price reductions:
- **Base class:** `Reducere`  
- **Derived classes:**  
  - `DoiPlusUnu` — “Buy 2, get 1 free” type promotion  
  - `DiscountProcent` — percentage-based discount  
  - `DiscountFix` — fixed amount discount  

Each child class overrides the inherited methods to apply the appropriate reduction either to a single product or a product category.

---

### Store Class
- `Magazin` is the central coordination class.  
- Distinguishes between **Client** and **Admin** using the method `IdentitateUtilizator`.  
- The method `DeschidereAplicatie` launches the correct interface (`MagazinAdmin` or `MagazinClient`) based on the user’s role.

---


---

## Technologies Used

- **Visual Studio 2022** — development environment  
- **Microsoft Access** — database backend  
- **C# (.NET Framework)** — programming language  
- **GitHub** — version control and collaboration  
- **Libraries:**  
  - `System.OleDb` — database communication  
  - `iTextSharp` — PDF generation  

---

## Team Organization

- **Marisa Muntean** — Administrator logic and data management  
- **Janina Pârvuleţu** — Client logic and interaction system  
- **Both:** Store structure, shared database integration, testing, and class hierarchy design  

---

## How to Run

### Prerequisites
- .NET Framework installed  
- Visual Studio 2022  
- Microsoft Access Database Engine  

### Steps
1. Clone the repository  
   ```bash
   git clone https://github.com/MarisaMuntean/ProiectPOO.git
