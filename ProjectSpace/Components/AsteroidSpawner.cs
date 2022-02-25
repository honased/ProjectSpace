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
    public class AsteroidSpawner : Component
    {
        private const float TIMER_MAX = 2.0f;
        private const float TIMER_MIN = 0.2f;

        private Coroutine _routine;
        private float _timer;

        public AsteroidSpawner(Entity parent) : base(parent)
        {
            _timer = TIMER_MAX;

            _routine = new Coroutine(parent, Spawner());
            _routine.Start();
        }

        private IEnumerator<double> Spawner()
        {
            while (true)
            {
                var bounds = Camera.Bounds;

                var asteroid = new Asteroid(AssetLibrary.GetAsset<Sprite>("sprAsteroid"));
                asteroid.GetComponent<Transform2D>().Position = new Vector2(
                    (float)Globals.Random.NextDouble() * bounds.Width,
                    (float)Globals.Random.NextDouble() * bounds.Height
                    );

                if (Globals.Random.Next(0, 2) == 0)
                {
                    asteroid.GetComponent<Transform2D>().Position *= Vector2.UnitX;
                    if (Globals.Random.Next(0, 2) == 0) asteroid.GetComponent<Transform2D>().Position += new Vector2(0, bounds.Height);
                }
                else
                {
                    asteroid.GetComponent<Transform2D>().Position *= Vector2.UnitY;
                    if (Globals.Random.Next(0, 2) == 0) asteroid.GetComponent<Transform2D>().Position += new Vector2(bounds.Width, 0);
                }

                asteroid.GetComponent<Transform2D>().Position += new Vector2(bounds.X, bounds.Y);

                Vector2 velocity = new Vector2(Camera.Bounds.Width / 2.0f, Camera.Bounds.Height / 2.0f);
                velocity -= asteroid.GetComponent<Transform2D>().Position;
                velocity.Normalize();
                velocity *= Globals.Random.Next(10, 45);
                asteroid.Velocity = velocity;

                Scene.AddEntity(asteroid);

                yield return _timer;

                _timer = MathHelper.SmoothStep(_timer, TIMER_MIN, 0.05f);
            }
        }
    }
}
