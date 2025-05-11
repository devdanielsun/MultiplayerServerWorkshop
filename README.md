# Multiplayer Server Workshop

This repository contains a Server and Client for hosting and playing a simple Tic Tac Toe game. It serves as a basic demonstration of how a multiplayer game server _can_ be set up. The project was created for a workshop aimed at students in Game and Software Development programs.

## 🔧 Requirements

- Server: [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Server: [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
- Docker, for database container: [Docker](https://www.docker.com/)
- Client: [Unity 2022+](https://unity.com/)
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

3. Maak een scene. Voeg een leeg GameObject toe

4. Maak een UI met 9 knoppen (3x3), elk roept BoardManager.OnCellClicked(index) aan

5.  ...

6.  Search for `Todo` in the code to add the missing code.

## Server - Build and run - the easy way

1. Download [Docker Desktop](https://www.docker.com/products/docker-desktop/)

2. Execute the following command in CLI at the root folder of this repo: `docker compose up -d --build`

Database runs on `127.0.0.1:3306`

Backend runs on `https://127.0.0.1:5001`

Open `https://127.0.0.1:5001/swagger` in your browser, to view the API structure

Opschonen:

```
docker-compose down -v
```

## Server - Build and run - the hard way

Through this way it will be easier to develop the backend.

#### MySQL Database

[TicTacToe.Database/README.md](TicTacToe.Database/README.md)

#### C# .NET API

The backend project is dependent on the MySQL database, without the DB running the run will exit with an error.

[TicTacToe.Server/README.md](TicTacToe.Server/README.md)
