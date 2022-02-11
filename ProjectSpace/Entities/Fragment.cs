using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSpace.Entities
{
    public class Fragment : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Vector2 _velocity;
        private float _rotationSpeed;
        private double _lifeTime;

        private const double LIFE_TIME = 2.0;

        public Fragment(Sprite sprite, int frame)
        {
            _transform = new Transform2D(this);
            _renderer = new SpriteRenderer(this) { Sprite = sprite, Animation = "default", FrameIndex = frame, Origin = new Vector2(sprite.Animations["default"].Frames[0].Width/2.0f, sprite.Animations["default"].Frames[0].Height / 2.0f) };

            _velocity = new Vector2(
                Constants.Random.Next(-5, 5),
                Constants.Random.Next(-5, 5)
                );

            _rotationSpeed = (float)(Constants.Random.NextDouble() * 2 - 1) * 0.2f;

            _lifeTime = LIFE_TIME;
        }

        public override void Update(GameTime gameTime)
        {
            _transform.Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _renderer.Rotation += _rotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;

            _renderer.Color = Color.FromNonPremultiplied(255, 255, 255, (int)((_lifeTime / LIFE_TIME) * 255));

            if(_lifeTime < 0.0)
            {
                Destroy();
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {
            
        }
    }
}
