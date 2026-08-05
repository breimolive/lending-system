# Lending System

Lending System is a lightweight inventory app built with Angular 20 (frontend) and .NET 8 (backend). It helps users track items they lend out by supporting item creation, editing, deletion, and status tracking. The backend acts as a BFF: it handles authentication, serves static assets, and proxies API requests, while the Angular SPA delivers a
responsive, accessible UI. Designed for simplicity and reliability, the app stores lending records, supports multiple user accounts, and is easy to run locally for development or testing.

# About the Project
## Overview

Lending System is a modern, full-stack web application designed to simplify the management and tracking of borrowed equipment. Built as a vocational qualification exam project, it demonstrates proficiency in contemporary web development practices, including user authentication, real-time data management, and responsive UI design.

## What it Does?
Lending System provides a centralized platform for tracking items that have been loaned out. Users can create comprehensive inventory records, manage borrower information, monitor the status of active loans, and maintain a complete history of lending transactions. The application eliminates the need for manual tracking sheets or spreadsheets, offering a reliable, scalable solution for equipment management.

## Why was it Created?
This project was developed as a capstone exercise for a vocational qualification exam. It showcases essential competencies in full-stack web development, including:
- User authentication and authorization
- Database design and management
- RESTful API development
- Modern frontend frameworks and responsive design
- Cross-layer application architecture

## Target Users
Lending System is designed for:
- Organizations and Teams that regularly loan equipment or resources to employees or clients
- Educational Institutions managing lab equipment, tools, or learning materials
- Equipment Rental Services needing lightweight, accessible management solutions
- Community Groups tracking shared resources among members

## Key Features
- User Authentication: Secure login system with role-based access control
- Equipment Management: Create, edit, and delete inventory items with detailed descriptions
- Loan Tracking: Monitor borrowed items, borrower details, and loan status in real-time
- Multi-User Support: Different user accounts with independent inventory management

# Refactor Overview
## Motivation
While Lending System successfully fulfilled its original purpose as a vocational exam project, it represents an opportunity for continuous improvement and professional growth. Rather than moving on to a new project, I've chosen to use this application as a foundation for mastering full-stack development practices, design principles, and modern technologies. This refactor transforms the project into a reference implementation a portfolio piece that demonstrates not just initial development capabilities, but the discipline and foresight to maintain, improve, and evolve a codebase over time.

## Key Changes
The refactoring initiative encompasses several strategic improvements:

**Architecture & Technology Stack**
- Framework Upgrades: Migrating from Angular 20 to Angular 22 and .NET 8 to .NET 10 to leverage the latest language features, performance improvements, and security patches
- Deprecation Management: Proactively addressing the November 10, 2026 end-of-life date for .NET 8, ensuring the application remains secure and supported
- Code Standardization: Refactoring methods and architectural patterns to follow industry best practices, improving maintainability and scalability

**User Experience & Design**
- Design System: Creating a comprehensive Figma workflow to establish consistent, standardized UI patterns across all pages
- Enhanced User Workflows: Redesigning core interactions (borrowing, returning, managing equipment) based on usability principles
- Responsive Excellence: Ensuring a seamless, polished experience across all device sizes and scenarios

**Feature Expansion**
- Integrating lessons learned from previous CRUD applications
- Adding functionality that provides genuine value while maintaining simplicity
- Building a solid foundation for future enhancements

## Key Learnings
This refactoring journey has reinforced several important lessons:
1. Continuous Improvement is Essential: Investing time in redesign and thoughtful refactoring yields applications that are more maintainable, scalable, and professional
2. Design-First Development: Spending time in Figma before coding saves time and produces more cohesive products
3. Technical Debt Management: Staying current with framework versions and dependencies is an ongoing responsibility, not an afterthought
4. Rewarding Discipline: While refactoring requires significant effort and focus, the satisfaction of transforming a project into something genuinely excellent provides strong motivation for continuous growth as a developer

# Architecture
built with an **Angular 20** frontend and a **.NET 8** backend, this application provides a seamless experience for a lending system. The frontend is designed to be user-friendly and responsive, while the backend is robust and efficient, ensuring smooth data handling and operations.

## Project structure
| Path                      | Description                                                     |
|---------------------------|-----------------------------------------------------------------|
| `Client/`                 | Angular 20 SPA (TypeScript, HTML, CSS)                          |
| `Server/`                 | .NET 8 BFF — authentication, static file serving, reverse proxy |

## Prerequisites

| Tool                | Version |
|---------------------|---------|
| Node.js             | 22.16.0 |
| npm                 | 11.11.1 |
| .NET SDK            | 8.0     |

## Getting Started

Two processes run simultaneously — the .NET host (authentication + proxy) and the Angular dev server.

```bash
# Frontend
cd Client
npm install

# Backend
cd Server
dotnet restore
```

### 2. Run the application

**Terminal 1 — .NET host**
This is all you need to run the backend server, which handles authentication and serves the Angular application.
```bash
cd Server
dotnet run
```

If the terminal 1 does not open the Angular application in the browser automatically, you can start it with the following command in:

**Terminal 2 — Angular dev server**
```bash
cd Client
npm start
```

### 3. Further help
To login to the application, use the following credentials:
- Email: `heine.breimo@gmail.com`
- Password: `123lol123`

secondary test account
- Email: `test@gmail.com`
- Password: `123lol123`
