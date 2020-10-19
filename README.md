# Assignment_GiannisAntoniou
Unity Snake Game!

# Snake Game

SnakeGame is a 2d game created on currently latest Unity version (2019.4.12f1). 

## Description
This a simple Snake game. The snake grows by a length of 1 every time it eats a food on the map. When game starts, 1 food is spawned randomly across the map and when snake eats the food, a new food is spawned on new random position. The game is over when the snake collides with the edge of the map or itself.

When game starts: 
- There is a check for a custom data file that holds the High Score. If file does not exist (first run of the game), an empty file is created. When user hits new High Score, it is being saved on that file.
- It's loading a .json file and holds food parameters (type, points). 

## Scenes
Game consists of 2 scenes: 
- Main Scene where the game starts. There is a button to start the game and text to display all time High score.
- Game Scene where the actual game is played. It consists of the map, snake, food, current score, streak, and the popup on game over or win with a OK button to load Main Menu Scene.

## Board
Board holds the map of multiple tiles that each tile has it's own position. Tiles are used to hold the random spawned food and the full snake body position.

## Food
Parameters of food are stored in .json file. There are 2 types of food: Mouse and Spider. Food is spawned randomly on board. 

## Scoring
Once the snake's head hits a food, use will get points. Mouse is worth of 10 points and Spider is worth 20 points. There is a streak if snake eats sequentially the same type of food. 
For example: 
 - Eat Mouse - gets 10 points
 - Eat Mouse - gets 20 points (10x2)
 - Eat Mouse - gets 30 points (10x3)
 - Eat Spider - gets 20 points (Streak is reset)
 - Eat Mouse - gets 10 points (Streak is reset)
 - Eat Mouse - gets 20 points (10x2)


```
## Video
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
