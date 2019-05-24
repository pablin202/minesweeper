using MinesweeperApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Services
{
  public interface IGameBoardRepository
  {
    GameBoard GetGameBoard(int width, int height, int mines, string name);
    GameBoard GetGameBoardById(int id);
    GameBoard GetGameBoardByIdWithPanels(int id);
    GameBoard RevealPanel(int boardId, int x, int y);
    GameBoard FlagPanel(int boardId, int x, int y);
  }
}
