using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerDefense.Model;
using TowerDefense.Persistence;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace TowerDefense.Test
{
    [TestClass]
    public class TowerDefenseTest
    {
        private GameModel _model;
        private Mock<IDataAccess> _mock;
        GameState _mockedGameState;
        private bool gameWon;
        private bool gameLost;

        [TestInitialize]
        public void Initialize()
        {
            _mockedGameState = new GameState(1);
            _mockedGameState.Table[1, 4].CreateCell(Entity.SHOOTER);
            _mockedGameState.Table[1, 6].CreateCell(Entity.TOWER);
            _mockedGameState.Table[1, 7].CreateCell(Entity.MINER);
            _mockedGameState.Table[2, 2].CreateCell(Entity.ORC);
            _mockedGameState.Table[2, 6].CreateCell(Entity.ORC);
            _mockedGameState.Table[11, 4].CreateCell(Entity.ORC);


            // O = ORC; S = SHOOTER; T = TOWER; M = MINER
            //The table:
            /*    0 1 2 3 4 5 6 7
             *0  | | | | | | | | |
             *1  | | | | |S| |T|M| 
             *2  | | |O| | | |O| | 
             *3  | | | | | | | | | 
             *4  | | | | | | | | |
             *5  | | | | | | | | |
             *6  | | | | | | | | |
             *7  | | | | | | | | |
             *8  | | | | | | | | |
             *9  | | | | | | | | |
             *10 | | | | | | | | |
             *11 | | | | |O| | | |
             */


            _mock = new Mock<IDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>())).Returns(Task.Run(() => _mockedGameState));
            // a mock a LoadAsync mûveletben bármilyen paraméterre a fent meghatározott _mockedGameState változót fogja visszaadni

            _model = new GameModel(_mock.Object); // példányosítjuk a modellt a mock objektummal
            

            _model.CatastropheEvent += new EventHandler<int>(Model_Catastrophe);
            _model.EndGameEvent += new EventHandler<bool>(Model_EndGame);
            _model.ShowAttackEvent += new EventHandler<AttackEventArgs>(Model_ShowAttackEvent);
            _model.UpdateCellEvent += new EventHandler<CellEventArgs>(Model_UpdateCell);
            _model.UpdateEnemiesLabelEvent += new EventHandler<int>(Model_UpdateEnemiesLabel);
            _model.UpdateGoldLabelEvent += new EventHandler<int>(Model_Catastrophe);
            _model.CannonAvailableEvent += new EventHandler<bool>(Model_EndGame);

            gameWon = false;
            gameLost = false;

        }

        //NEW GAME TEST
        [TestMethod]
        public void TowerDefense_NewGameTest()
        {
            _model.NewGame(1);
            Assert.AreEqual(_model.GS().Gold, 1000);
            for (int i = 0; i < 12; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    Assert.AreEqual(_model.GS().Table[i, j].Type, Entity.EMPTY);
                    Assert.AreEqual(_model.GS().Table[i, j].Level, 1);
                }
            }

        }

        //LOAD GAME TEST
        [TestMethod]
        public async Task TowerDefense_LoadGameTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            Assert.AreEqual(_model.GS().Table[1, 4].Type, Entity.SHOOTER);
            Assert.AreEqual(_model.GS().Table[1, 6].Type, Entity.TOWER);
            Assert.AreEqual(_model.GS().Table[1, 7].Type, Entity.MINER);
            Assert.AreEqual(_model.GS().Table[2, 6].Type, Entity.ORC);
            Assert.AreEqual(_model.GS().Table[11, 4].Type, Entity.ORC);
            Assert.AreEqual(_model.GS().Gold, 1000);

        }

        //ENEMY'S STEPPING TEST
        [TestMethod]
        public async Task TowerDefense_EnemySteppingTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            //speed = 20 
            int N = _model.GS().Table[11, 4].Speed;

            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            Assert.AreEqual(_model.GS().Table[10, 4].Type, Entity.ORC);

        }
        
        //DAMAGE TESTS
        [TestMethod]
        public async Task TowerDefense_TowerDamageTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            //RoF = 20 
            int N = _model.GS().Table[1, 6].RateOF;

            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            //Tower.Damage = 50
            //Orc.Hp = 200
            Assert.AreEqual(_model.GS().Table[2, 6].Hp, 150);

        }

        [TestMethod]
        public async Task TowerDefense_GoldMiningTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            //RoF = 30 
            int N = _model.GS().Table[1, 7].RateOF;

            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            //Gold0 = 1000
            //Miner.GoldMined = 200
            Assert.AreEqual(_model.GS().Gold, 1200);

        }

        [TestMethod]
        public async Task TowerDefense_ShooterDamageTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            //RoF = 10 
            int N = _model.GS().Table[11, 4].RateOF;

            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            //Shooter.Damage = 30
            //Orc.Hp = 200
            Assert.AreEqual(_model.GS().Table[11, 4].Hp, 170);

        }

        [TestMethod]
        public async Task TowerDefense_OrcDamageTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            //RoF = 20
            int N = _model.GS().Table[2, 6].RateOF;

            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            //Tower.Hp = 1000
            //Orc.Damage = 200
            Assert.AreEqual(_model.GS().Table[1, 6].Hp, 800);

        }

        //UPGRADE TEST
        [TestMethod]
        public async Task TowerDefense_UpgradeTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            _model.Upgrade(Entity.TOWER, 1, 6);

            Assert.AreEqual(_model.GS().Table[1, 6].Hp, 1500);
            Assert.AreEqual(_model.GS().Table[1, 6].Damage, 75);
            Assert.AreEqual(_model.GS().Table[1, 6].RateOF, 15);
            Assert.AreEqual(_model.GS().Gold, 600);
            Assert.AreEqual(_model.GS().Table[1, 6].Level, 2);

            _model.Upgrade(Entity.SHOOTER, 1, 4);

            Assert.AreEqual(_model.GS().Table[1, 4].Hp, 125);
            Assert.AreEqual(_model.GS().Table[1, 4].Damage, 40);
            Assert.AreEqual(_model.GS().Table[1, 4].RateOF, 5);
            Assert.AreEqual(_model.GS().Gold, 400);
            Assert.AreEqual(_model.GS().Table[1, 4].Level, 2);

            _model.Upgrade(Entity.MINER, 1, 7);

            Assert.AreEqual(_model.GS().Table[1, 7].RateOF, 15);
            Assert.AreEqual(_model.GS().Gold, 0);
            Assert.AreEqual(_model.GS().Table[1, 7].Level, 2);
        

        }

        //PLACE TEST
        [TestMethod]
        public async Task TowerDefense_PlaceTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            _model.Place(Entity.TOWER, 1, 0);

            Assert.AreEqual(_model.GS().Table[1, 0].Hp, 1000);
            Assert.AreEqual(_model.GS().Table[1, 0].Damage, 50);
            Assert.AreEqual(_model.GS().Table[1, 0].RateOF, 20);
            Assert.AreEqual(_model.GS().Gold, 800);

            _model.Place(Entity.SHOOTER, 5, 4);

            Assert.AreEqual(_model.GS().Table[5, 4].Hp, 100);
            Assert.AreEqual(_model.GS().Table[5, 4].Damage, 30);
            Assert.AreEqual(_model.GS().Table[5, 4].RateOF, 10);
            Assert.AreEqual(_model.GS().Gold, 660);

            _model.Place(Entity.MINER, 3, 4);

            Assert.AreEqual(_model.GS().Table[3, 4].Hp, 500);
            Assert.AreEqual(_model.GS().Table[3, 4].Damage, 0);
            Assert.AreEqual(_model.GS().Table[3, 4].RateOF, 30);
            Assert.AreEqual(_model.GS().Gold, 560);

            _model.Place(Entity.TRAP, 7, 4);

            Assert.AreEqual(_model.GS().Table[7, 4].Hp, 50);
            Assert.AreEqual(_model.GS().Table[7, 4].Damage, 100);
            Assert.AreEqual(_model.GS().Table[7, 4].RateOF, 1);
            Assert.AreEqual(_model.GS().Gold, 490);
        }

        //GAME OVER TESTS
        [TestMethod]
        public async Task TowerDefense_GameWonTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            _model.Place(Entity.TOWER, 1, 2);

            // O = ORC; S = SHOOTER; T = TOWER; M = MINER
            //The table:
            /*    0 1 2 3 4 5 6 7
             *0  | | | | | | | | |
             *1  | | |T| |S| |T|M| 
             *2  | | |O| | | |O| | 
             *3  | | | | | | | | | 
             *4  | | | | | | | | |
             *5  | | | | | | | | |
             *6  | | | | | | | | |
             *7  | | | | | | | | |
             *8  | | | | | | | | |
             *9  | | | | | | | | |
             *10 | | | | | | | | |
             *11 | | | | |O| | | |
             */


            int shooter = 3 * _model.GS().Table[1, 4].RateOF;
            int tower = 4 * _model.GS().Table[1, 2].RateOF;

            int N = tower > shooter ? tower : shooter;
            for (int i = 0; i < N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            Assert.IsTrue(gameWon);
        }

        [TestMethod]
        public async Task TowerDefense_GameLostTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            int N = _model.GS().Table[2, 2].Speed;

            for (int i = 0; i < 2*N; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            Assert.IsTrue(gameLost);

        }

        //ENEMY SPAWN TEST
        [TestMethod]
        public async Task TowerDefense_EnemySpawnTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);


            for (int i = 0; i < 30; ++i)
                _model.ProgressGameTest(this, new EventArgs());

            int count = 0; 
            for (int i = 0; i < 8; ++i)
            {
                if (_model.GS().Table[11, i].Type != Entity.EMPTY)
                    ++count;
            }
            Assert.AreEqual(count, 1);

        }

        //CATASTROPHE TEST
        [TestMethod]
        public async Task TowerDefense_CatastropheTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);

            _model.Catastrophe(2, 6);
            
            //1000-200=800
            Assert.AreEqual(_model.GS().Table[1, 6].Hp, 800);
            //500-200 = 300
            Assert.AreEqual(_model.GS().Table[1, 7].Hp, 300);
            //200-200=0 => EMPTY
            Assert.AreEqual(_model.GS().Table[2, 6].Type, Entity.EMPTY);

        }

        //BREAK DOWN TEST
        [TestMethod]
        public async Task TowerDefense_BreakDownTest()
        {
            _model.NewGame(1);
            await _model.LoadGameAsync(String.Empty);
            // O = ORC; S = SHOOTER; T = TOWER; M = MINER
            //The table:
            /*    0 1 2 3 4 5 6 7
             *0  | | | | | | | | |
             *1  | | | | |S| |T|M| 
             *2  | | |O| | | |O| | 
             *3  | | | | | | | | | 
             *4  | | | | | | | | |
             *5  | | | | | | | | |
             *6  | | | | | | | | |
             *7  | | | | | | | | |
             *8  | | | | | | | | |
             *9  | | | | | | | | |
             *10 | | | | | | | | |
             *11 | | | | |O| | | |
             */

            //+100
            _model.BreakDown(1, 6);
            Assert.AreEqual(_model.GS().Gold, 1100);
            //+50
            _model.BreakDown(1, 7);
            Assert.AreEqual(_model.GS().Gold, 1150);
            //+70
            _model.BreakDown(1, 4);
            Assert.AreEqual(_model.GS().Gold, 1220);
            //-200
            _model.Place(Entity.TOWER,1,6);
            //-400
            _model.Upgrade(Entity.TOWER,1,6);
            //+300
            _model.BreakDown(1, 6);
            Assert.AreEqual(_model.GS().Gold, 920);

        }

        public void Model_Catastrophe(Object sender, int e)
        {

        }
        public void Model_UpdateEnemiesLabel(Object sender, int e)
        {

        }
        public void Model_EndGame(Object sender, bool e)
        {
            gameWon = e;
            gameLost = !e;
        }
        public void Model_ShowAttackEvent(Object sender, AttackEventArgs e)
        {

        }
        public void Model_UpdateCell(Object sender, CellEventArgs e)
        {

        }
        public void Model_CellUpgraded(Object sender, EventArgs e)
        {

        }
    }
}
