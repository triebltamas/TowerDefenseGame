using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Persistence
{
    public interface IDataAccess
    {
        Task<GameState> LoadAsync(String path);
        Task SaveAsync(String path, GameState gs);
    }
}
