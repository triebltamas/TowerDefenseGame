using System;
using System.Collections.Generic;
using System.Text;
using TowerDefense.Persistence;
using System.Threading.Tasks;
using System.Timers;

namespace TowerDefense.Model
{
    public class GameModel
    {
        #region Private fields
        private IDataAccess dataAccess;
        private GameState gs;
        private Timer timer;
        private bool hasBegun;
        #endregion

        #region Public properties
        public GameState GS() { return gs; }
        public bool HasBegun() { return hasBegun; }
        #endregion

        #region Persistence
        /// <summary>
        /// Jatek elmenteseert felel
        /// </summary>
        /// <param name="path"> a mentett file utvonala </param>
        /// <returns> ?? </returns>
        public async Task SaveGameAsync(String path)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await dataAccess.SaveAsync(path, gs);
        }
        /// <summary>
        /// Jatek betolteseert felel, beallitja a gamestate-et
        /// </summary>
        /// <param name="path"> a mentett file utvonala </param>
        /// <returns> ?? </returns>
        public async Task LoadGameAsync(String path)
        {
            if (dataAccess == null)
                throw new InvalidOperationException("No data access is provided."); 
            gs = await dataAccess.LoadAsync(path);
            for (int i = 0; i < 12; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    UpdateCellEvent(this, new CellEventArgs(i, j, gs.Table[i, j].Type, gs.Table[i, j].Hp, gs.Table[i, j].Level));
                }
            }
            UpdateGoldLabelEvent(this, gs.Gold);
            UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
            CannonAvailableEvent(this, gs.CannonAvailable);
            timer.Interval = 100;
            timer.Start();
        }
        #endregion

        #region Events
        public event EventHandler<CellEventArgs> UpdateCellEvent;
        public event EventHandler<int> UpdateGoldLabelEvent;
        public event EventHandler<int> UpdateEnemiesLabelEvent;
        public event EventHandler<bool> EndGameEvent;
        public event EventHandler<AttackEventArgs> ShowAttackEvent;
        public event EventHandler<int> CatastropheEvent;
        public event EventHandler<bool> CannonAvailableEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// modell konstruktora
        /// </summary>
        /// <param name="da">perzisztenciaert felelos interfesz</param>
        public GameModel(IDataAccess da)
        {
            timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += ProgressGame;
            dataAccess = da;
            gs = new GameState(1);
        }

        #endregion

        #region Public methods
        /// <summary>
        /// uj jatek kezdeseert felelos fuggveny
        /// </summary>
        /// <param name="diff"> a jatek nehezseget allitja be </param>
        public void NewGame(int diff)
        {
            timer.Stop();
            hasBegun = true;
            gs.CannonAvailable = false;
            gs = new GameState(diff);
            UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
            UpdateGoldLabelEvent(this, gs.Gold);
            CannonAvailableEvent(this, gs.CannonAvailable);
            timer.Start();
        }
        /// <summary>
        /// sajat egyseg lehelyezeseert felelos
        /// </summary>
        /// <param name="type"> egyseg tipusa </param>
        /// <param name="row"> cella sora </param>
        /// <param name="col"> cella oszlopa </param>
        /// <returns> sikeres lehelyezes eseten true, sikertelen eseten false </returns>
        public bool Place(Entity type, int row, int col)
        {
            int cost = 0;
            switch (type)
            {
                case Entity.TOWER:
                    {
                        cost = 200;
                        break;
                    }
                case Entity.MINER:
                    {
                        cost = 100;
                        break;
                    }
                case Entity.SHOOTER:
                    {
                        cost = 140;
                        break;
                    }
                case Entity.TRAP:
                    {
                        cost = 70;
                        break;
                    }
                case Entity.CANNON:
                    {
                        cost = 0;
                        gs.CannonAvailable = false;
                        CannonAvailableEvent(this, false);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (gs.Table[row, col].Type == Entity.EMPTY && gs.Gold >= cost)
            {
                gs.Gold -= cost;
                gs.Table[row, col].CreateCell(type);
                UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                UpdateGoldLabelEvent(this, gs.Gold);
                return true;
            }
            return false;
        }
        /// <summary>
        /// sajat egyseg fejleszteseert felelos
        /// </summary>
        /// <param name="type"> egyseg tipusa </param>
        /// <param name="row"> cella sora </param>
        /// <param name="col"> cella oszlopa </param>
        /// <returns> sikeres fejlesztes eseten true, sikertelen eseten false  </returns>
        public bool Upgrade(Entity type, int row, int col)
        {
            if (Entity.TOWER <= type && type <= Entity.MINER && gs.Table[row, col].Level == 1)
            {
                gs.Table[row, col].Level = 2;
                switch (type)
                {
                    case Entity.TOWER:
                        if (gs.Gold >= 400)
                        {
                            gs.Table[row, col].Damage += 25;
                            gs.Table[row, col].Hp += 500;
                            gs.Table[row, col].RateOF -= 5;
                            gs.Gold -= 400;
                        }
                        break;
                    case Entity.SHOOTER:
                        if (gs.Gold >= 200)
                        {
                            gs.Table[row, col].Damage += 10;
                            gs.Table[row, col].Hp += 25;
                            gs.Table[row, col].RateOF -= 5;
                            gs.Gold -= 200;
                        }
                        break;
                    case Entity.MINER:
                        if (gs.Gold >= 400)
                        {
                            gs.Table[row, col].RateOF -= 15;
                            gs.Gold -= 400;
                        }
                        break;

                    case Entity.TRAP:
                        if (gs.Gold >= 150)
                        {
                            gs.Table[row, col].Damage += 100;
                            gs.Gold -= 150;
                        }
                        break;
                    default:
                        break;
                }
                UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                UpdateGoldLabelEvent(this, gs.Gold);
                return true;
            }
            return false;
        }
        /// <summary>
        /// lekerdezi hogy egy cella ellenseges vagy ures-e ( a fejlesztest konnyiti meg )
        /// </summary>
        /// <param name="row"> cella sora </param>
        /// <param name="col"> cella oszlopa </param>
        /// <returns> ellenseges vagy ures cella eseten true, egyebkent false </returns>
        public bool isEnemyOrEmpty(int row, int col)
        {
            return gs.Table[row, col].Type <= Entity.ORC || gs.Table[row, col].Type == Entity.EMPTY;
        }
        /// <summary>
        /// lekerdezi hogy egy cella ures-e
        /// </summary>
        /// <param name="row"> cella sora </param>
        /// <param name="col"> cella oszlopa </param>
        /// <returns> ha egy cella nem ures, true, egyebkent false </returns>
        public bool notEmpty(int row, int col)
        {
            return gs.Table[row, col].Type != Entity.EMPTY;
        }
        /// <summary>
        /// katasztrofa veghezviteleert felelos
        /// </summary>
        /// <param name="row"> kozeppont cella sora </param>
        /// <param name="col"> kozeppont cella oszlopa </param>
        public void Catastrophe(int row, int col)
        {
            int rowStart = (row - 1 <= 0) ? 0 : (row - 1);
            int rowEnd = (row + 1 >= 11) ? 11 : (row + 1);

            int colStart = (col - 1 <= 0) ? 0 : (col - 1);
            int colEnd = (col + 1 >= 7) ? 7 : (col + 1);

            for (int i = rowStart; i <= rowEnd; i++)
            {
                for (int j = colStart; j <= colEnd; j++)
                {
                    if (gs.Table[i, j].Type < Entity.EMPTY)
                    {
                        if (gs.Table[i, j].Hp - 200 > 0)
                        {
                            gs.Table[i, j].Hp -= 200;
                        }
                        else
                        {
                            if (gs.Table[i, j].Type <= Entity.ORC)
                                gs.RemainingEnemies--;
                            gs.Table[i, j].Type = Entity.EMPTY;
                            UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                            UpdateGoldLabelEvent(this, gs.Gold);
                        }
                        UpdateCellEvent(this, new CellEventArgs(i, j, gs.Table[i, j].Type, gs.Table[i, j].Hp, gs.Table[i, j].Level));
                    }
                }
            }
        }

        /// <summary>
        /// epulet lebontasaert felel, 50%-t ad vissza a koltsegbol
        /// </summary>
        /// <param name="row">lebontando epulet sora</param>
        /// <param name="col">lebontando epulet oszlopa</param>
        public void BreakDown(int row, int col)
        {
            switch (gs.Table[row, col].Type)
            {
                case Entity.TOWER:
                    {
                        gs.Table[row, col].Type = Entity.EMPTY;
                        UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                        gs.Gold += (gs.Table[row, col].Level == 1) ? 100 : 300 ;
                        UpdateGoldLabelEvent(this, gs.Gold);
                        break;
                    }
                case Entity.SHOOTER:
                    {
                        gs.Table[row, col].Type = Entity.EMPTY;
                        UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                        gs.Gold += (gs.Table[row, col].Level == 1) ? 70 : 170;
                        UpdateGoldLabelEvent(this, gs.Gold);
                        break;
                    }
                case Entity.MINER:
                    {
                        gs.Table[row, col].Type = Entity.EMPTY;
                        UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                        gs.Gold += (gs.Table[row, col].Level == 1) ? 50 : 250;
                        UpdateGoldLabelEvent(this, gs.Gold);
                        break;
                    }
                case Entity.TRAP:
                    {
                        gs.Table[row, col].Type = Entity.EMPTY;
                        UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                        gs.Gold += (gs.Table[row, col].Level == 1) ? 35 : 110;
                        UpdateGoldLabelEvent(this, gs.Gold);
                        break;
                    }
                default:
                    break;
            }

        }
        /// <summary>
        /// megallitja a jatekot
        /// </summary>
        public void Pause()
        {
                timer.Stop();
        }
        /// <summary>
        /// folytatja a jatekot
        /// </summary>
        public void UnPause()
        {
            if(!timer.Enabled)
                timer.Start();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// chance %-kal ad a jatekosnak egy cannont
        /// </summary>
        /// <param name="chance">0-100 szazalekos eselye a cannon dropnak</param>
        private void DropCannon(int chance)
        {
            Random r = new Random();
            if ((r.Next(0, 100)+1) <= chance)
            {
                gs.CannonAvailable = true;
                CannonAvailableEvent(this,true);
            }
        }
        /// <summary>
        /// A jatekora 1 tick-enkenti lepteteseert felelos fuggveny,
        /// kezeli az ellensegek lepteteset, katasztrofa kivaltasat, minden entity tamadasat,
        /// ellensegek spawnolasat, vereseg / gyozelem ellenorzeset
        /// </summary>
        /// <param name="sender"> ?? </param>
        /// <param name="e"> ?? </param>
        private void ProgressGame(object sender, EventArgs e)
        {
            gs.ElapsedTime++;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.Table[i, j].Type <= Entity.ORC && (gs.ElapsedTime % gs.Table[i, j].Speed == 0))
                    {
                        NextStep(i, j);
                    }
                }
            }

            //katasztrofa
            Random rand = new Random();
            int catastrophe = rand.Next(0, 1000);
            if(catastrophe < 1)
            {
                int coord1 = rand.Next(0, 12);
                int coord2 = rand.Next(0, 8);
                Catastrophe(coord1,coord2);
                CatastropheEvent(this, (coord1 * 8 + coord2));
            }

            // tamadas
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.Table[i, j].Type != Entity.EMPTY && gs.Table[i, j].RateOF != 0)
                    {
                        if (gs.ElapsedTime % gs.Table[i, j].RateOF == 0)
                        {
                            Attack(gs.Table[i, j], i, j);
                        }
                    }
                }
            }

            // 3mp-kent jelennek meg az ellenseges egysegek
            if (gs.ElapsedTime % 30 == 0)
            {
                bool done = false;
                Random random = new Random();
                while (!done)
                {
                    int enemyIndex = random.Next(0, 8);

                    if (gs.Table[11, enemyIndex].Type == Entity.EMPTY)
                    {
                        gs.Table[11, enemyIndex].CreateCell(Entity.ORC);
                        done = true;

                        UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                        UpdateCellEvent(this, new CellEventArgs(11, enemyIndex, gs.Table[11, enemyIndex].Type, gs.Table[11, enemyIndex].Hp, gs.Table[11, enemyIndex].Level));
                    }
                }
            }

            //vereseg ellenorzese
            bool b = false;
            for (int j = 0; j < 8; j++)
            {
                if ((int)gs.Table[0, j].Type < 3)
                {
                    b = true;
                    break;
                }
            }
            if (b || (gs.RemainingEnemies <= 0))
            {
                Pause();
                hasBegun = false;
                EndGameEvent(this,!b);
            }
        }
        /// <summary>
        /// lepteti az ellenseget egy cellaval feljebb
        /// </summary>
        /// <param name="row"> ellneseg poziciojanak sora </param>
        /// <param name="col"> ellneseg poziciojanak oszlopa </param>
        private void NextStep(int row, int col)
        {
            if (gs.Table[row - 1, col].Type != Entity.EMPTY)
            {
                return;
            }
                gs.Table[row - 1, col].Type = gs.Table[row, col].Type;
                gs.Table[row - 1, col].Speed = gs.Table[row, col].Speed;
                gs.Table[row - 1, col].RateOF = gs.Table[row, col].RateOF;
                gs.Table[row - 1, col].Level = gs.Table[row, col].Level;
                gs.Table[row - 1, col].Hp = gs.Table[row, col].Hp;
                gs.Table[row - 1, col].Friendly = gs.Table[row, col].Friendly;
                gs.Table[row - 1, col].Damage = gs.Table[row, col].Damage;
                gs.Table[row, col].Type = Entity.EMPTY;
                UpdateCellEvent(this, new CellEventArgs(row - 1, col, gs.Table[row - 1, col].Type, gs.Table[row - 1, col].Hp, gs.Table[row - 1, col].Level));
                UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
        }
        /// <summary>
        /// minden entity tamadasaert felelos
        /// </summary>
        /// <param name="attacker"> tamado objektum </param>
        /// <param name="row"> tamado cellajanak sora </param>
        /// <param name="col"> tamado cellajanak oszlopa </param>
        private void Attack(Cell attacker, int row, int col)
        {
            int cannonchance = 100;
            //int cannonchance = 2;
            switch (attacker.Type)
            {
                case Entity.ORC:
                    if (row > 0 && gs.Table[row - 1, col].Type >= Entity.TOWER && (gs.Table[row - 1, col].Type != Entity.EMPTY && gs.Table[row - 1, col].Type != Entity.TRAP))
                    {
                        if (gs.Table[row - 1, col].Hp - attacker.Damage > 0)
                        {
                            gs.Table[row - 1, col].Hp -= attacker.Damage;
                        }
                        else
                        {
                            gs.Table[row - 1, col].Type = Entity.EMPTY;
                        }
                        UpdateCellEvent(this, new CellEventArgs(row - 1, col, gs.Table[row - 1, col].Type, gs.Table[row - 1, col].Hp, gs.Table[row - 1, col].Level));
                    }
                    break;
                case Entity.TOWER:
                    //3x3 mezők vizsgalata Table[i,j] korul
                    int rowStart = (row - 1 <= 0) ? 0 : (row - 1);
                    int rowEnd = (row + 1 >= 11) ? 11 : (row + 1);

                    int colStart = (col - 1 <= 0) ? 0 : (col - 1);
                    int colEnd = (col + 1 >= 7) ? 7 : (col + 1);

                    for (int i = rowStart; i <= rowEnd; i++)
                    {
                        for (int j = colStart; j <= colEnd; j++)
                        {
                            if (gs.Table[i, j].Type <= Entity.ORC)
                            {
                                if (gs.Table[i, j].Hp - attacker.Damage > 0)
                                {
                                    gs.Table[i, j].Hp -= attacker.Damage;
                                }
                                else
                                {
                                    gs.Table[i, j].Type = Entity.EMPTY;
                                    gs.Gold += 20;
                                    gs.RemainingEnemies--;

                                    UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                                    UpdateGoldLabelEvent(this, gs.Gold);
                                    DropCannon(cannonchance);
                                }
                                UpdateCellEvent(this, new CellEventArgs(i, j, gs.Table[i, j].Type, gs.Table[i, j].Hp, gs.Table[i, j].Level, true));
                            }
                        }
                    }
                    ShowAttackEvent(this, new AttackEventArgs(row, col, Entity.TOWER));
                    break;
                case Entity.SHOOTER:
                    for (int i = row + 1; i < 12; i++)
                    {
                        if (gs.Table[i, col].Type <= Entity.ORC)
                        {
                            ShowAttackEvent(this, new AttackEventArgs(row, col, Entity.SHOOTER, i, col));
                            if (gs.Table[i, col].Hp - attacker.Damage > 0)
                            {
                                gs.Table[i, col].Hp -= attacker.Damage;
                            }
                            else
                            {
                                gs.Table[i, col].Type = Entity.EMPTY;
                                gs.Gold += 20;
                                gs.RemainingEnemies--;
                                DropCannon(cannonchance);
                                UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                                UpdateGoldLabelEvent(this, gs.Gold);
                            }
                            UpdateCellEvent(this, new CellEventArgs(i, col, gs.Table[i, col].Type, gs.Table[i, col].Hp, gs.Table[i, col].Level, true));
                            break;
                        }
                    }
                    break;
                case Entity.MINER:
                    gs.Gold += 200;
                    UpdateGoldLabelEvent(this, gs.Gold);
                    break;
                case Entity.TRAP:
                    if (gs.Table[row + 1, col].Type <= Entity.ORC)
                    {
                        if (gs.Table[row + 1, col].Hp - attacker.Damage > 0)
                        {
                            gs.Table[row + 1, col].Hp -= attacker.Damage;
                        }
                        else
                        {
                            gs.Table[row + 1, col].Type = Entity.EMPTY;
                            gs.Gold += 20;
                            gs.RemainingEnemies--;
                            DropCannon(cannonchance);
                            UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                            UpdateGoldLabelEvent(this, gs.Gold);
                        }
                        gs.Table[row, col].Type = Entity.EMPTY;

                        UpdateCellEvent(this, new CellEventArgs(row + 1, col, gs.Table[row + 1, col].Type, gs.Table[row + 1, col].Hp, gs.Table[row + 1, col].Level));
                        UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                    }
                    break;
                case Entity.CANNON:
                    ShowAttackEvent(this, new AttackEventArgs(row, col, Entity.CANNON));
                    for (int i = row + 1; i < 12; i++)
                    {
                        if (gs.Table[i, col].Type <= Entity.ORC)
                        {
                            if (gs.Table[i, col].Hp - attacker.Damage > 0)
                            {
                                gs.Table[i, col].Hp -= attacker.Damage;
                            }
                            else
                            {
                                gs.Table[i, col].Type = Entity.EMPTY;
                                gs.Gold += 20;
                                gs.RemainingEnemies--;
                                DropCannon(cannonchance);
                                UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                                UpdateGoldLabelEvent(this, gs.Gold);
                            }
                            UpdateCellEvent(this, new CellEventArgs(i, col, gs.Table[i, col].Type, gs.Table[i, col].Hp, gs.Table[i, col].Level, true));
                        }
                    }
                    gs.Table[row, col].Type = Entity.EMPTY;
                    UpdateCellEvent(this, new CellEventArgs(row, col, gs.Table[row, col].Type, gs.Table[row, col].Hp, gs.Table[row, col].Level));
                    CannonAvailableEvent(this, false);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Test methods
        public void ProgressGameTest(object sender, EventArgs e)
        {
            gs.ElapsedTime++;

            //leptetes, cellupdateevent a next-stepbe kerult
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.Table[i, j].Type == Entity.ORC && (gs.ElapsedTime % gs.Table[i, j].Speed == 0))
                    {
                        NextStep(i, j);
                    }
                }
            }

            //tamadas

            //katasztrofat kulon teszteljuk


            //leptetes, cellupdateevent a next-stepbe kerult

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (gs.Table[i, j].Type != Entity.EMPTY && gs.Table[i, j].RateOF != 0)
                    {
                        if (gs.ElapsedTime % gs.Table[i, j].RateOF == 0)
                        {
                            Attack(gs.Table[i, j], i, j);
                        }
                    }
                }
            }

            //Teszteleskent 3mp-kent jelennek meg az ellenseges egysegek
            if (gs.ElapsedTime % 30 == 0)
            {
                bool done = false;
                Random random = new Random();
                int enemyType = 1;//random.Next(0, 3);
                while (!done)
                {
                    int enemyIndex = random.Next(0, 8);

                    if (gs.Table[11, enemyIndex].Type == Entity.EMPTY)
                    {
                        gs.Table[11, enemyIndex].CreateCell((Entity)enemyType);
                        done = true;

                        UpdateEnemiesLabelEvent(this, gs.RemainingEnemies);
                        UpdateCellEvent(this, new CellEventArgs(11, enemyIndex, gs.Table[11, enemyIndex].Type, gs.Table[11, enemyIndex].Hp, gs.Table[11, enemyIndex].Level));
                    }
                }
            }

            //vereseg ellenorzese
            bool b = false;
            for (int j = 0; j < 8; j++)
            {
                if ((int)gs.Table[0, j].Type < 3)
                {
                    //GAME_OVER -- false == vereseg && true == gyozelem ?
                    //timer.Stop();
                    b = true;
                    //EndGameEvent.Invoke(this, false);
                    //break;
                }
            }
            if (b || (gs.RemainingEnemies <= 0))
            {
                hasBegun = false;
                EndGameEvent(this, !b);
            }
        }
        #endregion
    }
}
