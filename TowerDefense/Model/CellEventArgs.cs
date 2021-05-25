using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Persistence;

namespace TowerDefense.Model
{
    public class CellEventArgs : EventArgs
    {


        public CellEventArgs(int r, int c, Entity t, int h, int l, bool isAttacked = false){
            Row = r;
            Col = c;
            Type = t;
            Hp = h;
            Level = l;
            IsAttacked = isAttacked;
        }
        public int Row
        {
            get; private set;
        }
        public int Col
        {
            get; private set;
        }
        public int Hp
        {
            get; private set;
        }
        public int Level
        {
            get; private set;
        }
        public Entity Type
        {
            get; private set;
        }
        public bool IsAttacked
        {
            get; private set;
        }
    }
}
