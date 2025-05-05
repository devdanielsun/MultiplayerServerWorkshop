-- Insert sample players
INSERT INTO players (username) VALUES ('player1');
INSERT INTO players (username) VALUES ('player2');
INSERT INTO players (username) VALUES ('player3');

-- Insert a finished game
-- The board represents a completed game where 'X' has won (e.g., top row filled with 'X')
INSERT INTO games (game_name, board, is_active, winner_id) 
VALUES ('TicTacToe Match 1', 'XXXOO____', FALSE, 1);

-- Insert a player pair for the game
INSERT INTO game_players (game_id, player_one_id, player_two_id, player_turn) 
VALUES (1, 1, 2, NULL);