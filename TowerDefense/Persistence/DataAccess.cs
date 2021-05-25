using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Persistence
{
    public class DataAccess : IDataAccess
    {
        
        public async Task<GameState> LoadAsync(String path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    GameState gs = new GameState(1);
                    string line;
                    string[] splitline;
                    for (int i = 0; i < 12; ++i)
                    {
                        for(int j = 0; j < 8; ++j)
                        {
                            line = await reader.ReadLineAsync();
                            splitline = line.Split(' ');
                            gs.Table[i,j].Type = (Entity)Int32.Parse(splitline[0]);
                            gs.Table[i,j].Hp = Int32.Parse(splitline[1]);
                            gs.Table[i,j].Speed = Int32.Parse(splitline[2]);
                            gs.Table[i,j].Damage = Int32.Parse(splitline[3]);
                            gs.Table[i,j].Level = Int32.Parse(splitline[4]);
                            gs.Table[i,j].RateOF = Int32.Parse(splitline[5]);
                        }
                    }
                    line = await reader.ReadLineAsync();
                    splitline = line.Split(' ');
                    gs.ElapsedTime = Int32.Parse(splitline[0]);
                    gs.Gold = Int32.Parse(splitline[1]);
                    gs.RemainingEnemies = Int32.Parse(splitline[2]);
                    gs.WaveChance = Int32.Parse(splitline[3]);
                    gs.CannonAvailable = (Int32.Parse(splitline[4]) == 1 ? true : false);
                    
                    return gs;
                }
            }
            catch
            {
                throw new DataException();
            }
        }
        public async Task SaveAsync(String path, GameState gs)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    for (int i = 0; i < 12; ++i)
                    {
                        for(int j = 0; j < 8; ++j)
                        {
                            await writer.WriteLineAsync(
                                    (int)(gs.Table[i,j].Type) + " "
                                    + gs.Table[i,j].Hp + " "
                                    + gs.Table[i,j].Speed + " "
                                    + gs.Table[i,j].Damage + " "
                                    + gs.Table[i,j].Level + " "
                                    + gs.Table[i,j].RateOF
                                    );
                        }
                    }
                    await writer.WriteLineAsync(
                            gs.ElapsedTime + " "
                            + gs.Gold + " "
                            + gs.RemainingEnemies + " "
                            + gs.WaveChance + " "
                            + (gs.CannonAvailable ? 1 : 0)
                            );
                }
            }
            catch
            {
                throw new DataException();
            }

        }
    }
}
