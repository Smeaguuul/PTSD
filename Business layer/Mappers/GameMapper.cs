using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Business.Mappers
{
    internal static class GameMapper
    {
        public static DTO.Game Map(Game game) // Make one of these classe for every class?
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            return new DTO.Game(game.Server, game.Number, game.PointHistory);
        }

        public static Game Map(DTO.Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            return new Game(game.Server, game.Number, game.PointHistory);
        }
    }
}
