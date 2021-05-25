using System.Windows.Forms;
using System.Drawing;

namespace TowerDefense.View
{
    /// <summary>
    /// Nehézség kiválasztó dialógus ablak
    /// </summary>
    public static class DiffDialog 
    {
        public static int ShowDialog(string text, string caption)
        {
            Form prompt = new Form();
            prompt.Width = 400;
            prompt.Height = 150;
            prompt.MinimumSize = new Size(prompt.Width, prompt.Height);
            prompt.MaximumSize = new Size(prompt.Width, prompt.Height);
            prompt.Text = caption;
            Label textLabel = new Label() { Left = 100, Top = 20, Text = text, Width = 150, Height = 100, Font = new Font(FontFamily.GenericSerif, 7) };
            int buttonWidth = 80;
            int buttonHeight = 75;
            Button easyButton = new Button() { Text = "Könnyű", Left = 5, Top = buttonHeight, Width = buttonWidth, Font = new Font(FontFamily.GenericSerif, 7)};
            Button mediumButton = new Button() { Text = "Közepes", Left = 100, Top = buttonHeight, Width = buttonWidth, Font = new Font(FontFamily.GenericSerif, 7) };
            Button hardButton = new Button() { Text = "Nehéz", Left = 200, Top = buttonHeight, Width = buttonWidth, Font = new Font(FontFamily.GenericSerif, 7) };
            Button closeButton = new Button() { Text = "Kilépés", Left = 300, Top = buttonHeight, Width = buttonWidth, Font = new Font(FontFamily.GenericSerif, 7) };
            int diff = 0;
            easyButton.Click += (sender, e) => { diff = 1; prompt.Close(); };
            mediumButton.Click += (sender, e) => { diff = 2; prompt.Close(); };
            hardButton.Click += (sender, e) => { diff = 3; prompt.Close(); };
            closeButton.Click += (sender, e) => { diff = -1; prompt.Close(); };
            prompt.Controls.Add(easyButton);
            prompt.Controls.Add(mediumButton);
            prompt.Controls.Add(hardButton);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(closeButton);
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.ShowDialog();
            return diff;
        }
    }
}
