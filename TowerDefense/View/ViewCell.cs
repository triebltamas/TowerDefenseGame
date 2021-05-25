using System;
using System.Drawing;
using System.Windows.Forms;

namespace TowerDefense.View
{
    /// <summary>
    /// HP, kép és a támadás megjelenítésért felelős osztály.
    /// </summary>
    public class ViewCell : PictureBox
    {
        #region Fields
        private System.Timers.Timer timer;
        private int hp;
        private bool isRunning, isAttacked;
        #endregion

        #region Constructor
        public ViewCell()
        {
            Width = 49;
            Height = 49;
            BorderStyle = BorderStyle.FixedSingle;
            Margin = new Padding(0);
            SizeMode = PictureBoxSizeMode.StretchImage;
            isRunning = false;
            isAttacked = false;
            hp = 0;

            timer = new System.Timers.Timer() { Interval = 1000 };

            timer.Elapsed += Timer_Elapsed;
            Paint += PaintEventHandler;
        }
        #endregion

        #region Event Handlers
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            isAttacked = false;
            this.Invoke(new Action(() => this.Refresh()));
            timer.Stop();
        }
        private void PaintEventHandler(object sender, PaintEventArgs e)
        {
            if(isRunning)
            {
                if(hp != 0)
                      e.Graphics.DrawString(Convert.ToString(hp), new Font(FontFamily.GenericSansSerif, 7), new SolidBrush(Color.Black), new Point(0, 0));
                else
                      e.Graphics.DrawString("", new Font(FontFamily.GenericSansSerif, 7), new SolidBrush(Color.Black), new Point(0, 0));
                if(isAttacked)
                      e.Graphics.FillRectangle(new SolidBrush(Color.Red), new Rectangle(new Point(0, 0), new Size(47, 10)));
            }
            else
                isRunning = true;
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
        #endregion

        #region ShowAttack method       
        public void ShowAttack(bool notEmpty)
        {
            if(!timer.Enabled)
            {
                if(notEmpty)
                {
                    isAttacked = true; 
                    this.Invoke(new Action(() => this.Refresh()));
                }
                else
                {
                    ControlPaint.DrawBorder(CreateGraphics(), ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
                }
                timer.Start();
            }
        }
        #endregion

        #region Properties
        public int Hp 
        {
            get { return hp; }
            set 
            { 
                hp = value;
                this.Invoke(new Action(() => this.Refresh()));
            } 
        }
        #endregion
    }
}
