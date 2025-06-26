using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints
{
    public static class GamesEndpoints
    {
        const String GetGameEndPointName = "GetGame";


        private static readonly List<GameDto> games = [
            new(1, "Assassins Creed 2", "Adventure", 19.9M, new DateOnly(2010, 2, 1)),
            new(2, "The Witcher 3", "RPG", 29.9M, new DateOnly(2015, 5, 19)),
            new(3, "God of War", "Action", 39.9M, new DateOnly(2018, 4, 20)),
            new(4, "Red Dead Redemption 2", "Adventure", 49.9M, new DateOnly(2018, 10, 26)),
            new(5, "Hollow Knight", "Platformer", 14.99M, new DateOnly(2017, 2, 24)),
            new(6, "Cyberpunk 2077", "RPG", 59.99M, new DateOnly(2020, 12, 10)),
            new(7, "Stardew Valley", "Simulation", 9.99M, new DateOnly(2016, 2, 26)),
            new(8, "Elden Ring", "Action RPG", 59.99M, new DateOnly(2022, 2, 25)),
            new(9, "Minecraft", "Sandbox", 26.95M, new DateOnly(2011, 11, 18)),
            new(10, "Among Us", "Party", 4.99M, new DateOnly(2018, 6, 15)),
            new(11, "Overwatch", "Shooter", 39.99M, new DateOnly(2016, 5, 24))];

        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {

            var group = app.MapGroup("games").WithParameterValidation();

            // GET /games
            group.MapGet("/", () => games);

            // GET /games/1
            group.MapGet("/{id}", (int id) =>
            {
                GameDto? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);

            }).WithName(GetGameEndPointName);

            // POST /games

            group.MapPost("/", (CreateGameDto newGame) =>
            {
                //if (string.IsNullOrEmpty(newGame.Name))
                //{
                //  return Results.BadRequest("Name is Required");
                //}



                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                    );
                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
            });

            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                    );

                return Results.NoContent();
            });

            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });

            return group;
        }



    }
}
