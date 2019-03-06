using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace TronGame
{
    public partial class SinglePlayerMenu : Form
    {
        //SETTINGS STRUCT
        private StartMenu.GameSettings settings;

        //CONSTRUCTOR
        public SinglePlayerMenu(StartMenu.GameSettings set)
        {
            settings = set;
            InitializeComponent();

            //Put form into fullscreen and make background black
            FormBorderStyle = FormBorderStyle.None;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.FromArgb(0, 0, 0);

            //Find middle points of form for positiong objects
            int midX, midY;
            midX = this.Width / 2;
            midY = this.Height / 2;

            this.KeyDown += new KeyEventHandler(Form_KeyDown);

            //EXIT BUTTON
            var Exit = new PictureBox
            {
                Name = "Exit",
                AutoSize = true,
                Image = Image.FromFile("Images/All Menus/Exit.png")
            };
            Exit.Click += new EventHandler(Exit_Click);
            this.Controls.Add(Exit);
            Exit.Location = new Point((this.Width / 15) * 14 - Exit.Width / 2, 0);

            //BACK BUTTON
            var Back = new PictureBox
            {
                Name = "Back",
                AutoSize = true,
                Image = Image.FromFile("Images/All Menus/Back.png")
            };
            Back.Click += new EventHandler(Back_Click);
            this.Controls.Add(Back);
            Back.Location = new Point((this.Width / 15) - Exit.Width / 2, 0);
                        
            //SINGLE PLAYER TITLE
            var SinglePlayer = new PictureBox
            {
                Name = "SinglePlayer",
                AutoSize = true,
                Image = Image.FromFile("Images/Single Player Menu/Single Player Heading.png")
            };
            this.Controls.Add(SinglePlayer);
            SinglePlayer.Location = new Point(midX - (SinglePlayer.Width / 2), 0);

            //NAME LABEL
            var Name = new PictureBox
            {
                Name = "Name",
                AutoSize = true,
                Image = Image.FromFile("Images/Single Player Menu/Name.png")
            };
            this.Controls.Add(Name);
            Name.Location = new Point(midX - (Name.Width / 2), (this.Height / 16) * 4 - (Name.Height / 2));

            //NAME TEXT BOX
            var NameEntry = new TextBox
            {
                Name = "NameEntry",
                Font = new Font("Ariel", 30, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center,
                AutoSize = true,
                Width = this.Width / 3,
                MaxLength = 18,
            };
            this.Controls.Add(NameEntry);
            NameEntry.Location = new Point(midX - (NameEntry.Width / 2), (this.Height / 16) * 6 - (NameEntry.Height / 2));
            
            //COLOUR LABEL
            var Colour = new PictureBox
            {
                Name = "Colour",
                AutoSize = true,
                Image = Image.FromFile("Images/Single Player Menu/Colour.png")
            };
            this.Controls.Add(Colour);
            Colour.Location = new Point(midX - (Colour.Width / 2), (this.Height / 16) * 8 - (Colour.Height / 2));

            //COLOUR BUTTON
            var ColourEntry = new Button
            {
                Name = "ColourEntry",
                Size = new Size(NameEntry.Width / 2, NameEntry.Height),                
                BackColor = Color.MidnightBlue,
            };
            ColourEntry.Click += new EventHandler(ColourEntry_Click);
            this.Controls.Add(ColourEntry);
            ColourEntry.Location = new Point(midX - (ColourEntry.Width / 2), (this.Height / 16) * 10 - (ColourEntry.Height / 2));

            //SETTINGS LABEL
            var Settings = new PictureBox
            {
                Name = "Settings",
                AutoSize = true,
                Image = Image.FromFile("Images/All Menus/Settings.png")
            };
            this.Controls.Add(Settings);
            Settings.Location = new Point((this.Width / 8) * 1 - Settings.Width / 2, (this.Height) - (Settings.Height));

            //HELP LABEL
            var Help = new PictureBox
            {
                Name = "Help",
                AutoSize = true,
                Image = Image.FromFile("Images/All Menus/Help.png")
            };
            this.Controls.Add(Help);
            Help.Location = new Point((this.Width / 8) * 7 - Help.Width / 2, (this.Height) - (Help.Height));

            //START GAME BUTTON
            var StartGame = new PictureBox
            {
                Name = "StartGame",
                AutoSize = true,
                Image = Image.FromFile("Images/Single Player Menu/Start Game.png")
            };
            this.Controls.Add(StartGame);
            StartGame.Location = new Point(midX - (StartGame.Width / 2), (this.Height) - (StartGame.Height));           
        }

        //The window that opens when clicking the colour buttons
        ColorDialog colorDialog1 = new ColorDialog();

        //Upon clicking colour button
        private void ColourEntry_Click(object sender, EventArgs e)
        {
            //Sets the colour button to the colour chosen to show selection
            colorDialog1.ShowDialog();
            Button b = (Button)sender;
            b.BackColor = colorDialog1.Color;
        }

        //Upon pressing a key wih form selected
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            //If escape pressed, exit programme
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        //Upon clicking "Exit"
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Upon clicking "Back"
        private void Back_Click(object sender, EventArgs e)
        {            
            //Creates and opens new StartMenu 
            var startMenu = new StartMenu(settings);
            startMenu.Closed += (s, args) => this.Close();
            startMenu.Show();
            //Allows a pause for the form to open in the background so that the screen doesn't flash
            Thread.Sleep(20);
            this.Hide();
        }
    }
}
