using TowerDefense.Persistence;

namespace TowerDefense.Model
{
    public class AttackEventArgs
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Entity Type { get; private set; }
        /// <summary>
        /// Sebzendő ellenség sora
        /// </summary>
        public int EnemyRow { get; private set; }
        /// <summary>
        /// Sebzendő ellenség oszlopa
        /// </summary>
        public int EnemyCol { get; private set; }
        public AttackEventArgs(int row, int col, Entity type, int enemyRow = -1, int enemyCol = -1)
        {
            Row = row;
            Col = col;
            Type = type;
            EnemyRow = enemyRow;
            EnemyCol = enemyCol;
        }
    }
}
