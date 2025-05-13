using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace

public class StartMenuManager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public TMP_InputField usernameInputField;
    public Button nextButton;
    public Button clearButton;

    void Start()
    {
        LoadUserSettings();

        nextButton.onClick.AddListener(OnNextButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        var savedUserSettings = SaveUserSettings();

        if (!savedUserSettings)
        {
            Debug.LogWarning("Failed to save user settings.");
            return;
        }

        // Load the next scene or perform the next action
        SceneManager.LoadScene("GameMenu");
    }

    private void OnClearButtonClicked()
    {
        ipInputField.text = string.Empty;
        usernameInputField.text = string.Empty;
        PlayerPrefs.DeleteKey(Config.apiUrlKey);
        PlayerPrefs.DeleteKey(Config.usernameKey);
        PlayerPrefs.Save();

        Debug.Log("User settings cleared.");
    }

    /*
        * This method is called when the user clicks the "Next" button.
        * It saves the user settings and returns if that was succesfull.
        */
    private bool SaveUserSettings()
    {
        if (string.IsNullOrEmpty(ipInputField.text) || string.IsNullOrEmpty(usernameInputField.text))
        {
            Debug.LogWarning("IP Address or Username cannot be empty.");
            return false;
        }
        if (usernameInputField.text.Length < 3 || usernameInputField.text.Length > 20)
        {
            Debug.LogWarning("Username must be at least 3 characters long and no more than 20 characters.");
            return false;
        }

        PlayerPrefs.SetString(Config.apiUrlKey, ipInputField.text);
        PlayerPrefs.SetString(Config.usernameKey, usernameInputField.text);
        PlayerPrefs.Save();

        return true;
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
