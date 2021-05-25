using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Persistence
{
    public class GameState
    {
        public Cell[,] Table
        {
            get; set;
        }
        public int ElapsedTime
        {
            get; set;
        }
        public int Gold
        {
            get; set;
        }
        public int RemainingEnemies
        {
            get; set;
        }
        public bool CannonAvailable
        {
            get; set;
        }
        public int WaveChance
        {
            get; set;
        }
        // diff lehet 1=konnyu, 2=kozepes, 3=nehez
        public GameState(int diff)
        {
            Table = new Cell[12, 8];
            // explicit inicializasa a tablanak
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Table[i, j] = new Cell();
                    Table[i, j].CreateCell(Entity.EMPTY);
                }
            }

            // kesobbi munka lesz a gameplay balance-anak beallitasa, teszt ertekek
            ElapsedTime = 0;
            Gold = 1200 - (diff * 200);
            RemainingEnemies = (diff * 20);
            WaveChance = diff * 10;
            CannonAvailable = false;
        }

    }
}
