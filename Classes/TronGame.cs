using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TronGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TronGame : Game
    {
        //GAMEDATA STRUCT
        public class GameData
        {
            public bool newRound;
            public bool exitGame;
            public Vector2 size;
            public int num;
            public int winner;
            public List<string> names;
            public List<int> wins;
            public List<int> eliminations;
            public List<int> crashes;
            public List<int> selfCrashes;
        }
        GameData data = new GameData();

        //GRAPHICS
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D rectTexture;
        Texture2D spaceToStart;
        
        //LEADERBOARD
        Leaderboard leaderboard;
        
        //PLAYER KEYS
        Keys[] P1Keys = { Keys.W,    Keys.A,      Keys.S,    Keys.D        };
        Keys[] P2Keys = { Keys.Up,   Keys.Left,   Keys.Down, Keys.Right    };
        Keys[] P3Keys = { Keys.I,    Keys.J,      Keys.K,    Keys.L        };
        Keys[] P4Keys = { Keys.Home, Keys.Delete, Keys.End,  Keys.PageDown };
        
        //CONSTANTS
        int speed = 2;
        int bikeSize = 10;

        //VARIABLES
        int numOfPlayers;
        bool gameStarted = false;
        bool inLeaderboard = false;

        //LISTS
        List<Player> Players = new List<Player>();
        List<Color> PlayerColours = new List<Color>();
        List<string> PlayerNames = new List<string>();
        List<Keys[]> PlayerKeys = new List<Keys[]>();
        List<Vector2> StartingPositions = new List<Vector2>();
        List<Vector2> StartingVelocity = new List<Vector2>();
        List<Bike.Direction> StartingDirections = new List<Bike.Direction>();        
        
        //CONSTRUCTOR
        public TronGame(int num, List<Color> Colours, List<string> Names)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            numOfPlayers = num;
            PlayerColours = Colours;
            PlayerNames = Names;
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = false;
            data.size = new Vector2(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);

            //Sets correct resolution and enables fullscreen
            graphics.PreferredBackBufferWidth = (int)data.size.X;
            graphics.PreferredBackBufferHeight = (int)data.size.Y;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            //Creates the lists of PlayerKeys for each player
            PlayerKeys.Add(P1Keys);
            PlayerKeys.Add(P2Keys);
            PlayerKeys.Add(P3Keys);
            PlayerKeys.Add(P4Keys);

            //Sets the effective boundaries of the map relative to the bikes
            int width = (int)data.size.X - bikeSize;
            int height = (int)data.size.Y - bikeSize;

            //Creates various lists for each bikes initial features
            StartingPositions.AddRange(new List<Vector2> { new Vector2(0, 0), new Vector2(width, height), new Vector2(0, height), new Vector2(width, 0) });
            StartingVelocity.AddRange(new List<Vector2> { new Vector2(speed, 0), new Vector2(-speed, 0), new Vector2(0, -speed), new Vector2(0, speed) });
            StartingDirections.AddRange(new List<Bike.Direction> { Bike.Direction.right, Bike.Direction.left, Bike.Direction.up, Bike.Direction.down });
            
            //Create player classes and pass in variables
            for (int i = 0; i < numOfPlayers; i++)
            {
                Players.Add(new Player(PlayerNames[i], PlayerColours[i], StartingPositions[i], StartingVelocity[i], StartingDirections[i], PlayerKeys[i], bikeSize));
            }

            base.Initialize();
        }

        private void NewRound()
        {
            //Resets the game to a starting state
            gameStarted = false;
            IsMouseVisible = false;
            //Creates new bikes for each player, replacing the previous rounds
            for (int i = 0; i < numOfPlayers; i++)
            {
                Players[i].CreateNewBike(StartingPositions[i], StartingVelocity[i], StartingDirections[i], PlayerKeys[i], bikeSize);                
            }
        }
        
        protected override void LoadContent()
        {
            //Creates a SpriteBatch used to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Loads my fonts and textures
            spaceToStart = Content.Load<Texture2D>(@"SpaceToStart");
            font = Content.Load<SpriteFont>(@"font/MyFont");
            rectTexture = Content.Load<Texture2D>(@"myTexture");
        }        

        protected override void Update(GameTime gameTime)
        {
            //If back or escape are pressed, or exitGame is true, exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || data.exitGame)
                Exit();

            //If space is pressed start the game
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) gameStarted = true;

            //If the leaderboard is currently being shown
            if (inLeaderboard)
            {
                //Update leaderboard to check for input
                leaderboard.Update(gameTime);
                //If exitGame is true, exit the game
                if (data.exitGame)
                {
                    Exit();
                }
                //if newRound is true, hide the leaderboard and start a new round
                else if (data.newRound)
                {
                    inLeaderboard = false;
                    leaderboard.Visible = false;
                    NewRound();
                }                
                
            }

            //If game is ongoing
            if (gameStarted && !inLeaderboard)
            {
                //Update each player and check if they have crashed into themselves
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].bike.Update();
                    Players[i].bike.CrashCheck((int)data.size.X, (int)data.size.Y);
                }
                //Check if any bikes have crashed into other players, or if the game has ended
                CheckBikeContact();
                CheckGameState();

                base.Update(gameTime);
            }            
        }        
        
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.Black);
            //Sets the correct location for the spaceToStart texture
            Vector2 mid = new Vector2((Window.ClientBounds.Width / 2) - (spaceToStart.Width / 2), (Window.ClientBounds.Height / 2) - (spaceToStart.Height / 2));

            spriteBatch.Begin();

            //If game has not started, draw the spaceToStart texture
            if (!gameStarted) spriteBatch.Draw(spaceToStart, mid);

            //For each player
            for (int i = 0; i < numOfPlayers; i++)
            {
                //Draw the starting square, regardless if game has started yet
                spriteBatch.Draw(rectTexture, Players[i].bike.Rects[0], Players[i].colour);
                if (gameStarted)
                {                     
                    for (int k = 0; k < Players[i].bike.Rects.Count; k++)
                    {
                        //Draw the players entire trail, in their colour
                        spriteBatch.Draw(rectTexture, Players[i].bike.Rects[k], Players[i].colour);
                    }
                    spriteBatch.Draw(rectTexture, Players[i].bike.bikeTip, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckBikeContact()
        {
            for (int A = 0; A < numOfPlayers; A++)
            {
                for (int B = 0; B < numOfPlayers; B++)
                {
                    //Compares each player with each other player
                    if (A != B)
                    {
                        for (int R = 0; R < Players[B].bike.Rects.Count; R++)
                        {
                            //If player A has hit any of player B's rects, B gets a kill and A is crashed
                            if (Players[A].bike.bikeTip.Intersects(Players[B].bike.Rects[R]))
                            {
                                //Only triggers once on the first instance of the player crashing
                                if (!(Players[A].bike.crashed)) Players[B].eliminations++;
                                Players[A].bike.crashed = true;
                            }
                        }
                    }
                }
            }

        }

        private void CheckGameState()
        {
            int count = 0;

            //Counts number of crashed players
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (Players[i].bike.crashed) count++;            
            }

            //If one or less players remaining, end the round
            if ((numOfPlayers - count) <= 1) EndRound();
        }
               
        private void EndRound()
        {
            data.names = new List<string>();
            data.wins = new List<int>();
            data.eliminations = new List<int>();
            data.crashes = new List<int>();
            data.selfCrashes = new List<int>();

            //For each player
            for (int i = 0; i < numOfPlayers; i++)
            {
                //If they crashed add to their crashes
                if (Players[i].bike.crashed)
                {
                    Players[i].crashes++;
                }
                //If they didn't crash, add to their wins and then set them as the winner 
                else
                {
                    Players[i].wins++;
                    data.winner = (i + 1);
                }

                //If they crashed into themselves, add to their selfCrashes
                if (Players[i].bike.selfCrash) Players[i].selfCrashes++;
            }                   
            
            //Set GameData
            data.num = numOfPlayers;
            for (int i = 0; i < numOfPlayers; i++)
            {
                data.names.Add(Players[i].name);
                data.wins.Add(Players[i].wins);
                data.eliminations.Add(Players[i].eliminations);
                data.crashes.Add(Players[i].crashes);
                data.selfCrashes.Add(Players[i].selfCrashes);
            }

            //Create and show leaderboard
            inLeaderboard = true;
            leaderboard = new Leaderboard(this, data);
            IsMouseVisible = true;
            Components.Add(leaderboard);
        }
    }
}
