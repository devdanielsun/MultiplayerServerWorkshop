-- Create the players table
CREATE TABLE players (
    player_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create the games table
CREATE TABLE games (
    game_id INT AUTO_INCREMENT PRIMARY KEY,
    game_name VARCHAR(50) NOT NULL,
    board CHAR(9) NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    winner_id INT DEFAULT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (winner_id) REFERENCES players(player_id) ON DELETE CASCADE
);

-- Create the game_players table to track player pairs in a game
CREATE TABLE game_players (
    game_player_id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    player_one_id INT NOT NULL,
    player_two_id INT DEFAULT NULL,
    player_turn INT DEFAULT NULL,
    FOREIGN KEY (game_id) REFERENCES games(game_id) ON DELETE CASCADE,
    FOREIGN KEY (player_one_id) REFERENCES players(player_id) ON DELETE CASCADE,
    FOREIGN KEY (player_two_id) REFERENCES players(player_id) ON DELETE CASCADE,
    FOREIGN KEY (player_turn) REFERENCES players(player_id) ON DELETE CASCADE
);