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
        /// Mez??re kattint??s esem??nykezel??je
        /// ??j egys??g lerak??s????rt, fejleszt??s????rt ??s lebont??s????rt felel??s
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
                        statusStrip.Invoke(new Action(() => statusLabel.Text = "Csak saj??t egys??get bonthat el."));
                    }
                    else
                    {
                        model.BreakDown(row, col);
                        action = UserAction.NONE;
                        goldLabel.Focus();
                    }
                    break;
                case UserAction.NONE:
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "Kattintson a fenti gombok k??z??l valamelyikre!"));
                    break;
            }
        }
        /// <summary>
        /// Egys??g kiv??laszt??s esem??nykezel??je
        /// </summary>
        private void UnitChosen(object sender, EventArgs e)
        {
            action = UserAction.PLACE;
            switch ((sender as Button).Text)
            {
                case "Torony":
                    chosenUnit = Entity.TOWER;
                    break;
                case "B??ny??sz":
                    chosenUnit = Entity.MINER;
                    break;
                case "Pusk??s":
                    chosenUnit = Entity.SHOOTER;
                    break;
                case "Akna":
                    chosenUnit = Entity.TRAP;
                    break;
                case "??gy??":
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
        /// ??j j??t??k esem??nykezel??je 
        /// </summary>
        private void NewGame(object sender, EventArgs e)
        {
            model.Pause();
            int diff = DiffDialog.ShowDialog("V??lassz neh??zs??get:", "Xdd Tower Defense");
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
            else if (model.HasBegun())//ha nem v??lasztott neh??zs??get, ??s nem is a kil??p??s gombra katintott <-> folytatja tov??bb a j??t??kot
            {
                model.UnPause();
            }
        }

        #endregion

        #region Modell event handlers
        /// <summary>
        /// ??gy?? el??rhet??s??g??nek esem??nykezel??je
        /// </summary>
        /// <param name="e">El??rhet??-e az ??gy??</param>
        private void CannonAvailable(object sender, bool e)
        {
            cannonButton.Invoke(new Action(() => cannonButton.Visible = e));
        }
        /// <summary>
        /// Saj??t egys??g t??mad??s megjelen??t??s??nek az esem??nykezel??je
        /// </summary>
        /// <param name="e">Tartalmazza a t??mad?? poz??ci??j??t, t??pus??t, valamint az ellens??g poz??ci??j??t</param>
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
        /// Cella friss??t??s????rt felel??s esem??nykezel??
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
            if (e.IsAttacked) //ha hp-t vagy k??pet v??ltoztatunk, akkor ez a v??ltoztat??s fel??l??rja a keretsz??n v??ltoztat??st
                table[e.Row][e.Col].ShowAttack(true);
        }

        /// <summary>
        /// J??t??k v??g??nek az esem??nykezel??je
        /// </summary>
        /// <param name="e">Gy??z??tt-e a j??t??kos</param>
        private void HandleEndGame(object sender, bool e)
        {
            model.Pause();
            int diff = DiffDialog.ShowDialog( (e? "Gy??zt??l" : "Vesz??tett??l") + ", v??lassz neh??zs??get:", "Xdd Tower Defense");
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
        /// Katasztr??fa megjelen??t??s??nek esem??nykezel??je
        /// </summary>
        /// <param name="e">Katasztr??fa helye</param>
        private void Catastrophe(object sender, int e)
        {
            statusStrip.Invoke(new Action(() => statusLabel.Text = "Katasztr??fa k??zeledik!"));
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
        /// Aranyat megjelen??t?? c??mke friss??t??s??nek esem??nykezel??je
        /// </summary>
        /// <param name="e">Arany mennyis??g</param>
        private void RefreshGoldLabel(object sender, int e)
        {
            goldLabel.Invoke(new Action(() => goldLabel.Text = ": " + e));
        }

        /// <summary>
        /// H??tralev?? ellens??geket megjelen??t?? c??mke friss??t??s??nek esem??nykezel??je
        /// </summary>
        /// <param name="e">Ellens??gek sz??ma</param>
        private void RefreshEnemiesLabel(object sender, int e)
        {
            enemiesLabel.Invoke(new Action( ( ) => enemiesLabel.Text = ": " + e));
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// J??t??kt??bla gener??l??s????rt felel??s
        /// Az egyes cell??k esem??nyeihez esem??nykezel??ket rendel
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
        /// J??t??kt??bla kiindul?? ??llapotba val?? ??ll??t??s????rt felel??s
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
                            statusLabel.Text = "Ebbe a sorba nem helyezhetsz egys??get!";
                        }
                        else
                        {
                            if(model.Place(chosenUnit, row, col))
                            {
                                chosenUnit = Entity.EMPTY;
                                goldLabel.Focus(); //ezzel az egys??get kiv??laszt?? gombokr??l levessz??k a f??kuszt
                            }
                            else
                                statusLabel.Text = "Egys??g lehelyez??se sikertelen.";
                         }
                    }
                else
                    statusLabel.Text = "Nincs kiv??lasztva egys??g. V??lassz egy lehelyezend?? egys??get a fentiek k??z??l!";
        }

        private void Upgrade(int row, int col)
        {
            if(model.Upgrade(model.GS().Table[row,col].Type,row, col))
            {
                action = UserAction.NONE;
                goldLabel.Focus();
            }
            else
                statusStrip.Invoke(new Action(() => statusLabel.Text = "Fejleszt??s sikertelen!"));
        }

        #endregion

        #region Persistence
        /// <summary>
        /// J??t??k bet??lt??s??nek esem??nykezel??je 
        /// </summary>
        private async void LoadGame(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "J??t??k bet??lt??se";
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
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A j??t??k bet??lt??se sikertelen."));
                }
            }
        }

        /// <summary>
        /// J??t??k ment??s??nek esem??nykezel??je 
        /// </summary>
        private async void SaveGame(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "J??t??k ment??se";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            model.Pause();
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await model.SaveGameAsync(saveFileDialog.FileName);
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A j??t??k ment??se sikeres!"));
                    model.UnPause();
                }
                catch (DataException)
                {
                    statusStrip.Invoke(new Action(() => statusLabel.Text = "A j??t??k ment??se sikertelen."));
                }
            }
        }
        #endregion
    }
}
