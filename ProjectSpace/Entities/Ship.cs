using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjectSpace.Entities
{
    public class Ship : Entity
    {
        private Collider2D _collider;
        private SpriteRenderer _renderer;
        private Transform2D _transform;

        private Vector2 _velocity;
        private float _direction;

        private const float DIRECTION_SPEED = MathHelper.Pi;
        private const float MOVEMENT_SPEED = 3.0f;
        private const float MAX_VELOCITY = 205.0f;

        public Ship()
        {
            _transform = new Transform2D(this) { Position = Camera.CameraSize / 2.0f };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprShip"), Origin = new Vector2(8, 8) };

            _collider = new Collider2D(this) { Shape = new BoundingRectangle(0, 0, 8, 8) { Origin = new Vector2(4, 2) } };

            _velocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            bool createTrail = false;

            if (Input.IsKeyDown(Keys.D)) _direction += DIRECTION_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.IsKeyDown(Keys.A)) _direction -= DIRECTION_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Input.IsKeyDown(Keys.D) || Input.IsKeyDown(Keys.A)) _renderer.Scale = new Vector2(1.0f, MathHelper.Lerp(_renderer.Scale.Y, 0.85f, 0.05f));
            else _renderer.Scale = new Vector2(1.0f, MathHelper.Lerp(_renderer.Scale.Y, 1.0f, 0.05f));

            if (Input.IsKeyDown(Keys.W))
            {
                _velocity += new Vector2(MathF.Cos(_direction), MathF.Sin(_direction)) * MOVEMENT_SPEED;
                createTrail = true;
            }

            if (_velocity.Length() > MAX_VELOCITY)
            {
                _velocity.Normalize();
                _velocity *= MAX_VELOCITY;
            }

            _velocity.X = MathHelper.Lerp(_velocity.X, 0.0f, 0.01f);
            _velocity.Y = MathHelper.Lerp(_velocity.Y, 0.0f, 0.01f);

            _renderer.Rotation = _direction;

            _transform.Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_transform.Position.X - 8 < 0 || _transform.Position.X + 8 > Camera.CameraSize.X)
            {
                _transform.Position = new Vector2(MathHelper.Clamp(_transform.Position.X, 0, Camera.CameraSize.X), _transform.Position.Y);
                _velocity.X *= -1;
                Camera.ShakeScreen(1.0f, 0.1);
            }
            if (_transform.Position.Y - 8 < 0 || _transform.Position.Y + 8 > Camera.CameraSize.Y)
            {
                _transform.Position = new Vector2(_transform.Position.X, MathHelper.Clamp(_transform.Position.Y, 0, Camera.CameraSize.Y));
                _velocity.Y *= -1;
                Camera.ShakeScreen(1.0f, 0.1);
            }

            _collider.Shape.Position = _transform.Position;

            if(_collider.CollidesWith(Constants.TAG_ASTEROID, out Entity e))
            {
                Destroy();
                if (e is Asteroid roid) roid.FullDestroy = true;
                e.Destroy();
                Scene.GetEntity<AsteroidSpawner>()?.Destroy();
                Scene.AddEntity(new Menu.MenuTransition());
            }

            // Create laser
            if(Input.IsKeyPressed(Keys.Space))
            {
                const float WIDTH = 8.0f;
                const float HEIGHT = 0.0f;

                Laser laser = new Laser();
                laser.GetComponent<Transform2D>().Position = new Vector2(
                    _transform.Position.X + (WIDTH * MathF.Cos(_direction)) + (HEIGHT * -MathF.Sin(_direction)),
                    _transform.Position.Y + (WIDTH * MathF.Sin(_direction)) + (HEIGHT * MathF.Cos(_direction)));
                laser.Velocity = new Vector2(MathF.Cos(_direction), MathF.Sin(_direction)) * 300;
                Scene.AddEntity(laser);

                Camera.ShakeScreen(0.5f, 0.05);
            }

            // Create trail
            if(createTrail)
            {
                float dir = (float)Math.Atan2(_velocity.Y, _velocity.X);
                ShipTrail trail = new ShipTrail(_velocity.Length() / 8.0f, dir);
                trail.Position = new Vector2(
                    _transform.Position.X - (6 * MathF.Cos(_direction)),
                    _transform.Position.Y - (6 * MathF.Sin(_direction))
                );
                Scene.AddEntity(trail);
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {
            // Create Fragments
            Sprite sprite = AssetLibrary.GetAsset<Sprite>("sprShipFragment");
            for (int i = 0; i < sprite.Animations["default"].Frames.Length; i++)
            {
                Fragment fragment = new Fragment(sprite, i);
                fragment.GetComponent<Transform2D>().Position = _transform.Position;
                fragment.GetComponent<SpriteRenderer>().Rotation = _renderer.Rotation;
                Scene.AddEntity(fragment);
            }
        }
    }
}
