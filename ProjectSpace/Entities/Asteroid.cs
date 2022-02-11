using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities
{
    public class Asteroid : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private bool _outOfBounds;

        public Vector2 Velocity { get; set; }
        public float RotationSpeed { get; set; }

        public Asteroid(Sprite sprite)
        {
            float size = sprite.Animations["default"].Frames[0].Width;
            _transform = new Transform2D(this);
            _renderer = new SpriteRenderer(this) { Sprite = sprite, Animation = "default", Origin = new Vector2(size / 2.0f, size / 2.0f) };
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(0, 0, size, size) { Origin = new Vector2(size / 2.0f, size / 2.0f) }, Tag = Constants.TAG_ASTEROID };
            SetRotationSpeed();
            _outOfBounds = false;
        }

        public override void Update(GameTime gameTime)
        {
            _transform.Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _collider.Shape.Position = _transform.Position;
            _renderer.Rotation += RotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(!Camera.Bounds.Contains(_transform.Position))
            {
                _outOfBounds = true;
                Destroy();
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {
            if (_outOfBounds) return;

            if(_renderer.Sprite == AssetLibrary.GetAsset<Sprite>("sprAsteroid"))
            {
                // Generate asteroids
                int numAsteroids = Constants.Random.Next(2, 5);

                float rotation = (float)Constants.Random.NextDouble() * MathHelper.TwoPi;
                float increment = MathHelper.TwoPi / numAsteroids;

                for (int i = 0; i < numAsteroids; i++)
                {
                    Asteroid asteroid = new Asteroid(AssetLibrary.GetAsset<Sprite>("sprAsteroidSmall"));

                    asteroid.Velocity = new Vector2(
                        MathF.Cos(rotation) * Constants.Random.Next(10, 30),
                        MathF.Sin(rotation) * Constants.Random.Next(10, 30)
                        );

                    asteroid.GetComponent<Transform2D>().Position = new Vector2(
                        _transform.Position.X + MathF.Cos(rotation) * 8,
                        _transform.Position.Y + MathF.Sin(rotation) * 8
                        );

                    Scene.AddEntity(asteroid);

                    rotation += increment;
                }
                Camera.ShakeScreen(2.0f, 0.2);
            }
            else
            {
                Camera.ShakeScreen(1.0f, 0.2);

                // Create Fragments
                Sprite sprite = AssetLibrary.GetAsset<Sprite>("sprAsteroidFragment");
                for(int i = 0; i < sprite.Animations["default"].Frames.Length; i++)
                {
                    Fragment fragment = new Fragment(sprite, i);
                    fragment.GetComponent<Transform2D>().Position = _transform.Position;
                    fragment.GetComponent<SpriteRenderer>().Rotation = _renderer.Rotation;
                    Scene.AddEntity(fragment);
                }
            }
        }

        private void SetRotationSpeed()
        {
            RotationSpeed = (((float)Constants.Random.NextDouble()) * 2 - 1) * 3;
        }
    }
}
