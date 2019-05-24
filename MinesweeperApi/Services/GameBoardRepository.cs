using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinesweeperApi.Models;

namespace MinesweeperApi.Services
{
  public class GameBoardRepository : IGameBoardRepository
  {

    private readonly ApplicationDbContext _dbContext;

    public GameBoardRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public GameBoard GetGameBoardById(int id)
    {
      return _dbContext.GameBoards.FirstOrDefault(m => m.Id == id);
    }

    public GameBoard GetGameBoardByIdWithPanels(int id)
    {
      return _dbContext.GameBoards.Include(p => p.Panels).FirstOrDefault(m => m.Id == id);
    }

    public GameBoard GetGameBoard(int width, int height, int mines, string name)
    {
      var gameBoard = new GameBoard();
      gameBoard.Id = 0;
      gameBoard.Width = width;
      gameBoard.Height = height;
      gameBoard.MineCount = mines;
      gameBoard.Panels = new List<Panel>();
      gameBoard.Name = name;
      gameBoard.DateCreated = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

      for (int i = 1; i <= height; i++)
      {
        for (int j = 1; j <= width; j++)
        {
          gameBoard.Panels.Add(new Panel(j, i));
        }
      }

      gameBoard.Status = GameStatus.InProgress;

      Random rand = new Random();

      var randomX = rand.Next(1, gameBoard.Width - 1);
      var randomY = rand.Next(1, gameBoard.Height - 1);

      gameBoard.FirstMove(randomX, randomY, rand);

      try
      {
        _dbContext.GameBoards.Add(gameBoard);
        _dbContext.SaveChanges();
        return gameBoard;
      }
      catch (Exception)
      {
        throw new Exception();
      }

    }

    public GameBoard RevealPanel(int boardId, int x, int y)
    {
      try
      {

        var game = GetGameBoardByIdWithPanels(boardId);
        game.RevealPanel(x, y);

        var listPanels = new List<Panel>();
       
        foreach (var item in game.Panels)
        {
          if (_dbContext.Entry<Panel>(item).State == EntityState.Modified)
          {
            listPanels.Add(item);
          }
        }

        _dbContext.Update(game);
        _dbContext.SaveChanges();

        game.Panels = listPanels;

        return game;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public GameBoard FlagPanel(int boardId, int x, int y)
    {
      try
      {

        var game = GetGameBoardByIdWithPanels(boardId);
        var panel = game.FlagPanel(x, y);
        _dbContext.Update(game);
        _dbContext.SaveChanges();

        var listPanels = new List<Panel>();
        listPanels.Add(panel);
        game.Panels = listPanels;

        return game;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public List<GameBoard> GetIncompleteGames() => _dbContext.GameBoards.Where(x => x.Status == GameStatus.InProgress).ToList();

  }
}
