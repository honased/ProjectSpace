using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectSpace.Entities
{
    public class AsteroidSpawner : Entity
    {
        private Coroutine _routine;

        public AsteroidSpawner()
        {
            _routine = new Coroutine(this, Spawner());
            _routine.Start();
        }

        private IEnumerator<double> Spawner()
        {
            while(true)
            {
                var bounds = Camera.Bounds;

                var asteroid = new Asteroid(AssetLibrary.GetAsset<Sprite>("sprAsteroid"));
                asteroid.GetComponent<Transform2D>().Position = new Vector2(
                    (float)Constants.Random.NextDouble() * bounds.Width,
                    (float)Constants.Random.NextDouble() * bounds.Height
                    );

                if (Constants.Random.Next(0, 2) == 0)
                {
                    asteroid.GetComponent<Transform2D>().Position *= Vector2.UnitX;
                    if (Constants.Random.Next(0, 2) == 0) asteroid.GetComponent<Transform2D>().Position += new Vector2(0, bounds.Height);
                }
                else
                {
                    asteroid.GetComponent<Transform2D>().Position *= Vector2.UnitY;
                    if (Constants.Random.Next(0, 2) == 0) asteroid.GetComponent<Transform2D>().Position += new Vector2(bounds.Width, 0);
                }

                asteroid.GetComponent<Transform2D>().Position += new Vector2(bounds.X, bounds.Y);

                Vector2 velocity = new Vector2(Camera.Bounds.Width / 2.0f, Camera.Bounds.Height / 2.0f);
                velocity -= asteroid.GetComponent<Transform2D>().Position;
                velocity.Normalize();
                velocity *= Constants.Random.Next(10, 45);
                asteroid.Velocity = velocity;

                Scene.AddEntity(asteroid);

                yield return 1.0f;
            }
        }

        protected override void Cleanup()
        {
            _routine.Enabled = false;
        }
    }
}
