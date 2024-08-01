namespace Boardgames.DataProcessor
{
    using Boardgame.DataProcessor.ExportDto;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ValueGeneration;
    using Newtonsoft.Json;
    using System.Net;
    using System.Text.Json.Nodes;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creatorsDto = context.Creators
                .AsEnumerable()
                .Where(c => c.Boardgames.Any())
                .Select(s => new CreatorsWithBoardgamesDto
                {
                    Name = s.FirstName + " " + s.LastName,
                    BoardGames = s.Boardgames.Select(b => new BoardgameExportDto
                    {
                        Name = b.Name,
                        YearPublished = b.YearPublished
                    })
                    .OrderBy(b => b.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.BoardgamesCount)
                .ThenBy(s => s.Name)
                .ToArray();

            return XmlHelper.SerializeToString(creatorsDto, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var creators = context.Sellers
                .Where(s => s.BoardgamesSellers.Any() && s.BoardgamesSellers.Any(bc => bc.Boardgame.YearPublished >= year && bc.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bc => bc.Boardgame.YearPublished >= year && bc.Boardgame.Rating <= rating)
                    .Select(bc => new
                    {
                        Name = bc.Boardgame.Name,
                        Rating = bc.Boardgame.Rating,
                        Mechanics = bc.Boardgame.Mechanics,
                        Category = bc.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(b => b.Rating)
                    .ThenBy(b => b.Name)
                    .ToList()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToList();
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
            string result = JsonConvert.SerializeObject(creators, settings);
            return result;
        }
    }
}