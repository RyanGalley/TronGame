using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

//Gives the different colour types unique names
using MColor = Microsoft.Xna.Framework.Color;

namespace TronGame
{
    public partial class StartMenu : Form
    {
        //SETTINGS STRUCT
        public class GameSettings
        {
            public bool isGameCreated;
            public int num;
            public List<MColor> Colours;
            public List<string> Names;
        }
        private GameSettings settings;


        //CONSTRUCTOR
        public StartMenu(GameSettings set)     
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

            //LOGO IMAGE
            var logo = new PictureBox
            {
                Name = "logo",
                AutoSize = true,             
                Image = Image.FromFile("Images/Start Menu/Logo.png")                
            };            
            this.Controls.Add(logo);
            logo.Location = new Point(midX - (logo.Width / 2), midY - (logo.Width / 2));

            //SINGLE PLAYER BUTTON
            var SinglePlayer = new PictureBox
            {                
                Name = "SinglePlayer",                          
                AutoSize = true,
                Image = Image.FromFile("Images/Start Menu/Single Player.png")              
            };
            SinglePlayer.Click += new EventHandler(SinglePlayer_Click);
            this.Controls.Add(SinglePlayer);
            SinglePlayer.Location = new Point((this.Width / 3) - (SinglePlayer.Width / 2), (midY / 2) * 3);

            //MULTIPLAYER BUTTON
            var Multiplayer = new PictureBox
            {
                Name = "Multiplayer",
                AutoSize = true,
                Image = Image.FromFile("Images/Start Menu/Multiplayer.png")    
            };
            Multiplayer.Click += new EventHandler(Multiplayer_Click);
            this.Controls.Add(Multiplayer);
            Multiplayer.Location = new Point(((this.Width / 3) * 2) - (Multiplayer.Width / 2), (midY / 2) * 3);
        }

        //Upon clicking "Single Player" button
        private void SinglePlayer_Click(object sender, EventArgs e)
        {
            //Creates and opens new SinglePlayerMenu
            var SinglePlayerMenu = new SinglePlayerMenu(settings);
            SinglePlayerMenu.Closed += (s, args) => this.Close();
            SinglePlayerMenu.Show();
            //Allows a pause for the form to open in the background so that the screen doesn't flash
            Thread.Sleep(20);
            this.Hide();
        }

        //Upon clicking "Multiplayer" button
        private void Multiplayer_Click(object sender, EventArgs e)
        {
            //Creates and opens new MultiplayerMenu
            var MultiplayerMenu = new MultiplayerMenu(settings);
            MultiplayerMenu.Closed += (s, args) => this.Close();
            MultiplayerMenu.Show();
            //Allows a pause for the form to open in the background so that the screen doesn't flash
            Thread.Sleep(20);
            this.Hide();
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
    }
}
