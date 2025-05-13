using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles; // Array to hold the 9 tile GameObjects of TicTacToe grid

    private bool isGameFinished = false; // Flag to check if the game is active
    private bool isPlayerTurn = true; // Flag to check if it's the player's turn
    private string winner = null; // Variable to hold the winner's name

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
            int index = i; // Capture the current index for the lambda
            tiles[i].AddComponent<BoxCollider2D>(); // Ensure each tile has a collider
            tiles[i].AddComponent<TileClickHandler>().Initialize(index, OnTileClicked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameFinished)
        {
            return; // Exit if the game is not active, nothing to update
        }

        // Check every second for the new game state through API call
        // This is a placeholder for the actual API call
        if (Time.frameCount % 60 == 0) // Check every second
        {
            CheckGameStateFromAPI();
        }
    }

    // Method called when a tile is clicked
    private void OnTileClicked(int tileIndex)
    {
        Debug.Log($"Tile with index '{tileIndex}' clicked!");

        // Make API call to update the game state

    }

    private void CheckGameStateFromAPI()
    {
        // Placeholder for API call to check game state
        Debug.Log("Checking game state from API...");
        // Update the game state based on the response

        // var result = ...
        // if (result.isGameFinished == true)
        // {
        //     isGameFinished = true;
        //     Debug.Log("Game Over!");
        //     winner = result.Winner;
        // }
    }
}
