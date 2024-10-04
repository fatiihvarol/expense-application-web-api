# Expense Management Application

## Overview

This is a full-stack **Expense Management** application designed to streamline the process of submitting, approving, and reimbursing employee expenses. The project uses **React** for the frontend, **MediatR** for CQRS implementation, **MSSQL** as the database, and **JWT tokens** for authentication and authorization. The application supports multiple roles (Employee, Manager, Accountant, Admin) and ensures secure and seamless operations.For mapping profile **AutoMapper** used in the server side.

## Features

- **Frontend**: Built with React for a smooth and dynamic user experience.
- **Backend**: MSSQL database integrated with CQRS design pattern using **MediatR**.
- **Authentication**: JWT token-based authentication with **access** and **refresh tokens**.
- **Authorization**: Role-based access control for different levels (Employee, Manager, Accountant, Admin).
- **Caching**: **In-memory caching** is used for category data to improve performance.
- **Password Security**: Passwords are encrypted using **MD5** for secure storage.
- **AutoMapper**: Used for object-to-object mapping, simplifying the transformation of data between models.

## Key Features

- **User Roles:**
  - **Employee**: Submits expense forms for approval.
  - **Manager**: Approves or rejects submitted expenses.
  - **Accountant**: Finalizes payment of approved expenses.
  - **Admin**: Views all transactions and generates reports.
  
- **Expense Submission and Approval Flow**:
  - Employees can submit expenses (e.g., taxi, food, gas) via a form.
  - Managers approve or reject expenses.
  - Accountants process payments after manager approval.
  
- **JWT Authentication**:
  - JWT tokens are used for user authentication, with role-based access control .
  - **Access Token**: Ensures secure, role-based access to resources.
  - **Refresh Token**: Used to generate new access tokens when the original expires.

- **CQRS Pattern**:
  - The backend follows the CQRS (Command Query Responsibility Segregation) pattern, implemented using **MediatR** for separating read and write operations.

## Prerequisites

Before running this project, ensure you have the following installed:

- **Microsoft Visual Studio (Enterprise Edition)**
- **Microsoft SQL Server (Developer Edition)**
- **SQL Server Management Studio**
- **Node.js** (for React frontend)

## Installation & Setup

### Backend (API)

1. Clone the repository.
2. Configure the connection to **MSSQL** in the `appsettings.json` file.
3. Run the migrations to set up the database schema.
4. Start the API server.

### Frontend (React)

1. Navigate to the frontend folder.
2. Run `npm install` to install dependencies.
3. Start the development server using `npm start`.

## Technologies Used

- **Frontend**: React, HTML, CSS, JavaScript
- **Backend**: ASP.NET Core, MediatR (for CQRS), Entity Framework Core
- **Database**: MSSQL
- **Authentication**: JWT Tokens with Access and Refresh Token handling

## API Endpoints

### Auth

- **POST** `/api/Auth/Login`: User login and JWT token generation.
- **POST** `/api/Auth/Refresh`: Generate a new access token using the refresh token.

### Expense Categories

- **GET** `/api/ExpenseCategories`: Retrieve a list of all expense categories.
- **POST** `/api/ExpenseCategories`: Create a new expense category.

### Expense Form Histories

- **GET** `/api/ExpenseFormHistories/{Id}`: Get the history of a specific expense form by its ID.

### Expense Forms

- **GET** `/api/ExpenseForms/GetMyExpenseForms`: Retrieve a list of the logged-in user's expense forms.
- **GET** `/api/ExpenseForms/{id}`: Retrieve a specific expense form by its ID.
- **PUT** `/api/ExpenseForms/{id}`: Update an existing expense form.
- **DELETE** `/api/ExpenseForms/{id}`: Delete an existing expense form.
- **GET** `/api/ExpenseForms/ByManager`: Retrieve expense forms that require manager approval.
- **GET** `/api/ExpenseForms/ByAccountant`: Retrieve expense forms that require accountant processing.
- **GET** `/api/ExpenseForms/ByAdmin`: Retrieve all expense forms for admin viewing.
- **POST** `/api/ExpenseForms/Create`: Submit a new expense form.
- **PUT** `/api/ExpenseForms/Reject/{id}`: Reject an expense form by ID.
- **PUT** `/api/ExpenseForms/Approve/{id}`: Approve an expense form by ID.
- **PUT** `/api/ExpenseForms/Pay/{id}`: Mark an expense form as paid by ID.
- **GET** `/api/ExpenseForms/GetEmployeeExpenseInfo`: Retrieve employee-specific expense information.


