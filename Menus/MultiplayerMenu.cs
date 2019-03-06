using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;

//Gives the different colour types unique names
using MColor = Microsoft.Xna.Framework.Color;

namespace TronGame
{
    public partial class MultiplayerMenu : Form
    {

        //OBJECTS
        PictureBox[] PlayerLabels = new PictureBox[4];
        Button[] ColourButtons = new Button[4];
        TextBox[] NameBoxes = new TextBox[4];
        RadioButton[] RadioButtons = new RadioButton[3];
        Label[] PlayerNumLabels = new Label[3];

        //VARIABLES
        public int numOfPlayers = 4;
        public List<string> PlayerNames = new List<string>();  
        
        //List of XNA.Framework colours
        public List<MColor> MPlayerColours = new List<MColor>();
        //List of System.Drawing colours
        List<Color> SPlayerColours = new List<Color>();

        //SETTINGS STRUCT
        private StartMenu.GameSettings settings;

        //CONSTRUCTOR
        public MultiplayerMenu(StartMenu.GameSettings set)
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

            //MULTIPLAYER TITLE
            var Multiplayer = new PictureBox
            {
                Name = "Multiplayer",
                AutoSize = true,
                Image = Image.FromFile("Images/Multiplayer Menu/Multiplayer Heading.png")
            };
            this.Controls.Add(Multiplayer);
            Multiplayer.Location = new Point(midX - (Multiplayer.Width / 2), 0);

            //NUMBER OF PLAYERS LABEL
            var NumOfPlayers = new PictureBox
            {
                Name = "NumOfPlayers",
                AutoSize = true,
                Image = Image.FromFile("Images/Multiplayer Menu/Number Of Players.png")
            };
            this.Controls.Add(NumOfPlayers);
            NumOfPlayers.Location = new Point(midX - (NumOfPlayers.Width / 2), (this.Height / 16) * 5 - (NumOfPlayers.Height / 2));

            //RADIO BUTTONS
            for (int i = 0; i < 3; i++)
            {
                RadioButtons[i] = new RadioButton
                {
                    Name = "RadioButton" + (i + 2),
                    Text = (i + 2).ToString(),
                    AutoSize = true,
                    ForeColor = Color.White,
                    Font = new Font("Ariel", 15, FontStyle.Bold),
                    AutoCheck = true,
                    Cursor = Cursors.Cross,                        
                };
                this.Controls.Add(RadioButtons[i]);
                RadioButtons[i].Location = new Point((this.Width / 10) * (i + 4), (this.Height / 16) * 6 - (RadioButtons[i].Height / 2));

            }

            RadioButtons[2].Checked = true;

            RadioButtons[0].Click += new EventHandler(TwoPlayers);
            RadioButtons[1].Click += new EventHandler(ThreePlayers);
            RadioButtons[2].Click += new EventHandler(FourPlayers);

            
            for (int i = 0; i < 4; i++)
            {
                //PLAYER LABELS
                PlayerLabels[i] = new PictureBox
                {
                    Name = "PlayerLabel" + (i + 1),
                    AutoSize = true,
                    Image = Image.FromFile("Images/Multiplayer Menu/Player " + (i + 1) + ".png")
                };
                this.Controls.Add(PlayerLabels[i]);
                PlayerLabels[i].Location = new Point(((this.Width / 5) * (i + 1)) - (PlayerLabels[i].Width / 2), (this.Height / 16) * 8 - (PlayerLabels[i].Height / 2));

                //NAME TEXT BOXES
                NameBoxes[i] = new TextBox
                {
                    Name = "NameBox" + (i + 1),
                    AutoSize = false,
                    Font = new Font("Ariel", 20, FontStyle.Bold),
                    Size = new Size(this.Width / 10, 38),
                    MaxLength = 18,
                };
                NameBoxes[i].KeyPress += new KeyPressEventHandler(TextResize);
                this.Controls.Add(NameBoxes[i]);
                NameBoxes[i].Location = new Point(((this.Width / 5) * (i + 1)) - (NameBoxes[i].Width / 2), (this.Height / 16) * 10 - (NameBoxes[i].Height / 2));
           
                //COLOUR BUTTONS
                ColourButtons[i] = new Button
                {                    
                    Name = "ColourButton" + (i + 1),
                    Size = new Size(this.Width / 10, this.Height / 20),
                };
                ColourButtons[i].Click += new EventHandler(ColourButton_Click);
                this.Controls.Add(ColourButtons[i]);
                ColourButtons[i].Location = new Point(((this.Width / 5) * (i + 1)) - (ColourButtons[i].Width / 2), (this.Height / 16) * 12 - (ColourButtons[i].Height / 2));

            }
            //Default colours
            ColourButtons[0].BackColor = Color.MidnightBlue;
            ColourButtons[1].BackColor = Color.DarkRed;
            ColourButtons[2].BackColor = Color.Yellow;
            ColourButtons[3].BackColor = Color.DarkGreen;

            //NAME LABEL
            var Name = new PictureBox
            {
                Name = "Name",
                AutoSize = true,
                Image = Image.FromFile("Images/Multiplayer Menu/Name.png")
            };
            this.Controls.Add(Name);
            Name.Location = new Point((NameBoxes[0].Left / 2) - (Name.Width / 2), (this.Height / 16) * 10 - (Name.Height / 2));

