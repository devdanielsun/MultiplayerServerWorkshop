# Multiplayer Server Workshop

This repository contains a Server and Client for hosting and playing a simple Tic Tac Toe game. It serves as a basic demonstration of how a multiplayer game server _can_ be set up. The project was created for a workshop aimed at students in Game and Software Development programs.

## Tech stack:

The stack is built around a turn-based game, Tic Tac Toe, using frameworks I’m familiar with. It’s intended for educational purposes and may not represent best practices for other games or production-ready implementations.

| (Software) Component | Framework/Language |
| -------------------- | ------------------ |
| Client               | Unity 3D           | 
| Backend              | C# .NET RESTful API |
| Database             | MySQL              | 
| Build and Run        | Docker containers for database and backend |

## Client - Unity 3D

Load client project into Unity 3D

```
...
```

## Server - Build and run - the easy way

1. Download [Docker Desktop](https://www.docker.com/products/docker-desktop/)

2. Execute the following command in CLI at the root folder of this repo: `docker compose up -d`

Database runs on `127.0.0.1:3306`

Backend runs on `127.0.0.1:8080`

Open `127.0.0.1:8080/swagger` in your browser, to view the API structure

## Server - Build and run - the hard way

Through this way it will be easier to develop the backend.

#### MySQL Database

```
cd database
docker build -t mysql_database .
docker run -d -p 127.0.0.1:3306:3306 mysql_database
```

Database runs on `127.0.0.1:3306`

#### C# .NET API

The backend project is dependent on the MySQL database, without the DB running the run will exit with an error.

```
cd tictactoe.Server
dotnet build
dotnet run
```

Then open `127.0.0.1:8080/swagger` in your browser
