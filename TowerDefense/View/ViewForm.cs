using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TowerDefense.Model;
using TowerDefense.Persistence;
using TowerDefense.View;
using System.Drawing;

namespace TowerDefense
{
    public partial class ViewForm : Form
    {
        #region Fields
        private List<List<ViewCell>> table;
        private GameModel model;
        private DataAccess dataAccess;
        private Entity chosenUnit;
        public enum UserAction { PLACE, UPGRADE, BREAK_DOWN, NONE }
        private UserAction action;
        #endregion

        #region Constructor
        public ViewForm()
        {
            InitializeComponent();
            
            table = new List<List<ViewCell>>();
            GenerateTable();
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Padding = new Padding(0);
            tableLayoutPanel.BackColor = Color.FromArgb(255, 226, 178);
            BackColor = Color.FromArgb(255, 226, 178);
            cannonButton.Visible = false;

            dataAccess = new DataAccess();
            model = new GameModel(dataAccess);
            chosenUnit = Entity.EMPTY;

            newGameMenuItem.Click += NewGame;
            saveGameMenuItem.Click += SaveGame;
            loadGameMenuItem.Click += LoadGame;
            towerButton.Click += UnitChosen;
            minerButton.Click += UnitChosen;
            shooterButton.Click += UnitChosen;
            trapButton.Click += UnitChosen;
            cannonButton.Click += UnitChosen;
            upgradeButton.Click += UpgradeButtonClicked;
            breakDownButton.Click += BreakDownButtonClicked;
            model.UpdateEnemiesLabelEvent += RefreshEnemiesLabel;
            model.UpdateGoldLabelEvent += RefreshGoldLabel;
            model.EndGameEvent += HandleEndGame;
            model.UpdateCellEvent += UpdateCell;
            model.ShowAttackEvent += ShowAttack;
            model.CatastropheEvent += Catastrophe;
            model.CannonAvailableEvent += CannonAvailable;
        }
        #endregion

        #region User event handlers
        private void UpgradeButtonClicked(object sender, EventArgs e)
        {
            action = UserAction.UPGRADE;
        }

