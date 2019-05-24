using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApi.Models
{
  public class ApplicationDbContext: DbContext
  {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }

    public DbSet<GameBoard> GameBoards { get; set; }
    public DbSet<Panel> Panel { get; set; }
  }
}
