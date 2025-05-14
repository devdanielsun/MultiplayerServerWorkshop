# Multiplayer Server Workshop

This repository contains a Server and Client for hosting and playing a simple TicTacToe game. It serves as a **proof of concept to demonstrate how a basic multiplayer game server _can_ be set up**. Note that it lacks best practices, security, and optimizations. The project was created for a workshop targeting students in Game and Software Development programs.

## 🔧 Requirements

- Server:
    - [VSCode](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
    - [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Database:
    - Docker, for database container: [Docker](https://www.docker.com/)
- Client
    - [Unity Hub](https://unity.com/)
    - Unity 6000.1.2f1 (or newer)
- Git (optional for versioncontrol)

## Tech stack:

The stack is built around a turn-based game, Tic Tac Toe, using frameworks I'm familiar with. It’s intended for educational purposes and may not represent best practices for other games or production-ready implementations.

| (Software) Component | Framework/Language |
| -------------------- | ------------------ |
| Client               | Unity Hub          | 
| Backend              | C# .NET RESTful API |
| Database             | MySQL              | 
| Build and Run        | Docker containers for database and backend |

## Client - Unity

Load client project into Unity.

1. Start Unity Hub

2. Open folder [TicTacToe.Client/](TicTacToe.Client)  

3. Open StartMenu.scene

4. Search for `http` in the code to add the missing code.

## Server > backend and database

#### MySQL Database

[TicTacToe.Database/README.md](TicTacToe.Database/README.md)

#### C# .NET API

The backend project is dependent on the MySQL database, without the database running the C#.NET API will exit with an error.

[TicTacToe.Server/README.md](TicTacToe.Server/README.md)

-----

## What this project lacks in:

- Database setup: The MySQL database runs in a container, which is generally discouraged for production use due to performance, data persistence, and operational concerns.
- Authentication and security: There is no authentication mechanism between the Unity client and the API, leaving the communication vulnerable to unauthorized access or abuse.
- Unsuitable tech stack for real-time games: The use of HTTP APIs and a relational database like MySQL is sufficient for a turn-based game like TicTacToe, but it's not suited for real-time games (e.g., shooters or action games). In such cases, you'd typically use:
    - WebSockets or UDP for low-latency communication
    - In-memory data stores (e.g. Redis) or event-driven backends
- No automated tests: The codebase lacks unit tests, integration tests, or CI/CD pipelines to ensure stability and maintainability.
- Missing rate limiting or API throttling: Without these, the server is vulnerable to abuse or accidental overload.
- ...
