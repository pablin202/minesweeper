using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Models
{
  public class GameBoard
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int MineCount { get; set; }
    public ICollection<Panel> Panels { get; set; }
    public GameStatus Status { get; set; }
    public string Name { get; set; }
    public long DateCreated { get; set; }

    public List<Panel> GetNeighbors(int x, int y)
    {
      return GetNeighbors(x, y, 1);
    }

    public List<Panel> GetNeighbors(int x, int y, int depth)
    {
      var nearbyPanels = Panels.Where(panel => panel.X >= (x - depth) && panel.X <= (x + depth)
                                           && panel.Y >= (y - depth) && panel.Y <= (y + depth));
      var currentPanel = Panels.Where(panel => panel.X == x && panel.Y == y);
      return nearbyPanels.Except(currentPanel).ToList();
    }

    public void RevealPanel(int x, int y)
    {
      //Step 1: Find the Specified Panel

      var selectedPanel = Panels.First(panel => panel.X == x && panel.Y == y);
      selectedPanel.IsRevealed = true;
      selectedPanel.IsFlagged = false; //Revealed panels cannot be flagged
                                       //      response.IsMine = selectedPanel.IsMine;
      //Step 2: If the panel is a mine, game over!
      if (selectedPanel.IsMine) Status = GameStatus.Failed; //Game over!

      //Step 3: If the panel is a zero, cascade reveal neighbors
      if (!selectedPanel.IsMine && selectedPanel.AdjacentMines == 0)
      {
        RevealZeros(x, y);
      }

      //Step 4: If this move caused the game to be complete, mark it as such
      if (!selectedPanel.IsMine)
      {
        CompletionCheck();
      }
    }

    public void FirstMove(int x, int y, Random rand)
    {
      //For any board, take the user's first revealed panel + any neighbors of that panel to X depth, and mark them as unavailable for mine placement.
      var depth = 0.125 * Width;
      var neighbors = GetNeighbors(x, y, (int)depth); //Get all neighbors to specified depth
      neighbors.Add(GetPanel(x, y));

      //Select random panels from set which are not excluded
      var mineList = Panels.Except(neighbors).OrderBy(user => rand.Next());
      var mineSlots = mineList.Take(MineCount).ToList().Select(z => new { z.X, z.Y });

      //Place the mines
      foreach (var mineCoord in mineSlots)
      {
        Panels.Single(panel => panel.X == mineCoord.X && panel.Y == mineCoord.Y).IsMine = true;
      }

      //For every panel which is not a mine, determine and save the adjacent mines.
      foreach (var openPanel in Panels.Where(panel => !panel.IsMine))
      {
        var nearbyPanels = GetNeighbors(openPanel.X, openPanel.Y);
        openPanel.AdjacentMines = nearbyPanels.Count(z => z.IsMine);
      }
    }

    public void RevealZeros(int x, int y)
    {
      var neighborPanels = GetNeighbors(x, y).Where(panel => !panel.IsRevealed);
      foreach (var neighbor in neighborPanels)
      {
        neighbor.IsRevealed = true;
        if (neighbor.AdjacentMines == 0)
        {
          RevealZeros(neighbor.X, neighbor.Y);
        }
      }
    }

    private void CompletionCheck()
    {
      var hiddenPanels = Panels.Where(x => !x.IsRevealed).Select(x => x.Id);
      var minePanels = Panels.Where(x => x.IsMine).Select(x => x.Id);
      if (!hiddenPanels.Except(minePanels).Any())
      {
        Status = GameStatus.Completed;
      }
    }

    public BoardStats GetStats()
    {
      BoardStats stats = new BoardStats();

      stats.Mines = Panels.Count(x => x.IsMine);
      stats.FlaggedMinePanels = Panels.Count(x => x.IsMine && x.IsFlagged);

      stats.PercentMinesFlagged = Math.Round((double)(stats.FlaggedMinePanels / stats.Mines) * 100, 2);

      stats.TotalPanels = Panels.Count;
      stats.PanelsRevealed = Panels.Count(x => x.IsFlagged || x.IsRevealed);

      stats.PercentPanelsRevealed = Math.Round((double)(stats.PanelsRevealed / stats.TotalPanels) * 100, 2);

      return stats;
    }

    public Panel GetPanel(int x, int y)
    {
      return Panels.First(z => z.X == x && z.Y == y);
    }

    public Panel FlagPanel(int x, int y)
    {
      var panel = Panels.Where(z => z.X == x && z.Y == y).First();
      if (!panel.IsRevealed)
      {
        panel.IsFlagged = true;
      }

      return panel;
    }
  }

}
