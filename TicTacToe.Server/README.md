# TicTacToe.Server

## Requirements

- .NET 8.0
- Either VS code or Visual Studio 2022

## Start project in VS code

Open terminal in VS Code and enter:

```
cd TicTacToe.Server
dotnet build
dotnet run
```

Use `https://127.0.0.1:5001/swagger` or some application like Postman to test the api

## Start project in Visual Studio 2022
1. Open [/TicTacToe.Server/](TicTacToe.Server) into Visual Studio 2022
2. Press play to start the Api
3. Use `https://127.0.0.1:5001/swagger` or some application like Postman to test the api

## How to connect from another device?

Make sure you have allowed the ports to be exposed by your servers firewall. Open port ` 5000` for http and/or open port `5001` for https. You probably are running the C# API server on your device, so that would mean it is only reachable within your local network.

If you don't know your private IP, you can do the following:

Open command prompt and type:
For windows: `ipconfig`
For Linux and Mac: `ifconfig`

You are probably looking for something like `192.168.x.x` or `172.16.x.x` or `10.0.x.x`.

When you are done, make sure to close the ports in the firewall of your device. Safety first.
