using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TronGame
{
    class Player
    {
        //PUBLIC VARIABLES
        public Bike bike;
        public Color colour;
        public string name;
        public int wins;
        public int eliminations;
        public int crashes;
        public int selfCrashes;


        //CONSTRUCTOR
        public Player(string TheName, Color Colour, Vector2 Pos, Vector2 Velocity, Bike.Direction Dir, Keys[] Keys, int Size)
        {
            name = TheName;
            colour = Colour;
            //Creates this players bike
            bike = new Bike(Pos, Velocity, Dir, Keys, Size);
        }

        public void CreateNewBike(Vector2 Pos, Vector2 Velocity, Bike.Direction Dir, Keys[] Keys, int Size)
        {
            //Overwrites last rounds bike with a fresh one
            bike = new Bike(Pos, Velocity, Dir, Keys, Size);
        }
        
    }
}
