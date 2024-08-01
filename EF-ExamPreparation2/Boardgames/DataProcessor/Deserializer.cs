namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var creatorsDto = XmlHelper.DeserializeFromString<CreatorDto[]>(xmlString, "Creators").ToArray();
            StringBuilder sb = new StringBuilder();
            var creatorsToBeAdded = new List<Creator>();

            foreach (var c in creatorsDto)
            {
                if (!IsValid(c))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<Boardgame> boardgames = new();
                foreach (var b in c.Boardgames)
                {
                    if (!IsValid(b))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    boardgames.Add(new()
                    {
                        Name = b.Name,
                        Rating = b.Rating,
                        YearPublished = b.YearPublished,
                        CategoryType = (CategoryType)b.CategoryType,
                        Mechanics = b.Mechanics
                    });
                }
                var creator = new Creator()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Boardgames = boardgames
                };
                creatorsToBeAdded.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, c.FirstName, c.LastName, boardgames.Count()));
            }
            context.Creators.AddRange(creatorsToBeAdded);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            SellerDto[]? sellersDto = JsonConvert.DeserializeObject<SellerDto[]>(jsonString);
            List<Seller> sellersToBeAdded = new List<Seller>();

            StringBuilder sb = new StringBuilder();
            foreach (SellerDto s in sellersDto)
            {
                if (!IsValid(s))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<BoardgameSeller> validBoardgamesSellers = new();
                Seller seller = new Seller()
                {
                    Name = s.Name,
                    Address = s.Address,
                    Country = s.Country,
                    Website = s.Website,
                };
                foreach (int id in s.Boardgames.Distinct())
                {
                    Boardgame boardgame = context.Boardgames.Find(id);
                    if (boardgame is null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    validBoardgamesSellers.Add(new BoardgameSeller()
                    {
                        Seller = seller,
                        Boardgame = boardgame
                    });
                }
                seller.BoardgamesSellers = validBoardgamesSellers;
                sellersToBeAdded.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count()));
            }
            context.Sellers.AddRange(sellersToBeAdded);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
