using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles; // Array to hold the 9 tile GameObjects of TicTacToe grid

    public Sprite sprite_empty;
    public Sprite sprite_X;
    public Sprite sprite_O;

    public TMP_Text gameNameText;
    public TMP_Text playerOneText;
    public TMP_Text playerTwoText;
    public TMP_Text whoHasTurnText;
    public TMP_Text boardText;
    public TMP_Text isGameActiveText;
    public TMP_Text winnerText;

    private bool initialiseText = true;
    private GameDTO gameData = null;

    // Time variables
    private int interval = 3; // 3 seconds
    private float nextTime = 0;

    // Values of PlayerPrefs
    private string apiUrl = null;
    private string playerId = null;
    private string gameId = null;

    void Start()
    {
        Debug.Log("GameController started!");

        // Ensure there are exactly 9 tiles assigned
        if (tiles.Length != 9)
        {
            Debug.LogError("Please assign exactly 9 tiles in the Inspector.");
            return;
        }

        // Assign click handlers to each tile
        for (int i = 0; i < tiles.Length; i++)
        {
            int tileIndex = i; // Capture the current value of i
            // Ensure each tile has a collider
            if (tiles[tileIndex].GetComponent<BoxCollider2D>() == null)
            {
                tiles[tileIndex].AddComponent<BoxCollider2D>();
            }
            // Ensure each tile has a button
            if (tiles[tileIndex].GetComponent<Button>() == null)
            {
                tiles[tileIndex].AddComponent<Button>();
            }
            // Add click listener to each tile
            tiles[tileIndex].GetComponent<Button>().onClick.AddListener(() => OnTileClicked(tileIndex));
        }

        apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        playerId = PlayerPrefs.GetString(Config.playerId, "");
        gameId = PlayerPrefs.GetString(Config.gameId, "");

        Debug.Log($"API URL: {apiUrl}");
        Debug.Log($"Player ID: {playerId}");
        Debug.Log($"Game ID: {gameId}");

        // Get initial game data from API
        StartCoroutine(GetGameData());
    }

    // Update is called once per frame
    void Update()
    {
        // Exit update() if the gameDate is null or if game is not active
        if (gameData == null || !gameData.isActive)
        {
            return;
        }

        // Check every x seconds for the new game state through API call
        if (Time.time >= nextTime)
        {
            // Update time in seconds
            nextTime += interval;

            // Execute API call every x seconds
            StartCoroutine(GetGameData());
        }
    }


    private IEnumerator GetGameData()
    {
        Debug.Log("Get game data...");

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"/api/games/{gameId}", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            // Check if the API call was successful
            if (request.result == UnityWebRequest.Result.Success)
            {
                var rawData = request.downloadHandler.text;

                Debug.Log(rawData);

                // Parse the JSON response
                gameData = JsonUtility.FromJson<GameDTO>(rawData);

                // Initialise text one
                if (initialiseText)
                {

                    // Set information into text
                    gameNameText.text = $"Game name: {gameData.gameName}";
                    playerOneText.text = $"Player 1 name: {gameData.playerOneName}";
                    playerTwoText.text = $"Player 2 name: {gameData.playerTwoName}";

                    initialiseText = false;
                }

                // After every API request, update dynamic text
                UpdateDynamicTexts();

                // Update the board based on the gameData
                for (int i = 0; i < tiles.Length; i++)
                {
                    // Check the value of each tile in the board
                    char tileValue = gameData.board[i];

                    // Update the tile sprite based on the value
                    if (tileValue == '1')
                    {
                        tiles[i].GetComponent<SpriteRenderer>().sprite = sprite_X;

                        if (gameData.playerOneId.ToString() == playerId)
                            tiles[i].GetComponent<SpriteRenderer>().color = Color.blue; // Set color to blue for player 1
                        else
                            tiles[i].GetComponent<SpriteRenderer>().color = Color.red; // Set color to red for player 2
                    }
                    else if (tileValue == '2')
                    {
                        tiles[i].GetComponent<SpriteRenderer>().sprite = sprite_O;

                        if (gameData.playerTwoId.ToString() == playerId)
                            tiles[i].GetComponent<SpriteRenderer>().color = Color.blue; // Set color to blue for player 2
                        else
                            tiles[i].GetComponent<SpriteRenderer>().color = Color.red; // Set color to red for player 1
                    }
                    else
                    {
                        tiles[i].GetComponent<SpriteRenderer>().sprite = sprite_empty; // Reset to default if empty
                        tiles[i].GetComponent<SpriteRenderer>().color = Color.white; // Set color to red for player 1
                    }
                }
            }
            else
            {
                LoggingHelper.LogApiError("Failed to get game info", request);
            }
        }
    }

    private void UpdateDynamicTexts()
    {
        playerTwoText.text = $"Player 2 name: {gameData.playerTwoName}";

        isGameActiveText.text = gameData.isActive ? "Game is active" : "Game has finished";

        boardText.text = $"Board: {gameData.board}";

        // Calculate name of player who has turn
        string whoHasTurn = gameData.playerTurn == gameData.playerOneId ? gameData.playerOneName : gameData.playerTurn == gameData.playerTwoId ? gameData.playerTwoName : "something went wrong";
        whoHasTurnText.text = $"Turn: {whoHasTurn}";

        if (gameData.winnerId != -1)
        {
            // Calculate name of player who has won
            string winner = gameData.winnerId == gameData.playerOneId ? gameData.playerOneName : gameData.winnerId == gameData.playerTwoId ? gameData.playerTwoName : "something went wrong";
            winnerText.text = $"Winner: {winner}";
        }
        else
        {
            winnerText.text = "";
        }
    }

    // Method called when a tile is clicked
    private void OnTileClicked(int tileIndex)
    {
        Debug.Log($"Tile with index '{tileIndex}' clicked!");

        StartCoroutine(MakeMove(tileIndex));
    }
    
    private IEnumerator MakeMove(int tileIndex)
    {
        /* Make API call to update the game
        * Also keep in mind that you handle the validtion, a move could be illegal
        */
        
        // Create the JSON object for the move

        MoveDTO moveData = new MoveDTO()
        {
            playerId = playerId,
            tileIndex = tileIndex.ToString()
        };

        // Convert the object to JSON string

        string json = JsonUtility.ToJson(moveData);

        Debug.Log($"Making move: {json}");

        // Create UnityWebRequest for POST with JSON body
        using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"/api/games/{gameId}/move", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Move made successfully!");

                // Disable the tile to prevent further clicks
                tiles[tileIndex].GetComponent<BoxCollider2D>().enabled = false; // Disable the collider
                tiles[tileIndex].GetComponent<TileClickHandler>().enabled = false; // Disable the click handler

                StartCoroutine(GetGameData());
            }
            else
            {
                LoggingHelper.LogApiError("Failed to make move", request);
            }
        }
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameByAPICall());
    }

    private IEnumerator ExitGameByAPICall()
    {
        if (gameData.isActive)
        {
            using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"/api/games/{gameId}/giveup/{playerId}", "POST"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                // Check if the API call was successful
                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Reset PlayerPrefs of GameId, and load GameMenu scene
                    PlayerPrefs.DeleteKey(Config.gameId);
                    SceneManager.LoadScene("GameMenu");
                }
                else
                {
                    LoggingHelper.LogApiError("Something went wrong when closing the game", request);
                }
            }
        }
        else
        {
            // Game already finished, so no API call needed to 'give up'
            PlayerPrefs.DeleteKey(Config.gameId);
            SceneManager.LoadScene("GameMenu");
        }
    }
}
