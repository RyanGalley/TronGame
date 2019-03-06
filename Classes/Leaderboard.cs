using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;

//Text renderer and font used to work out the correct size of objects
using TR = System.Windows.Forms.TextRenderer;
using Font = System.Drawing.Font;


namespace TronGame
{
    class Leaderboard : ControlManager
    {
        //GAMEDATA STRUCT
        private TronGame.GameData data;

        //OBJECTS
        private Label NewRound, Exit;

        //OBJECT LISTS
        private List<Label> Headings = new List<Label>();
        private List<Label> Names = new List<Label>();
        private List<Label> Wins = new List<Label>();
        private List<Label> Eliminations = new List<Label>();
        private List<Label> Crashes = new List<Label>();
        private List<Label> SelfCrashes = new List<Label>();
        private List<List<Label>> StatLabels = new List<List<Label>>();
        

        //CONSTRUCTOR
        public Leaderboard(Game game, TronGame.GameData set) : base(game)
        {
            data = set;
            data.newRound = false;
        }

        public override void InitializeComponent()
        {
            //Creates a System.Drawing.Font of my default font
            Font font = new Font("Ariel", 24);

            //Adds each list of game stat labels to StatLabels
            StatLabels.AddRange(new List<List<Label>> { Names, Wins, Eliminations, Crashes, SelfCrashes });

            //Sets the height and width of the screen
            int x = (int)data.size.X;
            int y = (int)data.size.Y;

            //Sets default winner name and end text
            string winnerName = "";
            string endText = " Wins!";
            try
            {
                winnerName = data.names[data.winner - 1];
            }
            //If there is no winner, catch and change the text shown
            catch
            {
                endText = "Draw!";
            }

            //WINNER LABEL
            var Winner = new Label
            {
                Text = winnerName + endText,
                BackgroundColor = Color.MidnightBlue,
                TextColor = Color.White,                
            };            
            Winner.Size = new Vector2(TR.MeasureText(Winner.Text, font).Width, TR.MeasureText(Winner.Text, font).Height);
            Winner.Location = new Vector2((x / 2) - Winner.Size.X / 2, (y / 16) * 3 - Winner.Size.Y / 2);
            this.Controls.Add(Winner);

            //Creates a list of the column headings as well as a single list containing all the player data
            List<string> HeadingNames = new List<string> { "Names", "Wins", "Eliminations", "Crashes", "SelfCrashes" };
            List<IList> playerData = new List<IList> { data.names, data.wins, data.eliminations, data.crashes, data.selfCrashes };

            //For each column in the table
            for (int i = 0; i < StatLabels.Count; i++)
            {
                //COLUMN HEADING LABELS
                Headings.Add(new Label
                {
                    Text = HeadingNames[i],
                    BackgroundColor = Color.Black,
                    TextColor = Color.White,                    
                });
                Headings[i].Size = new Vector2(TR.MeasureText(Headings[i].Text, font).Width, TR.MeasureText(Headings[i].Text, font).Height);
                Headings[i].Location = new Vector2((x / 6 * (i + 1)) - Headings[i].Size.X / 2, y / 16 * 6 - Headings[i].Size.Y / 2);
                this.Controls.Add(Headings[i]);

                //For each row of data
                for (int j = 0; j <data.num; j++)
                {
                    //STAT LABELS
                    StatLabels[i].Add(new Label
                    {
                        Text = playerData[i][j].ToString(),
                        BackgroundColor = Color.Black,
                        TextColor = Color.White,
                    });
                    StatLabels[i][j].Size = new Vector2(TR.MeasureText(StatLabels[i][j].Text, font).Width, TR.MeasureText(StatLabels[i][j].Text, font).Height);
                    StatLabels[i][j].Location = new Vector2((x / 6 * (i + 1)) - StatLabels[i][j].Size.X / 2, y / 16 * (7+j) - Headings[i].Size.Y / 2);
                    this.Controls.Add(StatLabels[i][j]);
                }
            }

            //NEWROUND BUTTON
            NewRound = new Label
            {
                Text = "New Round",
                BackgroundColor = Color.MidnightBlue,
                TextColor = Color.White,
                Size = new Vector2(156, 36),
            };
            NewRound.Location = new Vector2((x / 20) * 5 - NewRound.Size.X / 2, y / 16 * 12 - NewRound.Size.Y / 2);
            NewRound.Clicked += StartNewRound;
            NewRound.MouseEnter += NewRound_MouseOver;
            NewRound.MouseLeave += NewRound_MouseOff;
            this.Controls.Add(NewRound);

            //EXIT BUTTON
            Exit = new Label
            {
                Text = "Exit",
                BackgroundColor = Color.MidnightBlue,
                TextColor = Color.White,
                Size = new Vector2(51, 37),
            };
            Exit.Location = new Vector2((x / 20) * 15 - Exit.Size.X / 2, y / 16 * 12 - Exit.Size.Y / 2);
            Exit.Clicked += ExitGame;
            Exit.MouseEnter += Exit_MouseOver;
            Exit.MouseLeave += Exit_MouseOff;
            this.Controls.Add(Exit);
        }

        //Upon mousing over "NewRound"
        private void NewRound_MouseOver(object sender, EventArgs e)
        {
            NewRound.BackgroundColor = Color.Blue;
        }

        //Upon mousing over "Exit"
        private void Exit_MouseOver(object sender, EventArgs e)
        {
            Exit.BackgroundColor = Color.Blue;
        }

        //Upon mouse leaving "NewRound"
        private void NewRound_MouseOff(object sender, EventArgs e)
        {
            NewRound.BackgroundColor = Color.MidnightBlue;
        }

        //Upon mouse leaving "Exit"
        private void Exit_MouseOff(object sender, EventArgs e)
        {
            Exit.BackgroundColor = Color.MidnightBlue;
        }

        //Upon clicking "NewRound"
        private void StartNewRound(object sender, EventArgs e)
        {
            data.newRound = true;
        }

        //Upon clicking "Exit"
        private void ExitGame(object sender, EventArgs e)
        {
            data.exitGame = true;
        }
    }
}
