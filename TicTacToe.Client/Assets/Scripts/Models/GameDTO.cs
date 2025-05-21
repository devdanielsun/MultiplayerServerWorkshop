using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameDTO
{
    public int id;
    public string gameName;
    public bool isActive;
    public string board;
    public int winnerId;         // Use -1 to represent "no winner"
    public int playerOneId;
    public string playerOneName;
    public int playerTwoId;      // Use -1 to represent "no player two
    public string playerTwoName;
    public int playerTurn;
}

[System.Serializable]
public class GameListWrapper
{
    public List<GameDTO> games;
}
