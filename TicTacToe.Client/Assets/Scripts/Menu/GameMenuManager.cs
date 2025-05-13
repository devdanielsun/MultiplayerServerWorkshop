using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    public TMP_Text viewIpTextField;
    public TMP_Text viewUsernameTextField;
    public Button newGameButton;
    public Button browseGamesButton;
    public Button finishedGamesButton;

    // variables that hold the values of the user settings
    // use them as data for the API calls
    private string apiUrl;
    private string username;

    private void Start()
    {
        LoadPlayerPrefs();
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        browseGamesButton.onClick.AddListener(OnBrowseGamesButtonClicked);
        finishedGamesButton.onClick.AddListener(OnFinishedGamesButtonClicked);
    }

    private void LoadPlayerPrefs()
    {
        apiUrl = PlayerPrefs.GetString(Config.apiUrlKey, "");
        username = PlayerPrefs.GetString(Config.usernameKey, "");

        viewIpTextField.text = $"Api URL: {apiUrl}";
        viewUsernameTextField.text = $"Username: {username}";
    }

    private void OnNewGameButtonClicked()
    {
        // Call API to create a new game here

        // Check if the API call was successful

        // Load the appropriate scene or update UI
        SceneManager.LoadScene("TicTacToeBoard");
    }

    private void OnBrowseGamesButtonClicked()
    {
        // Call API to retrieve active games here

        // Check if the API call was successful

        // Open overlay panel

        // View active games in the overlay panel

    }

    private void OnFinishedGamesButtonClicked()
    {
        // Call API to retrieve finished games here

        // Check if the API call was successful

        // View finished games in the overlay panel

    }
}