### Reports

- **GET** `/api/Reports/PieChart`: Retrieve a pie chart report of expenses.
- **GET** `/api/Reports/BarChart`: Retrieve a bar chart report of expenses.
- **GET** `/api/Reports/ByStatus`: Retrieve a report of expenses by status.


##

Endpoints-1
![Alt text](https://github.com/user-attachments/assets/d47a77f4-57fa-4a44-9f2b-1391ef57aaf9)

##
Endpoints-2
![Alt text](https://github.com/user-attachments/assets/2da7c1ed-bfb0-4c48-a885-453cce83ef29)

##

## User Roles and Permissions

- **Employee**: Can submit and edit expenses.
- **Manager**: Can approve or reject employee expenses.
- **Accountant**: Can process payments.
- **Admin**: Can view reports and transactions.

## Refresh Token Workflow

- **Access Token Expiration**: When the access token expires, the client sends the refresh token to retrieve a new access token.
- **Security**: The refresh token ensures that the user remains logged in without needing to re-authenticate frequently, while still maintaining secure operations.

## Reports

- **Admin View**: The Admin can view reports summarizing the activities of employees, managers, and accountants. These reports are displayed within the system.

## Screenshots

### Employee Dashboard
![Employee Dashboard](https://github.com/user-attachments/assets/abbb57f5-57a3-457b-8ff5-3349458761d2)

### Employee Expense List
![Employee Expense List](https://github.com/user-attachments/assets/d639b0d8-5ddd-4360-b694-66490aa08d69)

### Employee Submit Expense
![Employee Submit Expense](https://github.com/user-attachments/assets/4574f46e-a4d7-4620-ab0b-4cc442cbeccd)

### Filter Example
![Filter Example](https://github.com/user-attachments/assets/092c8078-f7be-4d20-84e8-4996a6bd10c1)

### Manager Approve Page
![Manager Approve Page](https://github.com/user-attachments/assets/dd18c829-11c7-4913-813f-505b1ac694a3)
### Approve Alert
![Approve Alert](https://github.com/user-attachments/assets/e90dfa15-34ec-43df-9241-2536125e125a)
### Manager Reject Page
![Manager Reject Page](https://github.com/user-attachments/assets/c78fb6f5-fdfb-48c5-b9c3-f04239a629c0)



### Reject Alert Manager
![Reject Alert Manager](https://github.com/user-attachments/assets/6b7c13b4-5029-4806-9811-82c86e67133c)
### Accountant Pay Alert
![Accountant Pay Alert](https://github.com/user-attachments/assets/68837df4-6e04-43d4-b6d1-d58d90e1235a)

### Transaction Chart
![Transaction Chart](https://github.com/user-attachments/assets/9da93826-7f6b-413e-9e77-d44424a4a7ae)



### Admin Transaction History
![Admin Transaction History](https://github.com/user-attachments/assets/b35b5aca-100c-4dd3-ba91-fd1d8e1c2149)



### Bar Chart
![Bar Chart](https://github.com/user-attachments/assets/46a63127-8030-4933-98d9-c1b3d706a378)


### Bar Chart by Status
![Bar Chart by Status](https://github.com/user-attachments/assets/ee95a442-d186-4a35-a4d0-0ef2cf4c9b66)
### Pie Chart
![Pie Chart](https://github.com/user-attachments/assets/abff727c-2a95-494e-b42d-5eeb3b68b06e)
