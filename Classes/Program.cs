using System;
using System.Windows.Forms;

namespace TronGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static StartMenu SMenu;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]       

        static void Main()
        {
            //SETTINGS STRUCT
            //Used to pass setting variables between menus and game
            StartMenu.GameSettings settings = new StartMenu.GameSettings();


            //Create and run the menu form
            SMenu = new StartMenu(settings);
            Application.Run(SMenu);

            //If start game was pressed
            if (settings.isGameCreated)
            {
                //Create and run the game
                TronGame newGame = new TronGame(settings.num, settings.Colours, settings.Names);
                newGame.Run();
            }

        }
    }
#endif
}
