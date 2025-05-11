# TicTacToe.Database

Start database seperatly

1. Install Docker [https://www.docker.com/get-started/](https://www.docker.com/get-started/)
2. Open a Command Prompt and go to the folder `TicTacToe.Database` 

```
docker build -t tic-tac-toe-db .
docker run -d -p 3306:3306 --name tic-tac-toe-db tic-tac-toe-db
docker container list
```

If container already exists:

```
docker container stop tic-tac-toe-db
docker container rm tic-tac-toe-db
```
