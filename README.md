# Apartment Management System

## Overview
Apartment Management System is a multi-layered .NET application designed for apartment managers and residents. Apartment managers can manage users, apartments, invoices and payments. Users can view the invoice and dues information assigned to them and make payments for dues or invoices.

## Used Technologies
  - C#
  - ASP.NET Core
  - Entity Framework Core
  - SQL Server
  - JWT Bearer Authentication
  - AutoMapper
  - Hangfire

## Architecture
The project is divided into the following layers:

  - `ApartmentManagementSystem.API`: Handles HTTP requests and provides RESTful API endpoints.
  - `ApartmentManagementSystem.Core`: Contains the business logic with services and DTOs.
  - `ApartmentManagementSystem.Infrastructure`: Comprises database operations and Entity Framework configurations.
  - `ApartmentManagementSystem.Models`: Contains database entities and enums.

## API Usage
The API can be accessed at http://localhost:7228 and tested with Swagger UI.
  
  1. Auths
  2. Users
  3. Apartments
  4. Invoices
  5. Payments
