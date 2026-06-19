# Lending System

this is a simple inventory management system built with Angular 20 and .NET 8, designed to help users keep track of items they have lent out. The application allows users to add, edit, and delete items, as well as view a list of all items currently lent out.

## Architecture
built with an **Angular 20** frontend and a **.NET 8** backend, this application provides a seamless experience for a lending system. The frontend is designed to be user-friendly and responsive, while the backend is robust and efficient, ensuring smooth data handling and operations.

### Project structure
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