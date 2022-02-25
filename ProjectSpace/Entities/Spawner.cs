using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ProjectSpace.Components;

namespace ProjectSpace.Entities
{
    public class Spawner : Entity
    {
        public Spawner()
        {
            new AsteroidSpawner(this);
            new PickupSpawner(this);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
