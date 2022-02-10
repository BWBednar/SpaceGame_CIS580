/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using System.Collections.Generic;
using System.Text;
using SpaceGame_CIS580.StateManagement;

namespace SpaceGame_CIS580
{
    // Our game's implementation of IScreenFactory which can handle creating the screens
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}
