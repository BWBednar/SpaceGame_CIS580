using System;

namespace SpaceGame_CIS580
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SpaceGame())
                game.Run();
        }
    }
}
