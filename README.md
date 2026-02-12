# Prestige Rentals – Autonomous Car Rental Platform

Prestige Rentals is a full-stack autonomous car rental platform designed to eliminate manual workflows in vehicle reservations, identity validation, and vehicle pickup processes.

The system integrates secure authentication, automated ID verification, and QR-based validation to streamline the entire rental lifecycle.

---

## Overview

Prestige Rentals was built as a scalable, modular system following Clean Architecture and Domain-Driven Design principles.

The goal of the project was to simulate a real-world autonomous rental workflow:

- Secure account creation and authentication
- Role-based access control (RBAC)
- Automated identity verification using OCR
- Vehicle reservation management
- QR-based pickup validation
- Administrative management capabilities

---

## Architecture

The backend follows **Clean Architecture** principles:

- Separation of concerns between Domain, Application, Infrastructure, and Presentation layers
- Dependency inversion and clear boundaries
- Domain-driven modeling for core rental workflows

The frontend is structured modularly using Angular with clear routing, validation logic, and API communication layers.

---

## Tech Stack

### Backend
- .NET 8
- C#
- ASP.NET Core
- Clean Architecture
- Domain-Driven Design (DDD)
- JWT Authentication
- Role-Based Access Control (RBAC)
- PostgreSQL

### Frontend
- Angular 19
- TypeScript
- Reactive Forms
- Route Guards
- HTTP Interceptors

### Integrations
- Tesseract OCR
- OpenCV
- Rasa Chatbot
- QR Code generation & validation

---

## Key Features

### Authentication & Security
- JWT-based authentication
- Role-based authorization
- Secure API communication over HTTPS

### Automated Identity Verification
- OCR-based ID validation using Tesseract + OpenCV
- Pre-processing and text extraction pipeline

### Reservation Workflow
- Vehicle selection and booking system
- Reservation lifecycle management
- QR code generation for secure vehicle pickup validation

### Intelligent Assistance
- Rasa chatbot integration for automated user support

---

## System Design Highlights

- Modular and scalable architecture
- Clear separation between domain logic and infrastructure
- Secure API design with authentication middleware
- Structured database modeling using PostgreSQL
- Full project lifecycle ownership: planning, implementation, testing, and deployment

---

## Learning Outcomes

This project strengthened practical experience in:

- Designing secure REST APIs
- Applying Clean Architecture in real-world scenarios
- Implementing authentication and authorization correctly
- Integrating external AI-based services (OCR and chatbot)
- Managing end-to-end full-stack application development

---

## Status

Academic project completed as a Bachelor's Thesis.
Designed as a production-ready prototype with extensibility in mind.

---

## Author

Raul C. Pirvan  
Software Engineer
