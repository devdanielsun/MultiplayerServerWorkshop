using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    public TMP_Text viewIpTextField;
    public TMP_Text viewUsernameTextField;

    // variables that hold the values of the user settings
    // use them as data for the API calls
    private string apiUrl;
    private string username;

    private void Start()
    {
        LoadPlayerPrefs();
    }

    private void LoadPlayerPrefs()
    {
        apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        username = PlayerPrefs.GetString(Config.usernameKey, "");

        viewIpTextField.text = $"Api URL: {apiUrl}";
        viewUsernameTextField.text = $"Username: {username}";
    }

    public void OnNewGameButtonClicked()
    {
        StartCoroutine(CreateNewGame());
    }

    private IEnumerator CreateNewGame() {
        // Call API to create a new game here
        // Fetch data from the API
        var apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        var playerId = PlayerPrefs.GetString(Config.playerId, "");

        Debug.Log("Creating new game...");

        using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"/api/games/create?playerId={playerId}", "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            // Check if the API call was successful
            if (request.result == UnityWebRequest.Result.Success)
            {
                var rawData = request.downloadHandler.text;

                Debug.Log(rawData);

                // Parse the JSON response
                var gameData = JsonUtility.FromJson<GameDTO>(rawData);

                PlayerPrefs.SetString(Config.gameId, gameData.id.ToString());

                // Load the appropriate scene or update UI
                SceneManager.LoadScene("TicTacToeBoard");
            }
            else
            {
                LoggingHelper.LogApiError("Failed to create a new game", request);
            }
        }
    }
}