        private void BreakDownButtonClicked(object sender, EventArgs e)
        {
            action = UserAction.BREAK_DOWN;
        }
        /// <summary>
        /// Mezőre kattintás eseménykezelője
        /// Új egység lerakásáért, fejlesztéséért és lebontásáért felelős
        /// </summary>
        private void FieldClicked(object sender, EventArgs e)
        {
            int idx = tableLayoutPanel.Controls.IndexOf(sender as ViewCell);
            int row = idx / 8;
            int col = idx % 8;
            switch(action)
            {
                case UserAction.PLACE:
                    Place(row, col);
                    break;
                case UserAction.UPGRADE:
                    Upgrade(row, col);
                    break;
                case UserAction.BREAK_DOWN:
                    if(model.isEnemyOrEmpty(row, col))
                    {
                        statusStrip.Invoke(new Action(() => statusLabel.Text = "Csak saját egységet bonthat el."));
                    }
                    else
                    {
                        model.BreakDown(row, col);
                        action = UserAction.NONE;
                        goldLabel.Focus();
                    }
                    break;
                case UserAction.NONE:
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "Kattintson a fenti gombok közül valamelyikre!"));
                    break;
            }
        }
        /// <summary>
        /// Egység kiválasztás eseménykezelője
        /// </summary>
        private void UnitChosen(object sender, EventArgs e)
        {
            action = UserAction.PLACE;
            switch ((sender as Button).Text)
            {
                case "Torony":
                    chosenUnit = Entity.TOWER;
                    break;
                case "Bányász":
                    chosenUnit = Entity.MINER;
                    break;
                case "Puskás":
                    chosenUnit = Entity.SHOOTER;
                    break;
                case "Akna":
                    chosenUnit = Entity.TRAP;
                    break;
                case "Ágyú":
                    chosenUnit = Entity.CANNON;
                    break;
            }
            statusStrip.Invoke(new Action(() => statusLabel.Text = ""));
        }

        private void ViewCell_MouseLeave(object sender, EventArgs e)
        {
            ViewCell_MouseUp(sender, null);
        }

        private void ViewCell_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is ViewCell)
            {
                ViewCell pictureBox = (ViewCell)sender;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void ViewCell_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is ViewCell)
            {
                ViewCell pictureBox = (ViewCell)sender;
                pictureBox.BorderStyle = BorderStyle.Fixed3D;
            }
        }
        
        /// <summary>
        /// Új játék eseménykezelője 
        /// </summary>
        private void NewGame(object sender, EventArgs e)
        {
            model.Pause();
            int diff = DiffDialog.ShowDialog("Válassz nehézséget:", "Xdd Tower Defense");
            if (diff == -1)
            {
                Invoke(new Action(() => Close()));
                return;
            }
            if (diff != 0)
            {
                model.NewGame(diff);
                statusStrip.Invoke(new Action(() => statusLabel.Text = ""));
                ResetTable();
            }
            else if (model.HasBegun())//ha nem választott nehézséget, és nem is a kilépés gombra katintott <-> folytatja tovább a játékot
            {
                model.UnPause();
            }
        }

        #endregion

        #region Modell event handlers
        /// <summary>
        /// Ágyú elérhetőségének eseménykezelője
        /// </summary>
        /// <param name="e">Elérhető-e az ágyú</param>
        private void CannonAvailable(object sender, bool e)
        {
            cannonButton.Invoke(new Action(() => cannonButton.Visible = e));
        }
        /// <summary>
        /// Saját egység támadás megjelenítésének az eseménykezelője
        /// </summary>
        /// <param name="e">Tartalmazza a támadó pozícióját, típusát, valamint az ellenség pozícióját</param>
        private void ShowAttack(object sender, AttackEventArgs e)
        {
            switch(e.Type)
            {
                case Entity.TOWER:
                    int rowStart = (e.Row - 1 <= 0) ? 0 : (e.Row - 1);
                    int rowEnd = (e.Row + 1 >= 11) ? 11 : (e.Row + 1);

                    int colStart = (e.Col - 1 <= 0) ? 0 : (e.Col - 1);
                    int colEnd = (e.Col + 1 >= 7) ? 7 : (e.Col + 1);

                    for (int i = rowStart; i <= rowEnd; i++)
                    {
                        for (int j = colStart; j <= colEnd; j++)
                        {
                            if ((i != e.Row || j != e.Col) && model.isEnemyOrEmpty(i, j))
                            {
                                table[i][j].ShowAttack(false);
                            }
                        }
                    }
                    break;
                case Entity.SHOOTER:
                    for(int i = e.Row + 1; i < e.EnemyRow; i++)
                    {
                        table[i][e.Col].ShowAttack(false);
                    }
                    break;
                case Entity.CANNON:
                    for(int i = e.Row + 1; i < 11; i++)
                    {
                        if (model.notEmpty(i, e.Col))
                            table[i][e.Col].ShowAttack(true);
                        else
                            table[i][e.Col].ShowAttack(false);
                    }
                    break;
            }
        }

        /// <summary>
        /// Cella frissítéséért felelős eseménykezelő
        /// </summary>
        /// <param name="e">A cella adatait tartalmazza</param>
        private void UpdateCell(object sender, CellEventArgs e)
        {
            switch (e.Type)
            {
                case Entity.TOWER:
                    if (e.Level == 1)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.tower1;
                    }
                    else if (e.Level == 2)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.tower2;
                    }
                    break;
                case Entity.SHOOTER:
                    if (e.Level == 1)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.shooter1;
                    }
                    else if (e.Level == 2)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.shooter2;
                    }
                    break;
                case Entity.MINER:
                    if (e.Level == 1)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.miner1;
                    }
                    else if (e.Level == 2)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.miner2;
                    }
                    break;
                case Entity.TRAP:
                    if (e.Level == 1)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.trap1;
                    }
                    else if (e.Level == 2)
                    {
                        table[e.Row][e.Col].Image = Properties.Resources.trap2;
                    }
                    break;
                case Entity.CANNON:
                    table[e.Row][e.Col].Image = Properties.Resources.cannon;
                    break;
                case Entity.ORC:
                    table[e.Row][e.Col].Image = Properties.Resources.orc;
                    break;
            }
            if (e.Hp == 0 || e.Type == Entity.EMPTY)
            {
                table[e.Row][e.Col].Hp = 0;
                table[e.Row][e.Col].Image = Properties.Resources.empty;
            }
            else
                table[e.Row][e.Col].Hp = e.Hp;
            if (e.IsAttacked) //ha hp-t vagy képet változtatunk, akkor ez a változtatás felülírja a keretszín változtatást
                table[e.Row][e.Col].ShowAttack(true);
        }

        /// <summary>
        /// Játék végének az eseménykezelője
        /// </summary>
        /// <param name="e">Győzött-e a játékos</param>
        private void HandleEndGame(object sender, bool e)
        {
            model.Pause();
            int diff = DiffDialog.ShowDialog( (e? "Győztél" : "Veszítettél") + ", válassz nehézséget:", "Xdd Tower Defense");
            if ((diff == 0) || (diff == -1))
            {
                Invoke(new Action(() => Close()));
                return;
            }
            else
            {
                model.NewGame(diff);
                statusStrip.Invoke(new Action(() => statusLabel.Text = ""));
                ResetTable();
                return;
            }
        }

        /// <summary>
        /// Katasztrófa megjelenítésének eseménykezelője
        /// </summary>
        /// <param name="e">Katasztrófa helye</param>
        private void Catastrophe(object sender, int e)
        {
            statusStrip.Invoke(new Action(() => statusLabel.Text = "Katasztrófa közeledik!"));
            int rowStart = (e / 8 - 1 <= 0) ? 0 : (e / 8 - 1);
            int rowEnd = (e / 8 + 1 >= 11) ? 11 : (e / 8 + 1);

            int colStart = (e % 8 - 1 <= 0) ? 0 : (e % 8 - 1);
            int colEnd = (e % 8 + 1 >= 7) ? 7 : (e % 8 + 1);

            for (int i = rowStart; i <= rowEnd; i++)
            {
                for (int j = colStart; j <= colEnd; j++)
                {
                    if (model.notEmpty(i, j))
                    {
                        table[i][j].ShowAttack(true);
                    }
                    else
                    {
                        table[i][j].ShowAttack(false);
                    }
                }
            }
            statusStrip.Invoke(new Action(() => statusLabel.Text = ""));
        }

        /// <summary>
        /// Aranyat megjelenítő címke frissítésének eseménykezelője
        /// </summary>
        /// <param name="e">Arany mennyiség</param>
        private void RefreshGoldLabel(object sender, int e)
        {
            goldLabel.Invoke(new Action(() => goldLabel.Text = ": " + e));
        }

        /// <summary>
        /// Hátralevő ellenségeket megjelenítő címke frissítésének eseménykezelője
        /// </summary>
        /// <param name="e">Ellenségek száma</param>
        private void RefreshEnemiesLabel(object sender, int e)
        {
            enemiesLabel.Invoke(new Action( ( ) => enemiesLabel.Text = ": " + e));
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Játéktábla generálásáért felelős
        /// Az egyes cellák eseményeihez eseménykezelőket rendel
        /// </summary>
        private void GenerateTable()
        {
            for (int i = 0; i < 12; i++)
            {
                List<ViewCell> row = new List<ViewCell>();
                for (int j = 0; j < 8; j++)
                {
                    ViewCell viewCell = new ViewCell() { Enabled = false };
                    viewCell.MouseDown += ViewCell_MouseDown;
                    viewCell.MouseUp += ViewCell_MouseUp;
                    viewCell.MouseLeave += ViewCell_MouseLeave;
                    viewCell.Click += FieldClicked;
                    row.Add(viewCell);
                    tableLayoutPanel.Controls.Add(viewCell, j, i);
                }
                table.Add(row);
            }
        }

        /// <summary>
        /// Játéktábla kiinduló állapotba való állításáért felelős
        /// </summary>
        private void ResetTable()
        {
            for(int i = 0; i < 12; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    table[i][j].Image = Properties.Resources.empty;
                    table[i][j].Hp = 0;
                    table[i][j].Enabled = true;
                }
            }
        }

        private void Place(int row, int col)
        {
            if(chosenUnit != Entity.EMPTY)
                    {
                        if (row == 11 || row == 0 || row == 10)
                        {
                            statusLabel.Text = "Ebbe a sorba nem helyezhetsz egységet!";
                        }
                        else
                        {
                            if(model.Place(chosenUnit, row, col))
                            {
                                chosenUnit = Entity.EMPTY;
                                goldLabel.Focus(); //ezzel az egységet kiválasztó gombokról levesszük a fókuszt
                            }
                            else
                                statusLabel.Text = "Egység lehelyezése sikertelen.";
                         }
                    }
                else
                    statusLabel.Text = "Nincs kiválasztva egység. Válassz egy lehelyezendő egységet a fentiek közül!";
        }

        private void Upgrade(int row, int col)
        {
            if(model.Upgrade(model.GS().Table[row,col].Type,row, col))
            {
                action = UserAction.NONE;
                goldLabel.Focus();
            }
            else
                statusStrip.Invoke(new Action(() => statusLabel.Text = "Fejlesztés sikertelen!"));
        }

        #endregion

        #region Persistence
        /// <summary>
        /// Játék betöltésének eseménykezelője 
        /// </summary>
        private async void LoadGame(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Játék betöltése";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            model.Pause();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.LoadGameAsync(openFileDialog.FileName);
                    model.UnPause();
                }
                catch(DataException)
                {
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A játék betöltése sikertelen."));
                }
            }
        }

        /// <summary>
        /// Játék mentésének eseménykezelője 
        /// </summary>
        private async void SaveGame(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Játék mentése";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            model.Pause();
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.SaveGameAsync(saveFileDialog.FileName);
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A játék mentése sikeres!"));
                    model.UnPause();
                }
                catch (DataException)
                {
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A játék mentése sikertelen."));
                }
            }
        }
        #endregion
    }
}
