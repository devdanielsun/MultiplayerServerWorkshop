using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace

public class StartMenuManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public TMP_InputField usernameInputField;
    public Button nextButton;
    public Button clearButton;

    private bool apiOnline = false;

    void Start()
    {
        LoadUserSettings();
    }

    public void OnNextButtonClicked()
    {
        if (string.IsNullOrEmpty(ipInputField.text) || string.IsNullOrEmpty(usernameInputField.text))
        {
            Debug.LogWarning("IP Address or Username cannot be empty.");
            return;
        }
        if (usernameInputField.text.Length < 3 || usernameInputField.text.Length > 20)
        {
            Debug.LogWarning("Username must be at least 3 characters long and no more than 20 characters.");
            return;
        }
        if (!ipInputField.text.StartsWith("http://") && !ipInputField.text.StartsWith("https://"))
        {
            Debug.LogWarning("IP Address must start with http:// or https://");
            return;
        }

        StartCoroutine(RegisterNewPlayer(ipInputField.text, usernameInputField.text));
    }

    public void OnClearButtonClicked()
    {
        ipInputField.text = string.Empty;
        usernameInputField.text = string.Empty;
        PlayerPrefs.DeleteKey(Config.apiUrlKey);
        PlayerPrefs.DeleteKey(Config.usernameKey);
        PlayerPrefs.Save();

        Debug.Log("User settings cleared.");
    }

    private IEnumerator RegisterNewPlayer(string url, string playerName)
    {
        Debug.Log($"Try registering new player with URL: {ipInputField.text} and Username: {usernameInputField.text}");

        // string jsonPayload = JsonUtility.ToJson(new PlayerDTO { id = 0, username = playerName });

        using (UnityWebRequest request = new UnityWebRequest(url + $"/api/players/create?username={playerName}", "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                apiOnline = true;

                string responseText = request.downloadHandler.text;

                if (string.IsNullOrEmpty(responseText))
                {
                    Debug.LogError("Response is empty. Failed to create player.");
                    yield break;
                }

                try
                {
                    var newPLayer = JsonUtility.FromJson<PlayerDTO>(responseText);

                    PlayerPrefs.SetString(Config.apiUrlKey, url);
                    PlayerPrefs.SetString(Config.usernameKey, playerName);
                    PlayerPrefs.SetString(Config.playerId, newPLayer.id.ToString());
                    PlayerPrefs.Save();

                    Debug.Log("Create new player success, or 'loggedin' successfull.");


                    // Load the next scene or perform the next action
                    SceneManager.LoadScene("GameMenu");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to parse response: {ex.Message}");
                }
            }
            else
            {
                apiOnline = false;

                LoggingHelper.LogApiError("Error creating player", request);
            }
        }
    }

    private void LoadUserSettings()
    {
        var apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        var username = PlayerPrefs.GetString(Config.usernameKey, "");

        if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(username))
        {
            return;
        }
        
        Debug.Log($"Loaded API URL: {apiUrl}");
        Debug.Log($"Loaded Username: {username}");

        ipInputField.text = apiUrl;
        usernameInputField.text = username;
    }
}
