using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using ProjectSpace.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Components
{
    public class PickupSpawner : Component
    {
        private const float TIMER_MAX = 30.0f;
        private const float OFFSET = 32.0f;

        private Coroutine _routine;

        public PickupSpawner(Entity parent) : base(parent)
        {
            _routine = new Coroutine(parent, Spawner());
            _routine.Start();
        }

        private IEnumerator<double> Spawner()
        {
            yield return 3.0f;
            while (true)
            {
                if(Scene.GetEntity<Pickup>() == null)
                {
                    Vector2 position = new Vector2(
                    OFFSET + (float)Globals.Random.NextDouble() * (Camera.CameraSize.X - OFFSET*2),
                    OFFSET + (float)Globals.Random.NextDouble() * (Camera.CameraSize.Y - OFFSET*2)
                    );

                    Ship ship = Scene.GetEntity<Ship>();
                    if (ship != null && ship.GetComponent<BubbleShield>() == null)
                    {
                        while (Vector2.Distance(position, ship.GetComponent<Transform2D>().Position) < 64.0f)
                        {
                            position = new Vector2(
                            OFFSET + (float)Globals.Random.NextDouble() * (Camera.CameraSize.X - OFFSET*2),
                            OFFSET + (float)Globals.Random.NextDouble() * (Camera.CameraSize.Y - OFFSET*2)
                            );
                        }

                        var pickup = new PickupBubbleShield(position);

                        Scene.AddEntity(pickup);
                    }
                }

                yield return TIMER_MAX;
            }
        }
    }
}
