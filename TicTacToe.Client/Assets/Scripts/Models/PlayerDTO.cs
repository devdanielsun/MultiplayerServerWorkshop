using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerDTO
{
    public int id;
    public string username;
}

[System.Serializable]
public class PlayerListWrapper
{
    public List<PlayerDTO> players;
}