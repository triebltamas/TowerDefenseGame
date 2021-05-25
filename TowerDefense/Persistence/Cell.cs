using System;
using System.Collections.Generic;
using System.Text;

namespace TowerDefense.Persistence
{
    public enum Entity {
        ORC,
        TOWER,
        SHOOTER,
        MINER,
        TRAP,
        CANNON,
        EMPTY
    }

    public class Cell
    {
        //mezők inicializásása
        public void CreateCell(Entity entity)
        {
            this.Type = entity;
            this.Level = 1;

            this.Friendly = (int)entity >= 3;


            // a timer 0.1mp-enkent szamol, es a speed, meg a rateof miatt a lepes meg a tamadas 1,2,3 mp-enkent aktivalodik (elapsedTime % this.Speed == 0)
            switch (entity)
            {
                case Entity.EMPTY:
                    this.Damage = 0;
                    this.Hp = 0;
                    this.Speed = 0;
                    this.RateOF = 0;
                    break;
                case Entity.ORC:
                    this.Damage = 200;
                    this.Hp = 200;
                    this.Speed = 20;
                    this.RateOF = 20;
                    break;
                case Entity.TOWER:
                    this.Damage = 50;
                    this.Speed = 0;
                    this.Hp = 1000;
                    this.RateOF = 20;
                    break;
                case Entity.SHOOTER:
                    this.Damage = 30;
                    this.Speed = 0;
                    this.Hp = 100;
                    this.RateOF = 10;
                    break;
                case Entity.MINER:
                    this.Damage = 0;
                    this.Speed = 0;
                    this.Hp = 500;
                    this.RateOF = 30;
                    break;
                case Entity.TRAP:
                    this.Damage = 100;
                    this.Speed = 0;
                    this.Hp = 50;
                    this.RateOF = 1;
                    break;
                case Entity.CANNON:
                    this.Damage = 1000;
                    this.Hp = 50;
                    this.Speed = 0;
                    this.RateOF = 5;
                    break;
                default:
                    break;

            }

        }
        public Entity Type
        {
            get; set;
        }
        public bool Friendly
        {
            get; set;
        }
        public int Hp
        {
            get; set;
        }
        public int Speed
        {
            get; set;
        }
        public int Damage
        {
            get; set;
        }
        public int Level
        {
            get; set;
        }
        public int RateOF
        {
            get; set;
        }

    }
}
