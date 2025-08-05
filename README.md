# Salama Travel Platform

**Salama Travel** is a full-featured travel management platform designed for both customers and administrators. It enables users to browse and purchase visas online, while providing the admin team with powerful tools to manage clients, transactions, and visa orders in real-time.

---

## 🌐 Technologies Used

### Backend:
- **ASP.NET Core Web API**
- **SQL Server** with Stored Procedures
- **Live pricing updates** via direct server communication

### Frontend:
- **React.js**
- **Tailwind CSS**

### Admin Panel:
- **.NET Desktop Application**
- Allows full control over users, balances, and visa requests

---

## 🎯 Core Features

### 🔹 Public Website
- Browse available **Visa types** and prices
- User **Registration** and **Login**
- User **Profile page** with full access to:
  - Visa order history
  - Account balance
  - Request status (Approved / Rejected / Pending)
- **Live pricing updates** directly from the server (non-admins get updated prices without reload)
- Smart error handling and refund system:
  - If a visa request fails for any reason, the **payment is automatically refunded** back to the user's account balance

---

### 🔹 Admin Desktop App
- Built with Windows Forms / WPF
- View and manage:
  - All customers
  - Their **account balances**
  - Visa requests and their **statuses**:
    - Under Review
    - Completed
    - Cancelled
- View **issued visas**, including:
  - Visa type
  - Issue date
  - Expiry date
  - Client name
- Manually update visa statuses or balances
- Control pricing and other settings

---

### 💳 Payment System
- Currently under development
- Planned integration with a secure payment gateway
- Will support **card payments and refunds**

---

## ⚙️ Business Logic Highlights

- **Refund system**:
  - If the visa process fails for any reason (e.g., rejection by external systems), the exact paid amount is **instantly refunded** back to the user.
- **Wallet-based logic**:
  - Users can top up their account, pay for visas using wallet balance, and receive refunds.
- **Role separation**:
  - Admins can access real-time visa pricing
  - Normal users receive synced, updated prices without server overload

---

## 🧩 Database Overview

- Over **30 relational tables** covering:
  - Users
  - Visa Types
  - Transactions
  - Visa Requests
  - Visa Status Logs
  - Notifications
  - Roles, Permissions, and more
- Stored Procedures handle:
  - Balance updates
  - Visa creation
  - Refund operations
  - Filtering/searching for admins

---

## 🛠 Planned Features
- 🔐 Online Payment Integration (in progress)
- 📊 Admin Dashboard with charts and analytics
- 📱 Mobile app version

---

## 📬 Contact

For more information or a demo request, contact the development team.
