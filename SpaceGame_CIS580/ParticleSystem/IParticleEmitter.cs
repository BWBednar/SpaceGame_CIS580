/**
 * Starting code for this file taken from particle-system-example created by Nathan Bean
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceGame_CIS580.ParticleSystem
{
    public interface IParticleEmitter
    {
        public Vector2 Position { get; }

        public Vector2 Velocity { get; }
    }
}
