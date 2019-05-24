# Minesweeper Game Test

## Api

### Swagger

This API implements Swagger to show a description for API

Endpoint -> '/swagger'

### Api descriptions enpoint

#### Start Game

To start a new game, you need to use the verb POST to '/game/start' and send from body the next data: 

{
	"width":10,
	"height": 10,
	"mines": 15,
	"name": "game name"
}

You can configure the size of the panel, choose the number of mines and set a name for the game. This endpoint return the complete game.

The game has 3 status: 1 . In Progress 2. Failed 3. Complete. This values always return in each endpoint.

### Reveal Panel

To reveal a panel, you need to use the verb GET to 'game/reveal/{id}/{x}/{y}'

id parameter is the id of game
x is the horizontal coordinate
y is the vertical coordinate

This enpoint return the game with the panels modified. If the status is equal to 2, you had reveal a mine. If it´s 1, you cant continue.

### Set Flag Panel

To reveal a panel, you need to use the verb GET to 'game/flag/{id}/{x}/{y}'

* id parameter is the id of game
* x is the horizontal coordinate
* y is the vertical coordinate

This enpoint return the game with the panel modified.

### List incomplete games

If you want resume a game, you must use this endpoint: 'incompletegames'.

This endpoint return all games in progress. Then, you can choose one and resume it.

### Resume Game

If you have an ID for some incomplete game, you can use this endpoint to get the panels: 'onresume/{id}'
id parameter is the id of game

This endpoint return the game with theirs panels.











