-- Insert sample players
INSERT INTO players (username) VALUES ('player1');
INSERT INTO players (username) VALUES ('player2');
INSERT INTO players (username) VALUES ('player3');

-- Insert a sample game
INSERT INTO games (game_name, state, is_active) 
VALUES ('TicTacToe Match 1', '{"board": [["", "", ""], ["", "", ""], ["", "", ""]]}', TRUE);

-- Insert a player pair for the game
INSERT INTO game_players (game_id, player_one_id, player_two_id) 
VALUES (1, 1, 2);