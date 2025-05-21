using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro; // Import TextMeshPro namespace

public class GamePopupManager : MonoBehaviour
{
    [SerializeField] private GameObject Panel; // Scroll View Content for finished games
    [SerializeField] private GameObject Content;   // Scroll View Content for active games
    [SerializeField] private GameObject gameEntryPrefab;      // Prefab for game entries

    void Start()
    {
        // Initialize the panel and content
        Panel.SetActive(false);
    }

    public void ClosePanel()
    {
        Panel.SetActive(false);
        // Clear the content
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OpenPanel(bool showOpenGames)
    {
        Panel.SetActive(true);

        StartCoroutine(FetchGames(showOpenGames));
    }

    private IEnumerator FetchGames(bool showOpenGames)
    {
        // Fetch data from the API
        var apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"/api/games", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var rawData = request.downloadHandler.text;

                Debug.Log(rawData);

                // Parse the JSON response
                var gamesData = JsonUtility.FromJson<GameListWrapper>(rawData);

                //Debug.Log(gamesData);

                PopulateGames(gamesData, showOpenGames);
            }
            else
            {
                Debug.LogError("Failed to fetch games: " + request.error);
            }
        }
    }

    private void PopulateGames(GameListWrapper games, bool showOpenGames)
    {
        // Clear previous entries
        foreach (Transform child in Content.transform)
        {
            Destroy(child.gameObject);
        }

        // Populate Content with entries
        foreach (var game in games.games)
        {
            // Differentiate type of games:
            // Browse Games are active
            // Finished Games aren't active
            string titleText = "";
            if (game.playerTwoId != -1)
            {
                if (game.winnerId != -1)
                {
                    string player1Result = game.winnerId == game.playerOneId ? "won" : "lost";
                    string player2Result = game.winnerId == game.playerTwoId ? "won" : "lost";

                    titleText = $"FINISHED: {game.gameName} [{game.playerOneName} ({player1Result}) vs {game.playerTwoName} ({player2Result})] : {game.board}";
                }
                else
                {
                    titleText = $"ACTIVE: {game.gameName} [{game.playerOneName} vs {game.playerTwoName}] : {game.board}";
                }
            }
            else
            {
                titleText = $"{game.gameName} [{game.playerOneName} vs ?] : {game.board}";
            }

            // Based on Button click, add or skip adding entry
            // "Browse Games" - if showOpenGames equals true and playerTwo is not set (aka -1)
            // or
            // "Active/Finished Games" - if showOpenGames equals false and playerTwo is set (aka -1)
            if ((showOpenGames == true && game.playerTwoId == -1)
                ||
                (showOpenGames == false && game.playerTwoId != -1))
            {
                GameObject entry = Instantiate(gameEntryPrefab, Content.transform);

                TMP_Text title = entry.GetComponentInChildren<TMP_Text>();
                Button joinButton = entry.GetComponentInChildren<Button>();

                title.text = titleText;

                // Set join button based on if there is already a player 2
                if (game.playerTwoId == -1)
                {
                    // Missing player 2, show join button
                    joinButton.gameObject.SetActive(true);
                    joinButton.onClick.AddListener(() => JoinGame(game.id.ToString()));
                }
                else
                {
                    // Active/Finished game, dont show join button
                    joinButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator JoinGame(string gameId)
    {
        Debug.Log("Joining game with ID: " + gameId);
        // Add logic to join the game

        var apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        var playerId = PlayerPrefs.GetString(Config.playerId, "");

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + $"/api/games/{gameId}/join?playerId={playerId}", "", "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Successfully joined game with ID: " + gameId);
                // Add logic to update the UI or navigate to the game screen
            }
            else
            {
                Debug.LogError("Failed to join game: " + request.error);
            }
        }
    }
}
