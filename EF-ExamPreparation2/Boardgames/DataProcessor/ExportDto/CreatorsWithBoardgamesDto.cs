using Boardgame.DataProcessor.ExportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType($"Creator")]
    public class CreatorsWithBoardgamesDto
    {
        [XmlElement("CreatorName")]
        public string Name { get; set; }
        [XmlArray("Boardgames")]
        public BoardgameExportDto[] BoardGames { get; set; }

        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount
        {
            get { return BoardGames?.Length ?? 0; }
            set { }
        }
    }
}