            //COLOUR LABEL
            var Colour = new PictureBox
            {
                Name = "Colour",
                AutoSize = true,
                Image = Image.FromFile("Images/Multiplayer Menu/Colour.png")
            };
            this.Controls.Add(Colour);
            Colour.Location = new Point((NameBoxes[0].Left / 2) - (Colour.Width / 2), (this.Height / 16) * 12 - (Colour.Height / 2));

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
                Image = Image.FromFile("Images/Multiplayer Menu/Start Game.png")
            };
            StartGame.Click += new EventHandler(GameStart);
            this.Controls.Add(StartGame);
            StartGame.Location = new Point(midX - (StartGame.Width / 2), (this.Height) - (StartGame.Height));    
        }

        //Upon clicking "Start Game"
        private void GameStart(object sender, EventArgs e)
        {
            GrabVariables();          

            //Hides all objects
            foreach (Control obj in Controls)
            {
                obj.Hide();
            }

            //SET SETTINGS 
            settings.isGameCreated = true;
            settings.num = numOfPlayers;
            settings.Colours = MPlayerColours;
            settings.Names = PlayerNames;
            
            this.Close();
        }

        private void GrabVariables()
        {
            MColor x;
            for (int i = 0; i < numOfPlayers; i++)
            {
                //Convert the Winforms colours of the colour buttons to MonoGame colours
                SPlayerColours.Add(ColourButtons[i].BackColor);
                Color temp = SPlayerColours[i];
                x = new MColor(temp.R, temp.G, temp.B, temp.A);
                MPlayerColours.Add(x);

                //Grab font sizes and player names from text boxes                
                //Check names contain a letter or number. If not just assign a number as default
                if (Regex.IsMatch(NameBoxes[i].Text, @"[a-zA-Z0-9]"))
                {
                    PlayerNames.Add(NameBoxes[i].Text);
                    PlayerNames[i].Trim();
                }
                else PlayerNames.Add((i + 1).ToString());
            }
        }

        //Upon clicking Radio Button 2
        private void TwoPlayers(object sender, EventArgs e)
        {
            ColourButtons[2].Hide();
            ColourButtons[3].Hide();

            NameBoxes[2].Hide();
            NameBoxes[3].Hide();

            numOfPlayers = 2;
        }

        //Upon clicking Radio Button 3
        private void ThreePlayers(object sender, EventArgs e)
        {
            ColourButtons[2].Show();
            ColourButtons[3].Hide();

            NameBoxes[2].Show();
            NameBoxes[3].Hide();

            numOfPlayers = 3;
        }

        //Upon clicking Radio Button 4
        private void FourPlayers(object sender, EventArgs e)
        {
            ColourButtons[2].Show();
            ColourButtons[3].Show();

            NameBoxes[2].Show();
            NameBoxes[3].Show();

            numOfPlayers = 4;
        }

        //Upon typing into a text box
        //automatically resize text to fit text box perfectly
        private void TextResize(object sender, KeyPressEventArgs e)
        {            
            bool edge = false;

            //Combines text curently in textbox with key pressed to find resulting string 
            char keyPressed = e.KeyChar;
            TextBox box = (TextBox)sender;
            string textUpdate = box.Text + keyPressed;

            //Loops until edge is true (until the biggest fitting font size is found)
            while (true)
            {
                //If the fontsize is the deafault, edge defaults to true 
                if (box.Font.Size == 20) edge = true;
                //Measures the size of the updated text in both dimensions
                Size text = TextRenderer.MeasureText(textUpdate, box.Font);

                //If the updated text is longer than the textbox, reduce the font size by one, unless the font size is a minimum
                if (text.Width >= box.Width && box.Font.Size != 1) box.Font = new Font(box.Font.FontFamily, box.Font.Size - 1, box.Font.Style);

                //If the text fits the text box, checks if it is smaller than it and that font size is not a maximum
                else if ((text.Width < box.Width) && box.Font.Size < 20)
                {
                    //Creates a new font with fontsize increased by one and calculates its size
                    Font bigFont = new Font(box.Font.FontFamily, box.Font.Size + 1, box.Font.Style);
                    text = TextRenderer.MeasureText(textUpdate, bigFont);

                    //If the larger text size still fits the text box, change the font size to the greater size
                    if (text.Width < box.Width) box.Font = bigFont;
                    //Otherwise the font must be the biggest possible so edge is true
                    else edge = true;
                }

                //Measures the text size again as a double check that it fits
                text = TextRenderer.MeasureText(textUpdate, box.Font);
                //If it fits and it is the biggest possible or the fontsize is a minimum, break
                if ((text.Width < box.Width && text.Height < box.Height && edge) || box.Font.Size == 1) break;
            }

        }

        //The window that opens when clicking the colour buttons
        ColorDialog colorDialog1 = new ColorDialog();

        //Upon clicking colour button
        private void ColourButton_Click(object sender, EventArgs e)
        {
            //sets the colour button to the colour chosen to show selection
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
