using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Models
{
  public class StartData
  {
    public int width { get; set; }
    public int height { get; set; }
    public int mines { get; set; }
    public string name { get; set; }
  }
}
