using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Models
{
  public class RevealResponse
  {
    public int IdBoard { get; set; }
    public GameStatus Status { get; set; }
    public int AdjacentMines { get; set; }
    public bool IsMine { get; set; }
    public List<Panel> AdjacentsPanels { get; set; }
  }
}
