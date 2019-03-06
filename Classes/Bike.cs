using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace TronGame
{
    class Bike
    {
        //ENUM
        public enum Direction
        {
            up,
            left,
            down,
            right,
        }

        //PUBLIC VARIABLES
        public List<Rectangle> Rects = new List<Rectangle>();
        public Rectangle bikeTip;
        public bool crashed;
        public bool selfCrash;

        //PRIVATE VARIABLES
        private int turnDistance = 0;
        private int speed = 2;        
        private int bikeSize;
        private bool turning = false;
        private bool bikeUpdated = true;
        private Vector2 velocity;
        private Keys[] controlKeys;
        private Direction direction;


        //CONSTRUCOR
        public Bike(Vector2 Pos, Vector2 Velocity, Direction Dir, Keys[] Keys, int Size)
        {
            direction = Dir;
            velocity = Velocity;
            controlKeys = Keys;
            bikeSize = Size;
            Rectangle firstRect = new Rectangle((int)Pos.X, (int)Pos.Y, Size, Size);
            Rects.Add(firstRect);
        }
                
        public void CrashCheck(int screenWidth, int screenHeight)
        {
            //Checks if bike intersects with itself, not checking the three most recent rectangles
            for (int i = 0; i < Rects.Count - 3; i++)
            {
                if (bikeTip.Intersects(Rects[i]))
                {
                    crashed = true;
                    selfCrash = true;
                }
            }
            //Checks if bike is off of the edge of the screen
            if (bikeTip.X < 0 || bikeTip.Y < 0 || bikeTip.X > screenWidth || bikeTip.Y > screenHeight)
            {
                crashed = true;
                selfCrash = true;
            }
        }

        public void Update()
        {
            //If bike not crashed
            if (!crashed)
            {
                //And if bike hasn't just turned
                if (!turning)
                {                    
                    KeyboardState ks = Keyboard.GetState();
                    var keys = ks.GetPressedKeys();

                    int count = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        for (int j = 0; j < controlKeys.Count(); j++)
                        {
                            if (keys[i] == controlKeys[j]) count++;
                        }
                    }

                    //Checks if a player is only pressing one of their keys. Inputs only accepted if true
                    if (count <= 1)
                    {
                        //Changes player direction based on input
                        if (ks.IsKeyDown(controlKeys[0]) && direction != Direction.up && direction != Direction.down)
                        {
                            velocity.X = 0;
                            velocity.Y = -speed;
                            bikeUpdated = true;
                            turning = true;
                        }
                        if (ks.IsKeyDown(controlKeys[1]) && direction != Direction.left && direction != Direction.right)
                        {
                            velocity.X = -speed;
                            velocity.Y = 0;
                            bikeUpdated = true;
                            turning = true;
                        }
                        if (ks.IsKeyDown(controlKeys[2]) && direction != Direction.down && direction != Direction.up)
                        {
                            velocity.X = 0;
                            velocity.Y = speed;
                            bikeUpdated = true;
                            turning = true;
                        }
                        if (ks.IsKeyDown(controlKeys[3]) && direction != Direction.right && direction != Direction.left)
                        {
                            velocity.X = speed;
                            velocity.Y = 0;
                            bikeUpdated = true;
                            turning = true;
                        }
                    }
                }

                CreateRects();
                //These have to be done as two seperate methods so that CreateSnakeTip has access to the new rect
                CreateSnakeTip();

                //Updates direction after creating rectangles so that they can access to the previous direction
                if (velocity.X < 0) direction = Direction.left;
                if (velocity.X > 0) direction = Direction.right;
                if (velocity.Y < 0) direction = Direction.up;
                if (velocity.Y > 0) direction = Direction.down;
            }

            //Ensures the bikes can only turn after one full square is drawn (+ 2 pixels)         
            if (turning)
            {
                turnDistance += speed;
                if (turnDistance >= bikeSize + speed + 2)
                {
                    turnDistance = 0;
                    turning = false;
                }
            }
        }

        public void CreateSnakeTip()
        {
            Rectangle lastRect = Rects[Rects.Count - 1];
            //If direction has just changed
            if (bikeUpdated)
            {
                //If started moving up or down
                if (velocity.Y != 0)
                {
                    bikeTip.Height = 1;
                    bikeTip.Width = bikeSize - 2;
                    bikeTip.X = lastRect.X + 1;

                    //If was moving down before update
                    if (Rects.Count() == 1 && direction == Direction.down)  bikeTip.Y = lastRect.Y + bikeSize;
                    
                    else bikeTip.Y = lastRect.Y;
                }                

                //If started moving left or right
                if (velocity.X != 0)
                {
                    bikeTip.Height = bikeSize - 2;
                    bikeTip.Width = 1;
                    bikeTip.Y = lastRect.Y + 1;

                    //If was moving right before update
                    if (Rects.Count() == 1 && direction == Direction.right) bikeTip.X = lastRect.X + bikeSize;

                    else bikeTip.X = lastRect.X;
                }
                bikeUpdated = false;
            }
            //If not updated
            else
            {
                //If moving left
                if (velocity.X < 0)
                {
                    bikeTip.X = lastRect.X;
                    bikeTip.Y = lastRect.Y + 1;
                }
                //If moving up
                if (velocity.Y < 0)
                {
                    bikeTip.X = lastRect.X + 1;
                    bikeTip.Y = lastRect.Y;
                }
                //If moving right
                if (velocity.X > 0)
                {
                    bikeTip.X = lastRect.X + lastRect.Width;
                    bikeTip.Y = lastRect.Y + 1;
                }
                //If moving down
                if (velocity.Y > 0)
                {
                    bikeTip.X = lastRect.X + 1;
                    bikeTip.Y = lastRect.Y + lastRect.Height;
                }
            }
        }


        public void CreateRects()
        {
            //If updated
            if (bikeUpdated)
            {
                Rectangle lastRect = Rects[Rects.Count - 1];
                Rectangle newR = new Rectangle(0, 0, 0, 0);
                newR.Width = bikeSize;
                newR.Height = bikeSize;

                //If started moving up or down
                if (velocity.Y != 0)
                {
                    newR.Y = lastRect.Y;
                    //From moving right
                    if (direction == Direction.right) newR.X = (lastRect.Width - bikeSize) + lastRect.X;
                    //From moving left
                    else newR.X = lastRect.X;
                }

                //If started moving left or right
                if (velocity.X != 0)
                {
                    newR.X = lastRect.X;
                    //From moving down
                    if (direction == Direction.down) newR.Y = (lastRect.Height - bikeSize) + lastRect.Y;
                    //From moving up
                    else newR.Y = lastRect.Y;
                }

                //If rectangle has a lenght and width and is new, add it to Rects
                if (!newR.IsEmpty && newR != lastRect) Rects.Add(newR);
            }
            else
            {
                var rect = Rects[Rects.Count - 1];
                //If moving left
                if (velocity.X < 0)
                {
                    rect.X += (int)velocity.X;
                    rect.Width -= (int)velocity.X;
                }
                //If moving up
                else if (velocity.Y < 0)
                {
                    rect.Y += (int)velocity.Y;
                    rect.Height -= (int)velocity.Y;
                }
                //If moving down or right
                else
                {
                    rect.Width += (int)velocity.X;
                    rect.Height += (int)velocity.Y;
                }
                Rects[Rects.Count - 1] = rect;
            }
        }

    }
}
