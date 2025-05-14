# Multiplayer Server Workshop

This repository contains a Server and Client for hosting and playing a simple TicTacToe game. It serves as a proof of concept to demonstrate how a basic multiplayer game server _can_ be set up. Note that it lacks best practices, security, and optimizations. The project was created for a workshop targeting students in Game and Software Development programs.

## 🔧 Requirements

- Server: [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Server: [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
- Docker, for database container: [Docker](https://www.docker.com/)
- Client: [Unity Hub](https://unity.com/)
- Client: Unity 6000.1.2f1 (or newer)
- Git (optioneel voor versiebeheer)

## Tech stack:

The stack is built around a turn-based game, Tic Tac Toe, using frameworks I'm familiar with. It’s intended for educational purposes and may not represent best practices for other games or production-ready implementations.

| (Software) Component | Framework/Language |
| -------------------- | ------------------ |
| Client               | Unity 3D           | 
| Backend              | C# .NET RESTful API |
| Database             | MySQL              | 
| Build and Run        | Docker containers for database and backend |

## Client - Unity 3D

Load client project into Unity 3D

1. Start Unity Hub

2. Open folder [/TicTacToe.Client/](TicTacToe.Client)  

3. Open MainMenu.scene

4. Search for `http` in the code to add the missing code.

## Server > backend and database

#### MySQL Database

[TicTacToe.Database/README.md](TicTacToe.Database/README.md)

#### C# .NET API

The backend project is dependent on the MySQL database, without the database running the C#.NET API will exit with an error.

[TicTacToe.Server/README.md](TicTacToe.Server/README.md)
