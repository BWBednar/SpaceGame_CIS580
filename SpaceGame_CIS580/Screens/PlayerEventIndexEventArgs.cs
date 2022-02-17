﻿/**
 * Starting Code from Nathan Bean's GameArchitectureExample project
 */

using System;
using Microsoft.Xna.Framework;

namespace SpaceGame_CIS580.Screens
{
    // Custom event argument which includes the index of the player who
    // triggered the event. This is used by the MenuEntry.Selected event.
    public class PlayerIndexEventArgs : EventArgs
    {
        public PlayerIndex PlayerIndex { get; }

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}