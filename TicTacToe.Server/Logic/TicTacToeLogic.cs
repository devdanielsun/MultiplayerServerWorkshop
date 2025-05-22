namespace TicTacToe.Server.Logic;
public static class TicTacToeLogic
{

    // Helper method to check for winner
    public static int? CheckWinner(string board, int playerOneId, int? playerTwoId)
    {
        int[][] winPatterns = new int[][]
        {
            new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8}, // rows
            new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8}, // cols
            new[] {0,4,8}, new[] {2,4,6}                 // diags
        };

        foreach (var pattern in winPatterns)
        {
            if (board[pattern[0]] != '_' &&
                board[pattern[0]] == board[pattern[1]] &&
                board[pattern[1]] == board[pattern[2]])
            {
                if (board[pattern[0]] == '1') return playerOneId;
                if (board[pattern[0]] == '2') return playerTwoId;
            }
        }
        return null;
    }
}
