using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Models
{
  public class Panel
  {
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsMine { get; set; }
    public int AdjacentMines { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsFlagged { get; set; }

    public Panel(int x, int y)
    {
      X = x;
      Y = y;
    }
  }
}
