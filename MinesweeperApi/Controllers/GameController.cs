using Microsoft.AspNetCore.Mvc;
using MinesweeperApi.Models;
using MinesweeperApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MinesweeperApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GameController
  {
    private IGameBoardRepository _gameBoardRepository;

    public GameController(IGameBoardRepository gameBoardRepository)
    {
      _gameBoardRepository = gameBoardRepository;
    }

    [HttpPost("start")]
    public ActionResult<int> Start([FromBody] StartData response)
    {
      try
      {
        if ((response.height * response.width) < response.mines)
          throw new Exception("The number of mines can not be greater than the number of panels");

        return _gameBoardRepository.GetGameBoard(response.height, response.width, response.mines, response.name).Id;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<GameBoard> GetById(int id)
    {
      return _gameBoardRepository.GetGameBoardByIdWithPanels(id);
    }

    [HttpGet("flag/{id}/{x}/{y}")]
    public ActionResult<GameBoard> SetFlag(int id, int x, int y)
    {
      return _gameBoardRepository.FlagPanel(id, x, y);
    }

    [HttpGet("reveal/{id}/{x}/{y}")]
    public ActionResult<GameBoard> Reveal(int id, int x, int y)
    {
      try
      {
        return _gameBoardRepository.RevealPanel(id, x, y);
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet("incompletegames")]
    public ActionResult<List<GameBoard>> GetIncompleteGames()
    {
      try
      {
        return _gameBoardRepository.GetIncompleteGames();
      }
      catch (Exception)
      {
        throw;
      }
    }

    [HttpGet("onresume/{id}")]
    public ActionResult<GameBoard> OnResumeGame(int id)
    {
      try
      {
        var game= _gameBoardRepository.GetGameBoardByIdWithPanels(id);

        if (game ==null)
          throw new Exception("The game doesn´t exist");

        if (game.Status != GameStatus.InProgress)
          throw new Exception("The game is finalized.");

        return game;
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
